﻿using Castle.MicroKernel.Registration;
using Radical.Bootstrapper;
using System;
using System.IO;
using System.Net.Http.Formatting;
using System.Web.Http;
using Shipping.Data.Context;

namespace Shipping.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            var bootstrapper = new WindsorBootstrapper(Path.Combine(basePath, "bin"));
            var container = bootstrapper.Boot();

            container.Register(Component.For<IShippingContext>()
                .Instance(new ShippingContext())
                .LifestylePerWebRequest());

            GlobalConfiguration.Configure(http => WebApiConfig.Register(http, container));
        }
    }
}
