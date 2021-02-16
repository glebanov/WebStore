using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Middleware;
using WebStore.Infrastructure.Services;
using WebStore.DAL.Context;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.Infrastructure.Services.InSQL;
using WebStore.Infrastructure.Services.InMemory;

namespace WebStore
{
    public record Startup(IConfiguration Configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<WebStoreDB>(opt => opt.UseSqLite(Configuration.GetConnectionString("Sqlite")));//Подключение другого сервера
            services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddTransient<WebStoreDbInitializer>();

            services.AddTransient<IEmployeesData, InMemoryEmployeesData>();
            //services.AddTransient<IProductData, InMemoryProductData>();
            services.AddTransient<IProductData, SqlProductData>();

            services
               .AddControllersWithViews()
               .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebStoreDbInitializer db)
        {
            db.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseWelcomePage("/welcome");

            app.UseMiddleware<TestMiddleware>();

            app.MapWhen(
                context => context.Request.Query.ContainsKey("id") && context.Request.Query["id"] == "5",
                context => context.Run(async request => await request.Response.WriteAsync("Hello with id == 5!"))
            );
            app.Map("/hello", context => context.Run(async request => await request.Response.WriteAsync("Hello!!!")));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
