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
    public class MSLServiceDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait<LuisResult>(ServiceMSLAsync);
        }
        public async Task ServiceMSLAsync(IDialogContext context, IAwaitable<LuisResult> luisResult)
        {
            var result = await luisResult;
            EntityRecommendation _msl;
            EntityRecommendation _brand;
            EntityRecommendation _location;

            //msl for brand
            if (result.TryFindEntity(Enums.MarketShare.brand.ToString(), out _brand) && result.TryFindEntity(Enums.MarketShare.msl.ToString(), out _msl) && !result.TryFindEntity(Enums.MarketShare.location.ToString(),out _location))
            {
                await context.PostAsync($"MSL for **{_brand.Entity.ToString()}** is **47 %**.");
            }
            //msl for location
            else if (result.TryFindEntity(Enums.MarketShare.msl.ToString(), out _msl) && result.TryFindEntity(Enums.MarketShare.location.ToString(), out _location) && !result.TryFindEntity(Enums.MarketShare.brand.ToString(), out _brand))
            {
                await context.PostAsync($"MSL for **{_location.Entity.ToString()}** is **47 %**");
            }
            //msl for brand and location
            else if (result.TryFindEntity(Enums.MarketShare.brand.ToString(), out _brand) && result.TryFindEntity(Enums.MarketShare.msl.ToString(), out _msl) && result.TryFindEntity(Enums.MarketShare.location.ToString(), out _location))
            {
                await context.PostAsync($"MSl Compliance for **{_brand.Entity.ToString()}** for **{_location.Entity.ToString()}** is **47 %**.");
            }
            //only for msl
            else if (result.TryFindEntity(Enums.MarketShare.msl.ToString(), out _msl) && !result.TryFindEntity(Enums.MarketShare.location.ToString(), out _location) && !result.TryFindEntity(Enums.MarketShare.brand.ToString(), out _brand))
            {
                await context.PostAsync("MSL data is for brand and country specific , try asking specific quries. e.g. What is MSL for brazil ?");
            }
            //only for location
            else if (result.TryFindEntity(Enums.MarketShare.location.ToString(), out _location) && !result.TryFindEntity(Enums.MarketShare.brand.ToString(), out _brand) && !result.TryFindEntity(Enums.MarketShare.msl.ToString(), out _msl))
            {
                await context.PostAsync("MSL data is for brand and country specific , try asking specific quries. e.g. What is MSL for brazil ?");
            }
            //only for brand
            else if (result.TryFindEntity(Enums.MarketShare.brand.ToString(), out _brand) && !result.TryFindEntity(Enums.MarketShare.msl.ToString(), out _msl) && !result.TryFindEntity(Enums.MarketShare.location.ToString(), out _location))
            {
                await context.PostAsync("MSL data is for brand and country specific , try asking specific quries. e.g. What is MSL for brazil ?");
            }
            else
            {
                await context.Forward(new NoneDialog(), MSLAfterAsync, result, System.Threading.CancellationToken.None);
            }
            context.Done(true);
        }
        public async Task MSLAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }
    }
}