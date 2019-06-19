using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerSampleApp.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        public StorageRepository(AppSettings config)
        {
            Config = config;
        }

        private void Initialize()
        {
            if (Container != null) { return; }
            var storageAccount = new CloudStorageAccount(new StorageCredentials(Config.StorageAccountName, Config.StorageAccessKey), true);
            var client = storageAccount.CreateCloudBlobClient();
            Container = client.GetContainerReference(Config.VideoBlobContainerName);
        }

        private AppSettings Config { get; }
        private CloudBlobContainer Container { get; set; }

        public async Task<Uri> UploadVideoAsync(string name, Stream video)
        {
            Initialize();
            await Container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, new BlobRequestOptions(), new OperationContext());
            var file = Container.GetBlockBlobReference(name);
            await file.UploadFromStreamAsync(video);
            return file.Uri;
        }
    }
}
