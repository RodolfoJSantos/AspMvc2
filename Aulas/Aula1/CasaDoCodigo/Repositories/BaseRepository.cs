using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
	public class BaseRepository<T> where T : BaseModel
	{
		protected readonly ApplicationContext _contexto;
		protected readonly DbSet<T> _dbSet;

		public BaseRepository(ApplicationContext contexto)
		{
			_contexto = contexto;
			_dbSet = _contexto.Set<T>();
		}
	}
}
