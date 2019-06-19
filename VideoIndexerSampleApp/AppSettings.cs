using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace VideoIndexerSampleApp
{
    public class AppSettings
    {
        public string StorageAccountName { get; set; } = "videoindexerdemo2";
        public string VideoBlobContainerName { get; set; } = "videos";
        public string StorageAccessKey { get; set; } = "yaCIzL8p/E5IyMG1QZjMJ4dgk/Co8MlrwXapXpuX3u5+EtU2/Y5fO/PI95lxBd9xs1VK4CQLVx1fN4PzdCcU3Q==";
        public string VideoIndexerAccountId { get; set; } = "0bb14902-37c1-4b6c-978a-7baefc6163cc";
        public string VideoIndexerRegion { get; set; } = "japaneast";
        public string VideoIndexerApiKey { get; set; } = "62b9ccc35d3d4a83990c78b2928ce9d2";

        public void Store()
        {
            ApplicationData.Current.LocalSettings.Values[nameof(AppSettings)] = JsonConvert.SerializeObject(this);
        }

        public void Restore()
        {
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(nameof(AppSettings), out var value) && value is string json)
            {
                JsonConvert.PopulateObject(json, this);
            }
        }
    }
}
