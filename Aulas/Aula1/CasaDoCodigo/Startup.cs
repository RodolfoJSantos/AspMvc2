﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CasaDoCodigo
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		//Adiciona os serviços
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
            services.AddDistributedMemoryCache();
            services.AddSession();


			var connecitonString = Configuration.GetConnectionString("Default");
			services.AddDbContext<ApplicationContext>(options => 
						options.UseSqlServer(connecitonString));

			services.AddTransient<IDataService, DataService>();
			services.AddTransient<IProdutoRepository, ProdutoRepository>();
			services.AddTransient<IPedidoRepository, PedidoRepository>();
			services.AddTransient<IItemPedidoRepository, ItemPedidoRepository>();
			services.AddTransient<ICadastroRepository, CadastroRepository>();
		}

		// Console os serviços
		public void Configure(IApplicationBuilder app, IHostingEnvironment env,
			IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
            app.UseSession();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Pedido}/{action=Carrossel}/{codigo?}");
			});

			//caso base seja apagada, cria novamente quando a aplicação for executada
			//método garanta que tenha sido criado
			serviceProvider.GetService<IDataService>().InicializaDb();
		}
	}

}
