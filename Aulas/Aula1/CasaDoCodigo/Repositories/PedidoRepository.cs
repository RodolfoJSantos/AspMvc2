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
            throw new NotImplementedException();
        }
    }
}
