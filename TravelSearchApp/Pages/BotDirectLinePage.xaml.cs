namespace TravelSearchApp.Pages
{
    #region Usings
    using Microsoft.Bot.Connector.DirectLine;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using TravelSearchApp.Models;
    using Windows.System;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Navigation;
    #endregion

    public sealed partial class BotDirectLinePage : Page
    {
        private static string directLineSecretKey = "[<Direct Line Secret>]";
        private static string botId = "[<Azure Bot Service Name>]";
        private static string userId = "DirectLineSampleClientUser";

        private DirectLineClient _client;
        private Conversation _conversation;
        private ObservableCollection<BotMessage> _activities;

        public BotDirectLinePage()
        {
            InitializeComponent();
        }

        public ObservableCollection<BotMessage> Activities
        {
            get
            {
                return _activities ?? (_activities = new ObservableCollection<BotMessage>());
            }
        }

        private async Task StartBotConversation()
        {
            _client = new DirectLineClient(directLineSecretKey);
            _conversation = await _client.Conversations.StartConversationAsync();

            await SendActivity(string.Empty);

            Task.Run(async () => await ReadBotMessagesAsync(_conversation.ConversationId));
        }

        private void StopBotConversation()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }

            _conversation = null;

            Activities.Clear();
        }

        private Task SendActivity(string message)
        {
            Activity activity = new Activity
            {
                From = new ChannelAccount(userId),
                Text = message,
                Type = ActivityTypes.Message
            };

            if (!string.IsNullOrEmpty(message))
            {
                Activities.Add(new BotMessage
                {
                    Text = activity.Text,
                    Date = DateTime.Now,
                    IsSended = true
                });
            }

            return _client.Conversations.PostActivityAsync(_conversation.ConversationId, activity);
        }

        private async Task ReadBotMessagesAsync(string conversationId)
        {
            string watermark = null;

            while (true)
            {

                var activitySet = await _client.Conversations.GetActivitiesAsync(conversationId, watermark);
                watermark = activitySet?.Watermark;

                var activities = activitySet?.Activities.Where(x => x.From.Id == botId).ToList() ?? new List<Activity>();

                foreach (Activity activity in activities)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                    {
                        Activities.Add(new BotMessage
                        {
                            Text = activity.Text,
                            Date = activity.Timestamp,
                            Images = activity.Attachments?.Where(a => a.ContentType == "image/jpg").ToList(),
                            IsSended = false
                        });
                    }));
                }

                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            }
        }

        private Task SendMessage()
        {
            string textToSend = MessageText.Text;

            MessageText.Text = string.Empty;

            return SendActivity(textToSend);
        }

        #region Handlers
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            StartBotConversation();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            StopBotConversation();
        }

        private async void OnButtonClick(object sender, RoutedEventArgs e)
        {
            await SendMessage();
        }

        private async void OnMessageTextKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                await SendMessage();
            }
        }
        #endregion
    }
}