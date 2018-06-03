using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Cognitive.LUIS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LuisBot
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBot<LuisBot>(options =>
            {
                options.CredentialProvider = new ConfigurationCredentialProvider(Configuration);

                var model = new LuisModel("[<LUIS Application Id>]", "[<LUIS Application Secret Key>]", new Uri("https://westeurope.api.cognitive.microsoft.com/luis/v2.0/apps/"));
                options.Middleware.Add(new LuisRecognizerMiddleware(model, luisOptions: new LuisRequest { Verbose = true, TimezoneOffset = 60 }));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
               .UseStaticFiles()
               .UseBotFramework();
        }
    }
}
