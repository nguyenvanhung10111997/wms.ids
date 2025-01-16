// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Autofac.Extensions.DependencyInjection;
using wms.ids.web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace IdentityServerHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                   .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                   .ConfigureWebHostDefaults(webHostBuilder =>
                   {
                       webHostBuilder
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseStartup<Startup>();
                   });
    }
}