﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Aiursoft.Stargate.Data;
using Aiursoft.Stargate.Services;
using Aiursoft.Pylon;
using Aiursoft.Pylon.Services;
using Microsoft.Extensions.Hosting;
using Aiursoft.Pylon.Services.ToStargateServer;
using Aiursoft.Pylon.Services.ToAPIServer;
using Aiursoft.Pylon.Models;
using System.Net.WebSockets;
using Aiursoft.Pylon.Services.ToArchonServer;

namespace Aiursoft.Stargate
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StargateDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection")));
            services.AddMvc();

            services.AddTokenManager();

            services.AddSingleton<ServiceLocation>();
            services.AddSingleton<IHostedService, TimedCleaner>();
            services.AddSingleton<Counter>();
            services.AddSingleton<AppsContainer>();
            services.AddSingleton<ArchonApiService>();
            services.AddSingleton<StargateMemory>();
            services.AddSingleton<TimeoutCleaner>();
            services.AddScoped<HTTPService>();
            services.AddScoped<ChannelService>();
            services.AddScoped<PushMessageService>();

            services.AddScoped<Debugger>();
            services.AddScoped<IPusher<WebSocket>, WebSocketPusher>();
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHandleRobots();
                app.UseEnforceHttps();
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseWebSockets();
            app.UseMvcWithDefaultRoute();
            app.UseAiursoftAuthenticationFromConfiguration(Configuration, "Test");
            app.UseDocGenerator();
        }
    }
}
