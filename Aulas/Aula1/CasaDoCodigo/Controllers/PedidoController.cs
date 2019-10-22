using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IItemPedidoRepository _itemPedidoRepository;

        public PedidoController(IProdutoRepository produtoRepository, IPedidoRepository pedidoRepository,
            IItemPedidoRepository itemPedidoRepository)
        {
            _produtoRepository = produtoRepository;
            _pedidoRepository = pedidoRepository;
            _itemPedidoRepository = itemPedidoRepository;
        }

        public IActionResult Carrossel()
        {
            return View(_produtoRepository.GetProdutos());
        }


        public IActionResult Cadastro()
        {
            return View();
        }

        public IActionResult Carrinho(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                _pedidoRepository.Additem(codigo);
            }

            List<ItemPedido> itens = _pedidoRepository.GetPedido().Itens;
            CarrinhoViewModel carrinho = new CarrinhoViewModel(itens);

            return base.View(carrinho);
        }

        public IActionResult Resumo()
        {
            var pedido = _pedidoRepository.GetPedido();

            return View(pedido);
        }

        [HttpPost]
        public void UpdateQuantidade([FromBody] ItemPedido itemPedido)
        {
            _itemPedidoRepository.UpdateQuantidade(itemPedido);
        }
    }
}
