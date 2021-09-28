using Common;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerRole
{
    public class ServerProvider : ILogger
    {
        public List<string> AutomatskoSlanjeLogova(List<string> postojeci)
        {

            List<string> novonastali = new List<string>();

            List<string> sviLogovi = new List<string>();

            string name_in_blob = string.Format("poruke");
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobStorage.GetContainerReference("zadatakblob");

            CloudBlockBlob blob = container.GetBlockBlobReference(name_in_blob);

            string red = "";

            if (blob.Exists())
            {
                red = blob.DownloadText();
                string[] poruke = red.Split('|');

                foreach (string s in poruke)
                {
                    sviLogovi.Add(s);
                }
            }



            bool vecPrikazan = false;

            for(int i = 0; i < sviLogovi.Count; i++)
            {
                vecPrikazan = false;
                for (int j = 0; j < postojeci.Count; j++)
                {
                    if (postojeci[j].Equals(sviLogovi[i]))
                        vecPrikazan = true;
                }
               
                if(!vecPrikazan)
                    novonastali.Add(sviLogovi[i]);
            }

            return novonastali;

        }

        public List<string> VratiSveLogove()
        {
            List<string> sviLogovi = new List<string>();
            
            string name_in_blob = string.Format("poruke");
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobStorage.GetContainerReference("zadatakblob");    

            CloudBlockBlob blob = container.GetBlockBlobReference(name_in_blob);

            string red = "";

            if(blob.Exists())
            {
                red = blob.DownloadText();
                string[] poruke = red.Split('|');
                
                foreach(string s in poruke)
                {
                    sviLogovi.Add(s);
                }
            }

            return sviLogovi;
        }



    }
}
