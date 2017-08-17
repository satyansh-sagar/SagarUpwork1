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
    public class GreetingDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait<LuisResult>(GreetingsAsync);
        }
        public async Task GreetingsAsync(IDialogContext context, IAwaitable<LuisResult> luisResult)
        {
            var result = await luisResult;
            EntityRecommendation _greeting;
            EntityRecommendation _SendOff;

            if (result.TryFindEntity(Enums.Greeting.greet.ToString(), out _greeting) && !result.TryFindEntity(Enums.Greeting.sendoff.ToString(), out _SendOff))
            {
                await context.PostAsync("Hi ! I am PoloBot. Ask me anything related to market shares of brands and countries.  \nType **'HELP'** anytime to get specific queries.");
            }
            else if (result.TryFindEntity(Enums.Greeting.sendoff.ToString(), out _SendOff) && !result.TryFindEntity(Enums.Greeting.greet.ToString(), out _greeting))
            {
                await context.PostAsync("Bye! Come back soon if you have more questions.");
            }
            else
            {
                await context.Forward(new NoneDialog(), GreetingsAfterAsync, result, System.Threading.CancellationToken.None);
            }
            context.Done(true);
        }
        public async Task GreetingsAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }
    }
}