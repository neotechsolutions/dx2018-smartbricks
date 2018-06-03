using System;
using System.Collections.Generic;
using JasperEngineApp.Bot;
using JasperEngineApp.State;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Cognitive.LUIS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JasperEngineApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBot<JasperEngineBot>(options =>
            {
                options.CredentialProvider = new ConfigurationCredentialProvider(Configuration);
                options.EnableProactiveMessages = true;

                options.Middleware.Add(new ShowTypingMiddleware());

                var dataStore = new MemoryStorage();
                options.Middleware.Add(
                    new ConversationState<Dictionary<string, object>>(dataStore));
                options.Middleware.Add(
                    new UserState<UserTravelState>(dataStore));

                var (modelId, subscriptionKey, url) = GetLuisConfiguration(Configuration);
                var model = new LuisModel(modelId, subscriptionKey, url);
                options.Middleware.Add(
                    new LuisRecognizerMiddleware(
                        model,
                        luisOptions: new LuisRequest { Verbose = true, TimezoneOffset = 60 }));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection()
               .UseDefaultFiles()
               .UseStaticFiles()
               .UseBotFramework();
        }

        private (string modelId, string subscriptionKey, Uri url) GetLuisConfiguration(IConfiguration configuration)
        {
            var modelId = configuration.GetSection("Luis-ModelId")?.Value;
            var subscriptionKey = configuration.GetSection("Luis-SubscriptionKey")?.Value;
            var url = configuration.GetSection("Luis-Url")?.Value;
            return (modelId, subscriptionKey, new Uri(url));
        }
    }
}
