using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ManipulacijaPodacima;
using Microsoft.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace LoggerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        Server server = new Server();

        public override void Run()
        {


            
            
            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }

            
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();
            server.Open();

            Trace.TraceInformation("LoggerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("LoggerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();
            server.Close();

            Trace.TraceInformation("LoggerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {

               

                CloudQueue queue = QueueKreiranje.GetQueueReference("zadatakqueue");

             
                    CloudQueueMessage poruka = queue.GetMessage();

                    if (poruka == null)
                    {
                        Trace.WriteLine("Ne postoji ni jedna log poruka.");
                    }
                    else
                    {
                        string preuzetaPoruka = poruka.AsString;
                        Trace.WriteLine($"Preuzet log: {preuzetaPoruka}");



                        string name_in_blob = "poruke";
                        var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                        CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobStorage.GetContainerReference("zadatakblob");

                        CloudBlockBlob blob = container.GetBlockBlobReference(name_in_blob);  

                        string red = "";
                        if (blob.Exists())  
                        {
                            red = blob.DownloadText();   
                            red = red + "|";

                        }

                        red = red + preuzetaPoruka;
                        blob.UploadText(red);
                        queue.DeleteMessage(poruka);




                    }


                 

                
                await Task.Delay(5000);
            }
        }
    }
}
