using System.Collections.Generic;
using System.Threading.Tasks;
using JasperEngineApp.Dialogs;
using JasperEngineApp.State;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace JasperEngineApp.Bot
{
    public class JasperEngineBot : IBot
    {
        private readonly DialogSet _dialogs;

        public JasperEngineBot()
        {
            _dialogs = DialogHelper.CreateDialogs();
        }

        public async Task OnTurn(ITurnContext turnContext)
        {
            switch (turnContext.Activity.Type)
            {
                case ActivityTypes.ConversationUpdate:
                    await OnConversationUpdateAsync(turnContext);
                    break;

                case ActivityTypes.Message:
                    await OnMessageAsync(turnContext);
                    break;
            }
        }

        private async Task OnConversationUpdateAsync(ITurnContext turnContext)
        {
            foreach (var newMember in turnContext.Activity.MembersAdded)
            {
                if (newMember.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivity("Bonjour, JASPER à votre service !");
                }
            }
        }

        private async Task OnMessageAsync(ITurnContext turnContext)
        {
            var userState = turnContext.GetUserState<UserTravelState>();
            if (userState.Activities == null)
            {
                userState.Activities = new List<string>();
            }

            var state = ConversationState<Dictionary<string, object>>.Get(turnContext);
            var dc = _dialogs.CreateContext(turnContext, state);

            await dc.Continue();

            if (!turnContext.Responded || dc.ActiveDialog == null)
            {
                await dc.Begin("Travel");
            }
        }
    }
}
