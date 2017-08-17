using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Upwork1.Models;

namespace Upwork1.Utlities
{
    public class LocationFinder
    {
        /// <summary>
        /// Uses google location api (GeoCode API), change the key with your Key
        /// </summary>
        /// <param name="Location">takes location as a parameter</param>
        /// <returns>returns country</returns>
        public static async Task<string> GetGoogleApiLocation(string Location)
        {
            try
            {
                GoogleLocationService googleLocation = new GoogleLocationService();
                using (HttpClient client = new HttpClient())
                {
                    string serviceURL = "https://maps.googleapis.com/maps/api/geocode/json?address=" + Location + "&key=AIzaSyCTEdPovW6dmgXRbO8jozAit9bezop7mas";

                    HttpResponseMessage response = await client.GetAsync(serviceURL);
                    if (response.IsSuccessStatusCode)
                    {
                        var JasonDataResponse = await response.Content.ReadAsStringAsync();
                        googleLocation = JsonConvert.DeserializeObject<GoogleLocationService>(JasonDataResponse);
                    }
                }

                foreach (var item in googleLocation.results)
                {
                    foreach (var bit in item.address_components)
                    {
                        foreach (var dat in bit.types)
                        {
                            //case gerogia return usa , should return georgia , these four are exceptions
                            if (bit.long_name.ToLower() == "georgia")
                            {
                                return bit.long_name;
                            }
                            else if (bit.long_name.ToLower() == "saba")
                            {
                                return bit.long_name;
                            }
                            else if (bit.long_name.ToLower() == "sint eustatius")
                            {
                                return bit.long_name;
                            }
                            else if (bit.long_name.ToLower() == "bonaire")
                            {
                                return bit.long_name;
                            }
                            else if (bit.long_name.ToLower() == "republic of the congo")
                            {
                                return bit.long_name;
                            }
                            else if (dat == "country")
                            {
                                return bit.long_name;
                            }
                            else if (dat == "colloquial_area")
                            {
                                return bit.long_name;
                            }
                        }
                    }
                    foreach (var bit in item.address_components)
                    {
                        foreach (var dat in bit.types)
                        {
                            if (dat == "establishment")
                            {
                                return bit.long_name;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}