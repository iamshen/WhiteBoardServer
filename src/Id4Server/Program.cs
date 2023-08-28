// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Id4Server;

public class Program
{
    public static int Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                theme: AnsiConsoleTheme.Code)
            .CreateLogger();

        try
        {
            Log.Information("启动 IdentityServer4 认证服务...");
            CreateHostBuilder(args);
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "主机意外终止。");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static void CreateHostBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder();

        #region 服务

        builder.Services.AddControllersWithViews();

        #endregion

        #region IdentityServer

        var identityServerBuilder = builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
            // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
            options.EmitStaticAudienceClaim = false;
        }).AddTestUsers(TestUsers.Users);

        // in-memory, code config
        identityServerBuilder.AddInMemoryIdentityResources(Config.IdentityResources);
        identityServerBuilder.AddInMemoryApiScopes(Config.ApiScopes);
        identityServerBuilder.AddInMemoryClients(Config.Clients);

        // not recommended for production - you need to store your key material somewhere secure
        identityServerBuilder.AddDeveloperSigningCredential();

        #endregion

        #region AddAuthentication

        builder.Services.AddAuthentication()
            // 这里可以注册第三方认证服务: Google、QQ、 Wechat、Github
            ;

        #endregion

        var app = builder.Build();

        app.UseStaticFiles();

        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.MapDefaultControllerRoute();

        app.Run();
    }
}