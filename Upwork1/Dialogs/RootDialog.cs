using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Upwork1.Dialogs
{
    [Serializable]
    [LuisModel("0975a937-c9f5-4153-9b63-1da4633ae359", "7357e773a0904180875656dabe732998")]
    public class RootDialog : LuisDialog<object>
    {
        
        [LuisIntent("Greetings")]
        private async Task GreetingsAsync(IDialogContext context , LuisResult result)
        {
            await context.Forward(new GreetingDialog(), OnCompletionAsync, result, System.Threading.CancellationToken.None);
        }

        [LuisIntent("SendEmail")]
        private async Task EmailAsync(IDialogContext context , LuisResult result)
        {
            await context.Forward(new EmailDialog(), OnCompletionAsync, result, System.Threading.CancellationToken.None);
        }

        [LuisIntent("ServiceMarketShare")]
        private async Task MarketShareAsync(IDialogContext context , LuisResult result)
        {
            await context.Forward(new MarketShareDialog(), OnCompletionAsync, result, System.Threading.CancellationToken.None);
        }

        [LuisIntent("ServiceMSL")]
        private async Task MSLServiceAsync(IDialogContext context , LuisResult result)
        {
            await context.Forward(new MSLServiceDialog(), OnCompletionAsync, result, System.Threading.CancellationToken.None);
        }

        [LuisIntent("WordDictonary")]
        private async Task SlangAsync(IDialogContext context , LuisResult result)
        {
            await context.Forward(new SlangDialog(), OnCompletionAsync, result, System.Threading.CancellationToken.None);
        }

        [LuisIntent("Help")]
        private async Task HelpAsync(IDialogContext context, LuisResult result)
        {
            await context.Forward(new HelpDialog(), OnCompletionAsync, result, System.Threading.CancellationToken.None);
        }

        [LuisIntent("None")]
        private async Task None(IDialogContext context, LuisResult result)
        {
            await context.Forward(new NoneDialog(), OnCompletionAsync, result, System.Threading.CancellationToken.None);
        }

        private async Task OnCompletionAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }
    }
}