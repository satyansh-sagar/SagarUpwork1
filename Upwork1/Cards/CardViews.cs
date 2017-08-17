using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Upwork1.Cards
{
    public class CardViews
    {
        private List<Attachment> _attachmentList;

        public CardViews()
        {
            _attachmentList = new List<Attachment>();           
        }

        public void AddcardAttachment(string message)
        {
            HeroCard card = new HeroCard()
            {
                Text = message.ToString()

            };


            Attachment attachment = new Attachment()
            {
                Content = card,
                ContentType = HeroCard.ContentType
            };
            
            _attachmentList.Add(attachment);
        }

        public List<Attachment> ReturncardAttachment()
        {
            return _attachmentList;
        }
    }
}