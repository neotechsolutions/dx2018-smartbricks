using JasperEngineApp.Services;
using JasperEngineApp.State;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.TraceExtensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelSearchApp.Services;

namespace JasperEngineApp.Dialogs
{
    public static class DialogHelper
    {
        public static DialogSet CreateDialogs()
        {
            var dialogs = new DialogSet();
            dialogs.Add("Travel", new WaterfallStep[]
            {
                TravelStepGreeting,
                TravelStepHealth,
                TravelStepLaunchSearch
            });
            dialogs.Add("Search", new WaterfallStep[]
            {
                SearchStepContinent,
                SearchStepActivities,
                SearchStepLaunchResults
            });
            dialogs.Add("Results", new WaterfallStep[]
            {
                ResultsStepSearch,
                ResultsStepAction
            });
            dialogs.Add("LoadBag", new WaterfallStep[]
            {
                LoadBagStepStart,
                LoadBagStepEnd,
            });
            dialogs.Add("HealthPrompt", new TextPrompt());
            dialogs.Add("StartPrompt", new TextPrompt());
            dialogs.Add("ContinentPrompt", new TextPrompt());
            dialogs.Add("ActivitiesPrompt", new TextPrompt());
            dialogs.Add("ActionPrompt", new TextPrompt());
            dialogs.Add("LoadBagPrompt", new TextPrompt());

            return dialogs;
        }

        #region Steps for "Travel" dialog

        private static async Task TravelStepGreeting(DialogContext dc, IDictionary<string, object> args, SkipStepFunction next)
        {
            await ManageIntentsInMessageAsync(dc, async (ctx, intent, _) =>
            {
                if (intent == "Start" || intent == "Greetings")
                {
                    await dc.Prompt("HealthPrompt", "Comment allez-vous aujourd'hui ?");
                }
                else
                {
                    await ctx.SendActivity("Vous n'êtes pas très poli !");
                    dc.EndAll();
                }
            });
        }

        private static async Task TravelStepHealth(DialogContext dc, IDictionary<string, object> args, SkipStepFunction next)
        {
            if (args != null)
            {
                await ManageIntentsInMessageAsync(dc, async (ctx, intent, _) =>
                {
                    var state = ctx.GetUserState<UserTravelState>();

                    switch (intent)
                    {
                        case "GoodHealth":
                            state.GoodHealth = true;
                            await dc.Prompt("StartPrompt", "Excellente nouvelle ! Pour garder votre bonne humeur et rester en forme, que diriez-vous de partir quelques jours en voyage ?");
                            break;

                        case "BadHealth":
                            state.GoodHealth = false;
                            await dc.Prompt("StartPrompt", "Hmmm... Je crois avoir une idée pour vous remettre en forme ! Que diriez-vous de partir quelques jours en voyage ?");
                            break;
                    }
                });
            }
        }

        private static async Task TravelStepLaunchSearch(DialogContext dc, IDictionary<string, object> args, SkipStepFunction next)
        {
            if (args != null)
            {
                await ManageIntentsInMessageAsync(dc, async (ctx, intent, _) =>
                {
                    var state = ctx.GetUserState<UserTravelState>();

                    switch (intent)
                    {
                        case "Affirmative":
                            await dc.Begin("Search");
                            break;

                        case "Negative":
                            if (state.GoodHealth)
                            {
                                await dc.Context.SendActivity("Merci d'avoir utiliser JASPER, passez une bonne journé !");
                            }
                            else
                            {
                                await dc.Context.SendActivity("Merci d'avoir utiliser JASPER, rétablissez-vous bien.");
                            }

                            dc.EndAll();
                            break;
                    }
                });
            }
        }

        #endregion

        #region Steps for "Search" dialog

        private static Task SearchStepContinent(DialogContext dc, IDictionary<string, object> args, SkipStepFunction next)
        {
            return dc.Prompt("ContinentPrompt", "Pour vous proposer une destination, je dois vous poser quelques questions. Tout d'abord, sur quel continent aimeriez-vous aller ?");
        }

        private static async Task SearchStepActivities(DialogContext dc, IDictionary<string, object> args, SkipStepFunction next)
        {
            if (args != null)
            {
                await ManageIntentsInMessageAsync(dc, async (ctx, intent, result) =>
                {
                    var state = ctx.GetUserState<UserTravelState>();

                    switch (intent)
                    {
                        case "Continent":
                            JToken data = GetEntity<JToken>(result, "Continent");
                            state.Continent = data.First.Value<string>();
                            break;

                        case "Unknown":
                            state.Continent = null;
                            break;
                    }
                });

                await Task.Delay(1000);
                await dc.Prompt("ActivitiesPrompt", "C'est noté. Souhaiteriez-vous faire des activités particulières ?");
            }
        }

        private static async Task SearchStepLaunchResults(DialogContext dc, IDictionary<string, object> args, SkipStepFunction next)
        {
            if (args != null)
            {
                var state = dc.Context.GetUserState<UserTravelState>();

                await ManageIntentsInMessageAsync(dc, async (ctx, intent, result) =>
                {
                    switch (intent)
                    {
                        case "Activities":
                            JToken[] data = GetEntities<JToken>(result, "Activities");
                            state.Activities = data.Select(x => x.First.Value<string>()).ToList();
                            break;

                        case "NoActivities":
                            state.Activities = new List<string>();
                            break;
                    }

                    await dc.Begin("Results");
                });
            }
        }

