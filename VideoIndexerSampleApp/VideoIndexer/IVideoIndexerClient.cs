using System;
using System.Threading.Tasks;
using VideoIndexerSampleApp.VideoIndexer.GetVideos;

namespace VideoIndexerSampleApp.VideoIndexer
{
    public interface IVideoIndexerClient
    {
        Task<string> UploadVideoToVideoIndexerAsync(Uri uri);
        Task<(Uri player, Uri insights)> GetWidgetAsync(string videoId);
        Task<GetVideosResult> GetVideosAsync(int? pageSize = null, int? skip = null);
    }
}
