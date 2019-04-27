using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;
using ServiceStack.Text;
using Twitch.Monterey.Web.Contracts;
using Twitch.Monterey.Web.DB;
using Twitch.Monterey.Web.Managers;
using Twitch.Monterey.Web.WebSockets;

namespace Twitch.Monterey.Web
{
    public class Startup
    {
        //private IServiceCollection services;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //this.services = services;
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IRedisClientsManager>(new RedisManagerPool("localhost:6379"));
            services.AddSingleton<RoomDatabase>();
            services.AddSingleton<UserDatabase>();
            services.AddSingleton<RoomManager>();
            services.AddSingleton<Mapper>();
            JsConfig.ExcludeTypeInfo = true;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // configure websocket usage
            app.UseWebSockets();
            app.Use(async (context, next) => 
            {
                if(context.WebSockets.IsWebSocketRequest)
                {
                    // create the client socket connection
                    var socket = await context.WebSockets.AcceptWebSocketAsync();
                    var clientSocket = new ClientSocket(socket, context);
                    await clientSocket.OnConnectedAsync();
                    await clientSocket.ReceiveAsync();
                }
                else
                {
                    await next.Invoke();
                }
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
        }     
    }
}
;