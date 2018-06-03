using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;

namespace LuisBot
{
    public class LuisBot : IBot
    {
        public async Task OnTurn(ITurnContext turnContext)
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var result = turnContext.Services.Get<RecognizerResult>(LuisRecognizerMiddleware.LuisRecognizerResultKey);
                var topIntent = result?.GetTopScoringIntent();

                if (topIntent?.intent == "Greetings")
                {
                    await turnContext.SendActivity($"Bonjour {turnContext.Activity.From.Name} !");
                }
                else
                {
                    await turnContext.SendActivity("Bienvenue sur ce bot utilisant LUIS !");
                }
            }
        }
    }
}