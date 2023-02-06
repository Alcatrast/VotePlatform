using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotePlatform.Models.Votes;

namespace VotePlatform
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
          //  services.AddTransient<IVote,Vote>();
            services.AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                  pattern: "{controller=Users}/{action=SingIn}");

                //endpoints.MapControllerRoute(
                //  name: "default",
                // pattern: "{controller=Users}/{action}");

                endpoints.MapControllerRoute(

                    name: "default",
                   pattern: "{controller}/{action}/{id?}");
            });
            DBBuilder.Build();
        }
    }
}
