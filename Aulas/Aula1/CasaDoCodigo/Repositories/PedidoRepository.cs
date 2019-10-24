using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface IPedidoRepository
    {
        Pedido GetPedido();
        void Additem(string codigo);
		UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido);

	}
    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
	{
        private readonly IHttpContextAccessor _contextAccessor;
		private readonly IItemPedidoRepository _itemPedidoRepository;

		public PedidoRepository(ApplicationContext contexto, IHttpContextAccessor contextAccessor,
								IItemPedidoRepository itemPedidoRepository) : base(contexto)
		{
            _contextAccessor = contextAccessor;
			_itemPedidoRepository = itemPedidoRepository;
		}

        private int? GetPedidoId()
        {
            return _contextAccessor.HttpContext.Session.GetInt32("pedidoId");
        }

        private void SetPedidoId(int pedidoId)
        {
            _contextAccessor.HttpContext.Session.SetInt32("pedidoId", pedidoId);
        }


        public Pedido GetPedido()
        {
            var pedidoId = GetPedidoId();
            var pedido = _dbSet
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .Where(p => p.Id == pedidoId)
                .SingleOrDefault();

            if (pedido == null)
            {
                pedido = new Pedido();
                _dbSet.Add(pedido);
                _contexto.SaveChanges();
                SetPedidoId(pedido.Id);
            }

            return pedido;
        }

        public void Additem(string codigo)
        {
            var produto = _contexto.Set<Produto>().Where(p => p.Codigo == codigo).SingleOrDefault();

            if (produto == null)
            {
                throw new ArgumentException("Produto não encontrado");
            }
            var pedido = GetPedido();
            var itemPedido = _contexto.Set<ItemPedido>()
                                        .Where(i => i.Produto.Codigo == codigo
                                                    && i.Pedido.Id == pedido.Id)
                                        .SingleOrDefault();
            if (itemPedido == null)
            {
                itemPedido = new ItemPedido(pedido, produto, 1, produto.Preco);
                _contexto.Set<ItemPedido>().Add(itemPedido);
                _contexto.SaveChanges();
            }
        }

		public UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido)
		{
			var itemPedidoDb = _itemPedidoRepository.getItemPedido(itemPedido.Id);
			if (itemPedidoDb != null)
			{
				itemPedidoDb.AtualizaQuantidade(itemPedido.Quantidade);

				if (itemPedido.Quantidade == 0)
				{
					_itemPedidoRepository.RemoveItemPedido(itemPedido.Id);
				}

				_contexto.SaveChanges();

				var carrinhoViewModel = new CarrinhoViewModel(GetPedido().Itens);

				return new UpdateQuantidadeResponse(itemPedidoDb, carrinhoViewModel);
			}

			throw new ArgumentException("ItemPedido não encontrado.");
		}
	}
}
