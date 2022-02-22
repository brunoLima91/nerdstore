using NerdStore.Core.Data;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Data
{
	public interface IPedidoRepository: IRepository<Pedido>
	{
		Task<Pedido> ObterPorId(Guid id);
		Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clientId);
		Task<Pedido> ObterPedidoRascunhoPorClienteId(Guid clientId);
		void Adicionar(Pedido pedido);
		void Atualizar(Pedido pedido);

		Task<PedidoItem> ObterItemPorId(Guid id);
		Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId);
		void AdicionarItem(PedidoItem pedidoItem);
		void AtualizarItem(PedidoItem pedidoItem);
		void RemoverItem(PedidoItem pedidoItem);

		Task<Voucher> ObterVoucherPorCodigo(string codigo);
	}
}
