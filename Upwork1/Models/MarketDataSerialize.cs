using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Upwork1.Models
{
    [XmlRoot("NewDataSet")]
    public class MarketDataSerialize
    {
        public static MarketDataSerialize marketdataSerializeObj = null;

        [XmlElement("Table1")]
        public List<MarketDetails> MarketDetails { get; set; }

        public static MarketDataSerialize GetObject()
        {
            return marketdataSerializeObj ?? (marketdataSerializeObj = new MarketDataSerialize());
        }
    }

    public class MarketDetails
    {
        [XmlElement("Country")]
        public string CountryName { get; set; }

        [XmlElement("Year")]
        public string Year { get; set; }

        [XmlElement("Category")]
        public string Category { get; set; }

        [XmlElement("Total")]
        public string Total { get; set; }

        [XmlElement("RB_Market")]
        public string RB_Market { get; set; }

        [XmlElement("Market_Share")]
        public string Market_Share { get; set; }
    }
}