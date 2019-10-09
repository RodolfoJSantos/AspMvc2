using CasaDoCodigo.Models;
using Microsoft.AspNetCore.Http;
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
    }
    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
	{
        private readonly IHttpContextAccessor _contextAccessor;

		public PedidoRepository(ApplicationContext contexto, IHttpContextAccessor contextAccessor) : base(contexto)
		{
            _contextAccessor = contextAccessor;
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
            var pedido = _dbSet.Where(p => p.Id == pedidoId).SingleOrDefault();

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
    }
}
