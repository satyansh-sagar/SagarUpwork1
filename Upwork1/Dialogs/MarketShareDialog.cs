﻿using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Upwork1.Constants;
using Upwork1.Models;
using Upwork1.Utlities;

namespace Upwork1.Dialogs
{
    [Serializable]
    public class MarketShareDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait<LuisResult>(GetMarketShareAsync);
        }
        public async Task GetMarketShareAsync(IDialogContext context, IAwaitable<LuisResult> luisResult)
        {
            var result = await luisResult;
            EntityRecommendation _share;
            EntityRecommendation _location;
            EntityRecommendation _brand;
            EntityRecommendation _category;
            //
            //five scenarios 
            //
            XMLSerialize xmlserialize = new XMLSerialize();
            MarketDataSerialize marketData = xmlserialize.GetDataDetails();
            StringBuilder sb = new StringBuilder();

            //market share by location
            if (result.TryFindEntity(Enums.MarketShare.location.ToString(), out _location) && result.TryFindEntity(Enums.MarketShare.share.ToString(), out _share) && !result.TryFindEntity(Enums.MarketShare.brand.ToString(), out _brand) && !result.TryFindEntity(Enums.MarketShare.category.ToString(), out _category))
            {
                sb = new StringBuilder();
                var ExactLocation = await LocationFinder.GetGoogleApiLocation(_location.Entity.ToString());

                foreach (var item in marketData.MarketDetails)
                {
                    if (item.CountryName.ToLower().Trim() == ExactLocation.ToString().ToLower().Trim())
                    {                       
                        sb.AppendLine($"**{item.Category}**  : **{item.Year}** : **{Math.Round(Convert.ToDouble(item.Market_Share)*100,3)}** %  \n");
                    }                  
                }
                if (sb.ToString() == "")
                {
                    await context.PostAsync($"I don't have Market Share details for **{ExactLocation.ToString()}**");
                }
                else
                {
                    await context.PostAsync($"Market Share for **{ExactLocation.ToString()}**  \n");
                    await context.PostAsync(sb.ToString());
                }
                context.Done(true);
            }
            //market share by brand
            else if (result.TryFindEntity(Enums.MarketShare.share.ToString(), out _share) && result.TryFindEntity(Enums.MarketShare.brand.ToString(), out _brand) && !result.TryFindEntity(Enums.MarketShare.location.ToString(), out _location) && !result.TryFindEntity(Enums.MarketShare.category.ToString(), out _category))
            {
                context.PrivateConversationData.SetValue<string>("brandName", _brand.Entity.ToString());
                await context.PostAsync($"You asked Market Share for **{_brand.Entity.ToString()}**");
                PromptDialog.Text(context,GetLocationTextAsync,"Which location market share are you looking for ?","Enter the location again",3);
                #region COde
                //sb = new StringBuilder();
                //foreach(var item in marketData.MarketDetails)
                //{
                //    if(item.Category.ToString().ToLower().Trim() == _brand.Entity.ToString().ToLower().Trim())
                //    {
                //        sb.AppendLine($"**{item.Year}** : **{Math.Round(Convert.ToDouble(item.Market_Share) * 100, 3)}**  \n");
                //    }
                //}
                //if (sb.ToString() == "")
                //{
                //    await context.PostAsync($"I don't have Market Share details for **{_brand.Entity.ToString()}**  \n");
                //}
                //else
                //{
                //    await context.PostAsync($"Market Share for **{_brand.Entity.ToString()}**  \n");
                //    await context.PostAsync(sb.ToString());
                //}
#endregion
            }
            //market share by category (by all brands)
            else if (result.TryFindEntity(Enums.MarketShare.share.ToString(), out _share) && result.TryFindEntity(Enums.MarketShare.category.ToString(), out _category) && !result.TryFindEntity(Enums.MarketShare.brand.ToString(), out _brand) && !result.TryFindEntity(Enums.MarketShare.location.ToString(), out _location))
            {
                await context.PostAsync("I am still learning , Right now i can show the market share details by brand and location only.  \nType **'HELP'** for more information.  \nTry these queries:  \n• What is the market share for air care in india in 2015  \n• What is the market share for airwick in america ?");
                context.Done(true);
            }
            //market share by location and brand
            //will show google charts here.
            else if (result.TryFindEntity(Enums.MarketShare.share.ToString(), out _share) && result.TryFindEntity(Enums.MarketShare.brand.ToString(), out _brand) && result.TryFindEntity(Enums.MarketShare.location.ToString(), out _location) && !result.TryFindEntity(Enums.MarketShare.category.ToString(), out _category))
            {
                sb = new StringBuilder();
                var ExactLocation = await LocationFinder.GetGoogleApiLocation(_location.Entity.ToString());

                foreach (var item in marketData.MarketDetails)
                {
                    if (item.CountryName.ToLower().Trim() == ExactLocation.ToString().ToLower().Trim() && item.Category.ToString().ToLower().Trim() == _brand.Entity.ToString().ToLower().Trim())
                    {
                        sb.AppendLine($"**{item.Year}** : **{Math.Round(Convert.ToDouble(item.Market_Share) * 100, 3)}** %  \n");
                    }
                }
                if (sb.ToString() == "")
                {
                    await context.PostAsync($"I don't have Market Share details for **{_brand.Entity.ToString()}** for **{ExactLocation.ToString()}**");
                }
                else
                {
                    await context.PostAsync($"Market Share for **{_brand.Entity.ToString().ToUpper()}** for **{ExactLocation.ToString()}**");
                    await context.PostAsync(sb.ToString());
                }
                context.Done(true);
            }
            //In case user asks for only market share
            else if (!result.TryFindEntity(Enums.MarketShare.location.ToString(), out _location) && result.TryFindEntity(Enums.MarketShare.share.ToString(), out _share) && !result.TryFindEntity(Enums.MarketShare.brand.ToString(), out _brand) && !result.TryFindEntity(Enums.MarketShare.category.ToString(), out _category))
            {
                await context.PostAsync("It seems you want to know about Market Shares , try reprhasing your query .  \nI can tell you the market share by Location , Brands and Category.  \nTry these queries:  \n• What is the market share for air care in india in 2015  \n• What is the market share for airwick in america ?");
                context.Done(true);
            }
            else
            {
                await context.Forward(new NoneDialog(), MarketShareAfterAsync, result, System.Threading.CancellationToken.None);
                
            }
            //context.Done(true);
        }

        public async Task MarketShareAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(true);
        }

        public async Task GetLocationTextAsync(IDialogContext context, IAwaitable<String> result)
        {
            var message = await result;
           
            if (message.Equals("Help", StringComparison.InvariantCultureIgnoreCase) || (message.Equals("cancel", StringComparison.InvariantCultureIgnoreCase)))
            {
                await context.PostAsync("Ask me anything related to market shares of brands and countries.  \nTry Specific queries e.g.  \n•What is the Market share in India.  \n•What is the market share for air care for USA  \n•MSL Compliance for india");
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                var ExactLocation = await LocationFinder.GetGoogleApiLocation(message);
                sb = new StringBuilder();
                
                XMLSerialize xmlserialize = new XMLSerialize();
                MarketDataSerialize marketData = xmlserialize.GetDataDetails();

                var brandName = context.PrivateConversationData.GetValue<string>("brandName");

                await context.PostAsync($"Hold on , While i get details for **{brandName}** for **{ExactLocation}**");

                foreach (var item in marketData.MarketDetails)
                {
                    if (item.CountryName.ToLower().Trim() == ExactLocation.ToString().ToLower().Trim() && item.Category.ToString().ToLower().Trim() == brandName.ToLower().Trim())
                    {
                        sb.AppendLine($"**{item.Year}** : **{Math.Round(Convert.ToDouble(item.Market_Share) * 100, 3)}** %  \n");
                    }
                }
                if (sb.ToString() == "")
                {
                    await context.PostAsync($"I don't have Market Share details for **{brandName}** for **{ExactLocation.ToString()}**");
                }
                else
                {
                    await context.PostAsync($"Market Share for **{brandName.ToUpper()}** for **{ExactLocation.ToString()}**");
                    await context.PostAsync(sb.ToString());
                }
               
            }
            context.Done(true);
        }

    }
}