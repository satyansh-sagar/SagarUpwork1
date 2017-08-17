using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Upwork1.Constants;

namespace Upwork1.Dialogs
{
    [Serializable]
    public class HelpDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait<LuisResult>(HelpAsync);
        }
        public async Task HelpAsync(IDialogContext context, IAwaitable<LuisResult> luisResult)
        {
            var result = await luisResult;
            EntityRecommendation _help;

            if (result.TryFindEntity(Enums.Help.help.ToString(), out _help))
            {
                await context.PostAsync("Ask me anything related to market shares of brands and countries.  \nTry Specific queries e.g.  \n•What is the Market share in India.  \n•What is the market share for air care for USA  \n•MSL Compliance for india");
            }
            else
            {
                await context.Forward(new NoneDialog(), HelpAfterAsync, result, System.Threading.CancellationToken.None);
            }
            context.Done(true);
        }
        public async Task HelpAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }
    }
}