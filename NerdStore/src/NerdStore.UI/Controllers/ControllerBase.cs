using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Core.Bus;
using NerdStore.Core.Messages.CommonMessages.Notification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace NerdStore.UI.Controllers
{
	public abstract class ControllerBase : Controller
	{
		private readonly DomainNotificationHandler _notifications;
		private readonly IMediatorHandler _mediatorHandler;
		protected Guid ClientId = Guid.Parse("7DE4D6DB-B6AE-44F1-A191-2C5700E1FB1C");

		public ControllerBase(INotificationHandler<DomainNotification> notification, IMediatorHandler mediatorHandler)
		{
			_notifications = (DomainNotificationHandler)notification;
			_mediatorHandler = mediatorHandler;
		}

		protected bool OperacaoValida()
		{
			return !_notifications.TemNotificacao();
		}

		protected IEnumerable<string> ObterMensagensErro()
		{
			return _notifications.ObterNotificacoes().Select(c => c.Value).ToList();
		}

		protected void NotificarErro(string codigo, string mensagem)
		{
			_mediatorHandler.PublicarNotificacao(new DomainNotification(codigo, mensagem));
		}
	}
}
