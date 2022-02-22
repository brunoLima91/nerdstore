using NerdStore.Core.Messages;
using System;

namespace NerdStore.Vendas.Application.Events
{
	public class PedidoRascunhoIniciadoEvent: Event
	{
		public Guid ClienteId { get; private set; }
		public Guid PedidoId { get; private set; }

		public PedidoRascunhoIniciadoEvent(Guid clientId, Guid pedidoId)
		{
			AggregateId = pedidoId;
			ClienteId = clientId;
			PedidoId = pedidoId;
		}
	}
}
