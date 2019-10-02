using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            string connectionString = Configuration.GetConnectionString("Default");
            services.AddDbContext<ApplicantionContext>(options =>
                options.UseSqlServer(connectionString)
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		// Console os serviços
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Pedido}/{action=Carrossel}/{id?}");
            });

			//caso base seja apagada, cria novamente quando a aplicação for executada
			serviceProvider.GetService<ApplicantionContext>().Database.EnsureCreated();
        }
    }
}
