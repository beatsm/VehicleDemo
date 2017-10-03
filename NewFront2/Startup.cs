using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using NewFront2;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace NewFront2
{




    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}