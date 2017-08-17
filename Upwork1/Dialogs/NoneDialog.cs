using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Upwork1.Dialogs
{
    [Serializable]
    public class NoneDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait<LuisResult>(this.NoneDialogProcessAsync);
        }

        public async Task NoneDialogProcessAsync(IDialogContext context, IAwaitable<LuisResult> luisResult)
        {
            var result = await luisResult;

            await context.PostAsync("I'm not getting you , i am still Learning.  \nFor specific queries type **'HELP'**");
            context.Done(true);
        }
    }
}