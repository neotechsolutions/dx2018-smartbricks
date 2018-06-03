using TravelSearchApp.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TravelSearchApp.Selectors
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SendedTemplate { get; set; }
        public DataTemplate ReceivedTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if(item is BotMessage message)
            {
                return message.IsSended ? SendedTemplate : ReceivedTemplate;
            }

            return base.SelectTemplateCore(item);
        }
    }
}

