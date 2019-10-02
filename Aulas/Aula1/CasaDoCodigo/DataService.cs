using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo
{
	public class DataService : IDataService
	{
		ApplicationContext contexto;
		public DataService(ApplicationContext contexto)
		{
			this.contexto = contexto;
		}

		public void InicializaDb()
		{
			contexto.Database.EnsureCreated();

			var json = File.ReadAllText("livros.json");
			var livro = JsonConvert.DeserializeObject<List<Livro>>(json);
		}

		class Livro
		{
			public string Codigo { get; set; }
			public string Nome { get; set; }
			public decimal Preco { get; set; }
		}
	}
}
