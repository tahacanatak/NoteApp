using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NoteApp.API.Startup))]

namespace NoteApp.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //API izinleri için owin cors kurduktan sonra
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            ConfigureAuth(app);
        }
    }
}
