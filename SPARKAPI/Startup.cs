using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(SPARKAPI.Startup))]

namespace SPARKAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {



            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            ConfigureAuth(app);
        }
    }
}
