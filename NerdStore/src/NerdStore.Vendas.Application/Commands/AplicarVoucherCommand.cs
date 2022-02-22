using FluentValidation;
using NerdStore.Core.Messages;
using System;

namespace NerdStore.Vendas.Application.Commands
{
	public class AplicarVoucherCommand : Command
	{
		public Guid ClienteId { get; private set; }
		public Guid PedidoId { get; private set; }
		public string CodigoVoucher { get; private set; }

		public AplicarVoucherCommand(Guid clienteId, Guid pedidoId, string codigoVoucher)
		{
			ClienteId = clienteId;
			PedidoId = pedidoId;
			CodigoVoucher = codigoVoucher;
		}

		public override bool EhValido()
		{
			ValidationResult = new AplicarVoucherValidation().Validate(this);
			return ValidationResult.IsValid;
		}
	}

	public class AplicarVoucherValidation : AbstractValidator<AplicarVoucherCommand>
	{
		public AplicarVoucherValidation()
		{
			RuleFor(c => c.ClienteId)
			.NotEqual(Guid.Empty)
			.WithMessage("Id do cliente inválido");

			RuleFor(c => c.PedidoId)
				.NotEqual(Guid.Empty)
				.WithMessage("Id do pedido inválido");

			RuleFor(c => c.CodigoVoucher)
				.NotEmpty()
				.WithMessage("O código do voucher não pode ser vazio");

		}
	}
}
