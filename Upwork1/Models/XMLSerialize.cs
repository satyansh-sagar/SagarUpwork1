using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

namespace Upwork1.Models
{
    public class XMLSerialize
    {
        public static XmlReader GetXmlReader(string xmlString)
        {
            StringReader builder = new StringReader(xmlString);
            XmlReader reader = new XmlTextReader(builder);
            return reader;
        }

        public MarketDataSerialize GetDataDetails()
        {
            AzureConnect.MakeConnection();
            CloudAppendBlob appendBlob = AzureConnect.BlobContainer.GetAppendBlobReference("Market_Share.xml");
            //Name of the blob
            if (!appendBlob.Exists())
            {
                appendBlob.CreateOrReplace();
                XDocument doc = new XDocument(new XElement("NewDataSet"));
                using (Stream stream = new MemoryStream())
                {
                    doc.Save(stream);
                    appendBlob.UploadFromStream(stream);
                }
            }
            else if (appendBlob.Exists())
            {
                var xml = appendBlob.DownloadText();
                MarketDataSerialize marketData = MarketDataSerialize.GetObject();
                XmlReader reader = GetXmlReader(xml);
                XmlSerializer serializer = new XmlSerializer(typeof(MarketDataSerialize));
                marketData = (MarketDataSerialize)serializer.Deserialize(reader);

                return marketData;
            }
            return null;
        }
    }
}