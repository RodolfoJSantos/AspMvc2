using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
	public interface ICadastroRepository
	{
		Cadastro Update(int cadastroId, Cadastro novoCadastro);
	}
	public class CadastroRepository : BaseRepository<Cadastro>, ICadastroRepository
	{
		public CadastroRepository(ApplicationContext contexto) : base(contexto)
		{
		}

		public Cadastro Update(int cadastroId, Cadastro novoCadastro)
		{
			var cadastroDb = _dbSet.Where(c => c.Id == cadastroId)
				.SingleOrDefault();

			if (cadastroDb == null)
			{
				throw new ArgumentException("cadastro");
			}

			cadastroDb.Update(novoCadastro);
			_contexto.SaveChanges();

			return cadastroDb;
		}
	}
}
