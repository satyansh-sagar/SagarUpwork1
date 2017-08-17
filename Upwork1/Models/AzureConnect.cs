using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Upwork1.Models
{
    public class AzureConnect
    {
        public static CloudBlobContainer BlobContainer { get; set; }

        public static CloudStorageAccount MakeConnection()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=massss;AccountKey=x7iciCkm+D49OcQYqD+qOvczlcg5jYZFbz2FjdetKTfIr1qiakz/C+xkTTdoySfBq7v7mVOro+KeSaN//YEdcA==");
            try
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                BlobContainer = blobClient.GetContainerReference("xmlcontainer");//Name of the azure container
                return storageAccount;
            }
            catch (Exception e)
            {
                Console.WriteLine("exception occured in cloud!");
            }
            return storageAccount;
        }
    }
}