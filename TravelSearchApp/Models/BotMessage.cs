using System;
using System.Collections.Generic;
using Microsoft.Bot.Connector.DirectLine;

namespace TravelSearchApp.Models
{
    public class BotMessage
    {
        public string Text { get; set; }

        public DateTime? Date { get; set; }

        public bool IsSended { get; set; }

        public List<Attachment> Images { get; internal set; }
    }
}
