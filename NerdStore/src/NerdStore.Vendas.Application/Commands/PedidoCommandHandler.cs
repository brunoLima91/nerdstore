using MediatR;
using NerdStore.Core.Bus;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.Notification;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Data;
using NerdStore.Vendas.Domain;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
	public class PedidoCommandHandler :
		IRequestHandler<AdicionarItemPedidoCommand, bool>
	{
		private readonly IPedidoRepository _pedidoRepository;
		private readonly IMediatorHandler _mediatorHandler;

		public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediatorHandler mediatorHandler)
		{
			_pedidoRepository = pedidoRepository;
			_mediatorHandler = _mediatorHandler;
		}
		public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
		{
			if (!ValidarComando(message)) return false;

			var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClientId);
			var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);

			if (pedido == null)
			{
				pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClientId);
				pedido.AdicionarItem(pedidoItem);
				_pedidoRepository.Adicionar(pedido);
				pedido.AdicionarEvento(new PedidoRascunhoIniciadoEvent(message.ClientId, message.ProdutoId));
			}
			else
			{
				var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
				pedido.AdicionarItem(pedidoItem);
				if (pedidoItemExistente)
				{
					_pedidoRepository.AtualizarItem(pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId));
				}
				else
				{
					_pedidoRepository.AdicionarItem(pedidoItem);
				}

				pedido.AdicionarEvento(new PedidoAtualizadoEvent(message.ClientId, pedido.Id, pedido.ValorTotal));
			}

			pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClientId, pedido.Id, message.ProdutoId, message.Nome, message.ValorUnitario, message.Quantidade));
			return await _pedidoRepository.UnuitOfWork.Commit();
		}

		private bool ValidarComando(Command message)
		{
			if (message.EhValido())
				return true;

			foreach (var error in message.ValidationResult.Errors)
			{
				_mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
			}
			return false;
			
		}
	}
}
