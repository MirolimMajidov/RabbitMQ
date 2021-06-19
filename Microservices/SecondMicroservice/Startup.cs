using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SecondMicroservice.RabbitMQEvens.EventHandlers;
using SecondMicroservice.RabbitMQEvens.Events;
using System;
using System.Reflection;

namespace SecondMicroservice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQConnection>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<RabbitMQConnection>>();

                var rabbitMQHostName = Configuration["RabbitMQHostName"];
                if (string.IsNullOrEmpty(rabbitMQHostName))
                    rabbitMQHostName = "localhost";

                var factory = new ConnectionFactory
                {
                    HostName = rabbitMQHostName,
                    DispatchConsumersAsync = true
                };

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);

                return new RabbitMQConnection(factory, logger, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, EventBusSubscriptionsManager>();

            services.AddSingleton<IEventBusRabbitMQ, EventBusRabbitMQ>(serviceProvider =>
            {
                var subscriptionClientName = Configuration["SubscriptionClientName"];
                var logger = serviceProvider.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var rabbitMqConnection = serviceProvider.GetRequiredService<IRabbitMQConnection>();
                var iLifetimeScope = serviceProvider.GetRequiredService<ILifetimeScope>();
                var subscriptionsManager = serviceProvider.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMQ(rabbitMqConnection, subscriptionsManager, iLifetimeScope, logger,
                    subscriptionClientName);
            });

            services.AddControllers();

            //configure autofac
            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRabbitMQEventHandler<>));
            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBusRabbitMQ>();
            eventBus.Subscribe<FirstTestEvent, FirstTestEventHandler>();
            eventBus.Subscribe<SecondTestEvent, SecondTestEventHandler>();
        }
    }
}
