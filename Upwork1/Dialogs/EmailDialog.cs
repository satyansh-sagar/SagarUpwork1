using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using Upwork1.Constants;
using Microsoft.Bot.Builder.Luis;

namespace Upwork1.Dialogs
{
    [Serializable]
    public class EmailDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait<LuisResult>(SendEmailAsync);
        }
        public async Task SendEmailAsync(IDialogContext context, IAwaitable<LuisResult> luisResult)
        {
            var result = await luisResult;
            EntityRecommendation _sendEmail;            
            
            if (result.TryFindEntity(Enums.Email.sendEmail.ToString(), out _sendEmail))
            {
                await context.PostAsync("I cannot send E-mails right now ,Right now i am learning the basics :)");
            }           
            else
            {
                await context.Forward(new NoneDialog(), EmailAfterAsync, result, System.Threading.CancellationToken.None);
            }
            context.Done(true);
        }
        public async Task EmailAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }
    }
}