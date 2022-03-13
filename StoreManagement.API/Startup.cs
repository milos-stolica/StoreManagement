using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StoreManagement.API.Common;
using StoreManagement.API.Entities;
using StoreManagement.API.Repositories;
using StoreManagement.API.Services;
using StoreManagement.API.Services.OrderingService;
using StoreManagement.API.Utils;

namespace StoreManagement.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<IProductsLoader>(x => 
                            new JsonProductsLoader("TestData.json",
                                                   new Dictionary<string, IJsonProductFactory>
                                                   {
                                                       { ProductTypes.Fruit, new FruitFactory() },
                                                       { ProductTypes.Coffee, new CoffeeFactory() }
                                                   },
                                                   new Dictionary<string, IValidator>
                                                   {
                                                       { ProductTypes.Fruit, new FruitValidator(x.GetRequiredService<ILogger<FruitValidator>>()) },
                                                       { ProductTypes.Coffee, new CoffeeValidator(x.GetRequiredService<ILogger<CoffeeValidator>>()) }
                                                   },
                                                   x.GetRequiredService<ILogger<JsonProductsLoader>>()
                                                   ));

            services.AddSingleton<IOrderDetailsFactory, OrderDetailsFactory>();

            services.AddSingleton<IProductsRepository>(x => 
                            new InMemoryProductsRepository(x.GetRequiredService<IProductsLoader>()));

            services.AddSingleton<IOrderingService>(x => 
                            new OrderingService(x.GetRequiredService<IProductsRepository>(),
                                                x.GetRequiredService<IOrderDetailsFactory>(),
                                                new Dictionary<EntityStatus, IErrorMessageFactory<OrderedProductDetails>>
                                                {
                                                    { EntityStatus.UnprocessableUpdate, new OutOfStockErrorFactory() }
                                                    //add more message factories if there is need for more detailed errors
                                                },
                                                x.GetRequiredService<IMapper>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
