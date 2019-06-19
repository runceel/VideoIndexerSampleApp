using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using VideoIndexerSampleApp.VideoIndexer.GetVideos;

namespace VideoIndexerSampleApp.VideoIndexer
{
    class VideoIndexerClient : IVideoIndexerClient
    {
        private readonly string ApiUrl = "https://api.videoindexer.ai";

        private AppSettings Config { get; }

        public VideoIndexerClient(AppSettings config)
        {
            Config = config;
        }

        private async Task<(HttpClient client, string accessToken)> CreateHttpClient()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.ServicePointManager.SecurityProtocol | System.Net.SecurityProtocolType.Tls12;
            // create the http client
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Config.VideoIndexerApiKey);

            // obtain account access token
            var accountAccessTokenRequestResult = await client.GetAsync($"{ApiUrl}/auth/{Config.VideoIndexerRegion}/Accounts/{Config.VideoIndexerAccountId}/AccessToken?allowEdit=true");
            var accountAccessToken = (await accountAccessTokenRequestResult.Content.ReadAsStringAsync()).Replace("\"", "");

            client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");
            return (client, accountAccessToken);
        }

        private async Task<(HttpClient client, string accessToken)> CreateVideoHttpClient(string videoId)
        {
            var (client, token) = await CreateHttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Config.VideoIndexerApiKey);
            var videoTokenRequestResult = await client.GetAsync($"{ApiUrl}/auth/{Config.VideoIndexerRegion}/Accounts/{Config.VideoIndexerAccountId}/Videos/{videoId}/AccessToken?allowEdit=true");
            var videoAccessToken = (await videoTokenRequestResult.Content.ReadAsStringAsync()).Replace("\"", "");
            client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");
            return (client, videoAccessToken);
        }

        public async Task<string> UploadVideoToVideoIndexerAsync(Uri uri)
        {
            var (client, token) = await CreateHttpClient();
            var r = await client.PostAsync($"{ApiUrl}/{Config.VideoIndexerRegion}/Accounts/{Config.VideoIndexerAccountId}/Videos?accessToken={token}&name=Video-{DateTime.Now.ToString("yyyyMMddHHmmss")}&privacy=private&language=ja-JP&videoUrl={uri}", new MultipartFormDataContent());
            r.EnsureSuccessStatusCode();
            var content = JsonConvert.DeserializeObject<dynamic>(await r.Content.ReadAsStringAsync());
            return (string)content["name"];
        }

        public async Task<(Uri player, Uri insights)> GetWidgetAsync(string videoId)
        {
            var (client, token) = await CreateVideoHttpClient(videoId);
            var insightsWidget = await client.GetAsync($"{ApiUrl}/{Config.VideoIndexerRegion}/Accounts/{Config.VideoIndexerAccountId}/Videos/{videoId}/InsightsWidget?accessToken={token}");
            var playerWidget = await client.GetAsync($"{ApiUrl}/{Config.VideoIndexerRegion}/Accounts/{Config.VideoIndexerAccountId}/Videos/{videoId}/PlayerWidget?accessToken={token}");
            return (insightsWidget.Headers.Location, playerWidget.Headers.Location);
        }

        public async Task<GetVideosResult> GetVideosAsync(int? pageSize = null, int? skip = null)
        {
            string createPageParams()
            {
                if (pageSize == null || skip == null)
                {
                    return "";
                }

                return $"&pageSize={pageSize}&skip={skip}";
            }
            var (client, token) = await CreateHttpClient();
            var r = await client.GetAsync($"{ApiUrl}/{Config.VideoIndexerRegion}/Accounts/{Config.VideoIndexerAccountId}/Videos?accessToken={token}{createPageParams()}");
            r.EnsureSuccessStatusCode();
            return GetVideosResult.FromJson(await r.Content.ReadAsStringAsync());
        }
    }
}
