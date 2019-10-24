using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
	public interface IItemPedidoRepository
	{
		ItemPedido getItemPedido(int ItemPedidoId);
		void RemoveItemPedido(int ItemPedidoId);
	}
	public class ItemPedidoRepository : BaseRepository<ItemPedido>, IItemPedidoRepository
	{
		public ItemPedidoRepository(ApplicationContext contexto) : base(contexto)
		{
		}

		public ItemPedido getItemPedido(int ItemPedidoId)
		{
			return _dbSet
				.Where(ip => ip.Id == ItemPedidoId)
				.SingleOrDefault();
		}

		public void RemoveItemPedido(int itemPedidoId)
		{
			_dbSet.Remove(getItemPedido(itemPedidoId));
		}
	}
}
