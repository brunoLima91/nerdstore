using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Core.Bus;
using NerdStore.Core.Messages.CommonMessages.Notification;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Queries;
using System;
using System.Threading.Tasks;

namespace NerdStore.UI.Controllers
{
    public class CarrinhoController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IPedidoQueries _pedidoQueries;

        public CarrinhoController(INotificationHandler<DomainNotification> notifications,            
            IProdutoAppService produtoAppService, IPedidoQueries pedidoQueries ,IMediatorHandler mediatorHandler):base(notifications,mediatorHandler)
        {
            _produtoAppService = produtoAppService;
            _mediatorHandler = mediatorHandler;
            _pedidoQueries = pedidoQueries;
        }

        [Route("meu-carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _pedidoQueries.ObterCarrinhoCliente(ClientId)) ;
        }

        [HttpPost]
        [Route("meu-carrinho")]
        public async Task<IActionResult> AdicionarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null) return BadRequest();

            if (produto.QuantidadeEstoque < quantidade)
            {
                TempData["Erro"] = "Produto com estoque insuficiente";
                return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
            }

            var command = new AdicionarItemPedidoCommand(ClientId, produto.Id, produto.Nome, quantidade, produto.Valor);
            await _mediatorHandler.EnviarComando(command);

            if (OperacaoValida())
            {
                return RedirectToAction("Index");
            }
            
            TempData["Erros"] = ObterMensagensErro();
            return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
        }

        //[HttpPost]
        //[Route("remover-item")]

        //public async Task<IActionResult> RemoverItem(Guid id)
        //{
        //    var produto = await _produtoAppService.ObterPorId(id);
        //    if (produto == null)
        //        return BadRequest();

        //    var command = new RemoverItemPedidoCommand(ClientId, id);
        //    await _mediatorHandler.EnviarComando(command);

        //    if (OperacaoValida())
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClientId));
        //}

        //[HttpPost]
        //[Route("atualizar-item")]

        //public async Task<IActionResult> AtualizarItem(Guid id, int quantidade)
        //{
        //    var produto = await _produtoAppService.ObterPorId(id);
        //    if (produto == null)
        //        return BadRequest();

        //    var command = new AtualizarItemPedidoCommand(ClientId, id);
        //    await _mediatorHandler.EnviarComando(command);

        //    if (OperacaoValida())
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClientId));
        //}

        //[HttpPost]
        //[Route("aplicar-voucher")]

        //public async Task<IActionResult> ApplicarVoucher(string voucherCodigo)
        //{

        //    var command = new AplicarVoucherPedidoCommand(ClientId, voucherCodigo);
        //    await _mediatorHandler.EnviarComando(command);

        //    if (OperacaoValida())
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClientId));
        //}
    }
}