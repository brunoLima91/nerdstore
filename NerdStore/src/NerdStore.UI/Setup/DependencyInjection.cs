using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Data;
using NerdStore.Catalogo.Data.Repository;
using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Core.Bus;
using NerdStore.Core.Messages.CommonMessages.Notification;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Application.Queries;
using NerdStore.Vendas.Data;

namespace NerdStore.UI.Setup
{
	public static class DependencyInjection
	{
		public static void RegisterServices(this IServiceCollection services)
		{
			// Mediator
			services.AddScoped<IMediatorHandler, MediatorHandler>();

			//Notifications
			services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

			//Catalogo
			services.AddScoped<IProdutoRepository, ProdutoRepository>();
			services.AddScoped<IProdutoAppService, ProdutoAppService>();
			services.AddScoped<IEstoqueService, EstoqueService>();
			services.AddScoped<CatalogoContext>();
			services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();

			//Vendas
			services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
			services.AddScoped<IPedidoRepository, IPedidoRepository>();
			services.AddScoped<IPedidoQueries, PedidosQueries>();

			services.AddScoped<INotificationHandler<PedidoRascunhoIniciadoEvent>, PedidoEventHandler>();
			services.AddScoped<INotificationHandler<PedidoAtualizadoEvent>, PedidoEventHandler>();
			services.AddScoped<INotificationHandler<PedidoItemAdicionadoEvent>, PedidoEventHandler>();


		}
	}
}
