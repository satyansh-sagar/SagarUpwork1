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
    public class SlangDialog : IDialog<object>
    {
        EntityRecommendation _blessedwords;
        EntityRecommendation _cursedwords;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait<LuisResult>(SlangMessageAsync);
        }

        public async Task SlangMessageAsync(IDialogContext context, IAwaitable<LuisResult> luisResult)
        {
            var result = await luisResult;
            Random random = new Random();

            if (result.TryFindEntity(Enums.Slangs.goodWords.ToString(), out _blessedwords) && !result.TryFindEntity(Enums.Slangs.badWords.ToString(), out _cursedwords))
            {
                string[] slangsArray = { $"Sure. Happy to help !", "Thank you for that remark !", "I am glad you liked it" };
                string response = slangsArray[random.Next(0, slangsArray.Length)];
                await context.PostAsync(response);
            }
            else if (result.TryFindEntity(Enums.Slangs.badWords.ToString(), out _cursedwords))
            {
                string[] slangsArray = { $"I can tell you are upset. If my answers were not helpfultype **HELP** for more details. ", "Aahh ! I dont like using these words. If my answers were not helpfultype **HELP** for more details." };
                string response = slangsArray[random.Next(0, slangsArray.Length)];
                await context.PostAsync(response);
            }
            else
            {
                await context.Forward(new NoneDialog(), SlangAfterAsync, result, System.Threading.CancellationToken.None);
            }
            context.Done(true);
        }

        public async Task SlangAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }
    }
}