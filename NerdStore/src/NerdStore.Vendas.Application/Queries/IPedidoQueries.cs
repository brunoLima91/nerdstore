using NerdStore.Vendas.Application.Queries.DTO;
using NerdStore.Vendas.Data;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Queries
{
	public interface IPedidoQueries
	{
		Task<CarrinhoDTO> ObterCarrinhoCliente(Guid clienteId);
		Task<IEnumerable<PedidoDTO>> ObterPedidosCLiente(Guid clienteId);
	}

	public class PedidosQueries : IPedidoQueries
	{
		private readonly IPedidoRepository _pedidoRepository;
		public PedidosQueries(IPedidoRepository pedidoRepository)
		{
			_pedidoRepository = pedidoRepository;
		}
		public async Task<CarrinhoDTO> ObterCarrinhoCliente(Guid clienteId)
		{
			var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(clienteId);
			if (pedido == null)
				return null;

			var carrinho = new CarrinhoDTO
			{
				ClienteId = pedido.ClientId,
				ValorTotal = pedido.ValorTotal,
				PedidoId = pedido.Id,
				ValorDesconto = pedido.Desconto,
				SubTotal = pedido.Desconto + pedido.ValorTotal
			};

			carrinho.VoucherCodigo = pedido.Voucher?.Codigo;

			foreach (var item in pedido.PedidoItems)
			{
				carrinho.Items.Add(new CarrinhoItemDTO
				{
					ProdutoId = item.ProdutoId,
					ProdutoNome = item.ProdutoNome,
					Quantidade = item.Quantidade,
					ValorTotal = item.Quantidade * item.ValorUnitario,
					ValorUnitario = item.ValorUnitario

				});
			}

			return carrinho;

		}

		public async Task<IEnumerable<PedidoDTO>> ObterPedidosCLiente(Guid clienteId)
		{
			var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);

			pedidos = pedidos.Where(p => p.PedidoStatus == PedidoStatus.Pago || p.PedidoStatus == PedidoStatus.Cancelado)
				.OrderByDescending(p => p.Codigo);

			if (!pedidos.Any())
				return null;

			var pedidosDto = new List<PedidoDTO>();

			foreach (var item in pedidos)
			{
				pedidosDto.Add(new PedidoDTO
				{
					Codigo = item.Codigo,
					DataCadastro = item.DataCadastro,
					PedidoStatus = (int)item.PedidoStatus,
					ValorTotal = item.ValorTotal,

				});
			}

			return pedidosDto;
		}
	}
}