        #endregion

        #region Steps for "Results" dialog

        private static async Task ResultsStepSearch(DialogContext dc, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dc.Context.SendActivity("Je recherche une destination qui pourrait vous intéresser, merci de patienter.");

            var state = dc.Context.GetUserState<UserTravelState>();
            var travels = await TravelService.SearchTravelsAsync(state.Continent, state.Activities);

            var travel = travels.FirstOrDefault();
            if (travel == null)
            {
                state.CurrentTravelId = null;
                await dc.Prompt("ActionPrompt", "Désolé, je n'ai trouvé aucune destination. Voulez-vous lancer une nouvelle recherche ?");
            }
            else
            {
                state.CurrentTravelId = travel.Id;

                var activity = MessageFactory.ContentUrl(travel.MainImage.Url, "image/jpg", text: $"J'ai trouvé une destination : {travel.Days} jours pour {travel.Desc}");
                await dc.Context.SendActivity(activity);

                await Task.Delay(500);

                await dc.Prompt("ActionPrompt", "Voulez-vous réserver cette destination ?");
            }
        }

        private static async Task ResultsStepAction(DialogContext dc, IDictionary<string, object> args, SkipStepFunction next)
        {
            if (args != null)
            {
                await ManageIntentsInMessageAsync(dc, async (ctx, intent, _) =>
                {
                    var state = dc.Context.GetUserState<UserTravelState>();
                    var isBooking = state.CurrentTravelId.HasValue;

                    switch (intent)
                    {
                        case "Affirmative":
                            if (isBooking)
                            {
                                await ctx.SendActivity("Je m'en occupe tout de suite, vous recevrez les informations par mail");
                                await Task.Delay(1000);
                                await dc.Begin("LoadBag");
                            }
                            else
                            {
                                await dc.Begin("Search");
                            }

                            break;

                        case "Negative":
                            if (state.GoodHealth)
                            {
                                await dc.Context.SendActivity("Merci d'avoir utiliser JASPER, passez une bonne journé !");
                            }
                            else
                            {
                                await dc.Context.SendActivity("Merci d'avoir utiliser JASPER, rétablissez-vous bien.");
                            }

                            dc.EndAll();
                            break;
                    }
                });
            }
        }

        #endregion

        #region Steps for "LoadBag" dialog

        private static Task LoadBagStepStart(DialogContext dc, IDictionary<string, object> args, SkipStepFunction next)
        {
            return dc.Prompt("LoadBagPrompt", "J'attend votre ordre pour que BRAIN charge votre valise dans le coffre de votre voiture");
        }

        private static async Task LoadBagStepEnd(DialogContext dc, IDictionary<string, object> args, SkipStepFunction next)
        {
            if (args != null)
            {
                var result = await BrainConnector.LoadBagAsync();
                if (result.status == 200)
                {
                    await dc.Context.TraceActivity($"Payload from BRAIN : {result.payload}");
                    await dc.Context.SendActivity("BRAIN charge votre valise, merci de patienter quelques instants.");

                    await BrainConnector.WaitUntilBagLoadedAsync();

                    await dc.Context.SendActivity("BRAIN indique que votre valide est chargée. Bon voyage !");
                    dc.EndAll();
                }
            }
        }

        #endregion

        #region Utils

        private static async Task ManageIntentsInMessageAsync(DialogContext dc, Func<ITurnContext, string, RecognizerResult, Task> onRecognizedIntent = null, Func<ITurnContext, Task> onUnrecognizedIntent = null)
        {
            var result = dc.Context.Services.Get<RecognizerResult>(LuisRecognizerMiddleware.LuisRecognizerResultKey);
            var topIntent = result?.GetTopScoringIntent();

            if ((topIntent?.score ?? 0) < 0.3)
            {
                await dc.Context.SendActivity("Je ne suis pas sûr d'avoir compris ce que vous avez dit...");

                if (onUnrecognizedIntent != null)
                {
                    await onUnrecognizedIntent(dc.Context);
                }
                else
                {
                    await dc.End();
                }
            }
            else
            {
                switch (topIntent?.intent)
                {
                    case "Cancel":
                    case "Exit":
                        await dc.Context.SendActivity("Merci d'avoir utilisé JASPER !");
                        dc.EndAll();
                        break;

                    default:
                        if (onRecognizedIntent == null)
                        {
                            await dc.Context.SendActivity($"Intent: {topIntent?.intent} ({topIntent?.score}).");
                        }
                        else
                        {
                            await onRecognizedIntent(dc.Context, topIntent?.intent, result);
                        }

                        break;
                }
            }
        }

        private static T GetEntity<T>(RecognizerResult luisResult, string entityKey)
        {
            var data = luisResult.Entities as IDictionary<string, JToken>;
            if (data.TryGetValue(entityKey, out JToken value))
            {
                return value.First.Value<T>();
            }

            return default(T);
        }

        private static T[] GetEntities<T>(RecognizerResult luisResult, string entityKey)
        {
            var data = luisResult.Entities as IDictionary<string, JToken>;
            if (data.TryGetValue(entityKey, out JToken value))
            {
                return value.Values<T>().ToArray();
            }

            return new T[] { };
        }

        #endregion
    }
}
