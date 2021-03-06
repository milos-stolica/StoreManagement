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
                                                   new FileReader(),
                                                   new Dictionary<string, IJsonValidatableProductFactory>
                                                   {
                                                       { ProductTypes.Fruit, new JsonValidatableFruitFactory(x.GetRequiredService<ILogger<FruitValidator>>()) },
                                                       { ProductTypes.Coffee, new JsonValidatableCoffeeFactory(x.GetRequiredService<ILogger<CoffeeValidator>>()) }
                                                   },
                                                   x.GetRequiredService<ILogger<JsonProductsLoader>>()
                                                   ));

            services.AddSingleton<IOrderingService>(x => 
                            new OrderingService(new InMemoryProductsRepository(x.GetRequiredService<IProductsLoader>()),
                                                new OrderDetailsFactory(),
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
