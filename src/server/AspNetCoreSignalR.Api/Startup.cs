using AspNetCoreSignalR.Api.Hubs;
using AspNetCoreSignalR.Common.Events;
using AspNetCoreSignalR.Core;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspNetCoreSignalR.Api
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //SignalR
            services.AddSignalR().AddMessagePackProtocol();

            //Swagger-ui 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "SignalR",
                    Version = "v1",
                    Description = "Welcome!",
                });
            });

            //Cors
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    config => config
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            //Autofac
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CoreModule());

            // Add framework services.
            services.AddMvc();

            //Automapper
            services.AddAutoMapper(typeof(Startup).Assembly);

            //Domain event
            RegisterEventHandlers(builder);

            builder.Populate(services);
            ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            //Swagger-ui
            app.UseSwagger(c => c.RouteTemplate = "api-endpoints/{documentName}/swagger.json");
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api-endpoints";
                c.SwaggerEndpoint("v1/swagger.json", "SignalR V1");
            });

            //Cors
            app.UseCors("CorsPolicy");

            app.UseSignalR(routes => routes.MapHub<ChatHub>("/chat"));

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }

        private void RegisterEventHandlers(ContainerBuilder builder)
        {
            IEnumerable<Assembly> assemblies = GetAssemblies();

            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .AsClosedTypesOf(typeof(IEventHandler<>))
                .InstancePerLifetimeScope();
        }

        private static Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            foreach (var library in DependencyContext.Default.RuntimeLibraries)
            {
                if (library.Name.StartsWith("SignalR"))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }
            return assemblies.ToArray();
        }
    }
}
