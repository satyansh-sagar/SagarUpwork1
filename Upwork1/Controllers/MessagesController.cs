﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Autofac;
using System.Linq;
using System.Diagnostics;

namespace Upwork1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity != null)
            {
                switch (activity.GetActivityType())
                {
                    case ActivityTypes.Message:

                        await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
                        break;

                    case ActivityTypes.ConversationUpdate:

                        IConversationUpdateActivity update = activity;
                        using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                        {
                            var client = scope.Resolve<IConnectorClient>();

                            if (update.MembersAdded.Any())
                            {
                                var reply = activity.CreateReply();
                                foreach (var newMember in update.MembersAdded)
                                {
                                    if (newMember.Id == activity.Recipient.Id)
                                    {
                                        reply.Text = $"Hi ! I am Polobot. Ask me anything related about market shares of your products and countries.  \nType **'HELP'** anytime to get specific queries.";
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    await client.Conversations.ReplyToActivityAsync(reply);
                                }
                            }
                        }
                        break;
                    case ActivityTypes.DeleteUserData:
                        break;

                    case ActivityTypes.ContactRelationUpdate:
                        break;

                    case ActivityTypes.Typing:
                        break;
                    case ActivityTypes.Ping:
                        break;


                    default:

                        Trace.TraceError($"Unknown activity type ignored: {activity.GetActivityType()}");

                        break;

                }
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}