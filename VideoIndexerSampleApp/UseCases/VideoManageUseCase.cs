using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using VideoIndexerSampleApp.VideoIndexer.GetVideos;
using VideoIndexerSampleApp.Repositories;
using VideoIndexerSampleApp.VideoIndexer;

namespace VideoIndexerSampleApp.UseCases
{
    public class VideoManageUseCase : BindableBase, IVideoManageUseCase
    {
        private readonly ObservableCollection<Result> _videos;
        public ReadOnlyObservableCollection<Result> Videos { get; }

        private Result _selectedVideo;
        public Result SelectedVideo
        {
            get { return _selectedVideo; }
            private set { SetProperty(ref _selectedVideo, value); }
        }

        public VideoManageUseCase(AppSettings appSettings, IStorageRepository storageRepository, IVideoIndexerClient videoIndexerClient)
        {
            AppSettings = appSettings;
            StorageRepository = storageRepository;
            VideoIndexerClient = videoIndexerClient;
            Videos = new ReadOnlyObservableCollection<Result>(_videos = new ObservableCollection<Result>());
        }

        public AppSettings AppSettings { get; }
        public IStorageRepository StorageRepository { get; }
        public IVideoIndexerClient VideoIndexerClient { get; }

        private Uri _insightsWidgetUri;
        public Uri InsightsWidgetUri
        {
            get { return _insightsWidgetUri; }
            private set { SetProperty(ref _insightsWidgetUri, value); }
        }

        private Uri _playerWidgetUri;
        public Uri PlayerWidgetUri
        {
            get { return _playerWidgetUri; }
            private set { SetProperty(ref _playerWidgetUri, value); }
        }

        public bool IsValidSettings => AppSettings.IsValid;

        public async Task<string> UploadVideoAsync(string name, Stream video)
        {
            var uri = await StorageRepository.UploadVideoAsync(name, video);
            return await VideoIndexerClient.UploadVideoToVideoIndexerAsync(uri);
        }

        public async Task ReloadVideosAsync()
        {
            _videos.Clear();
            GetVideosResult videos;
            do
            {
                videos = await VideoIndexerClient.GetVideosAsync();
                foreach (var video in videos.Results)
                {
                    _videos.Add(video);
                }
            } while (videos.NextPage != null && !videos.NextPage.Done);
        }

        public async Task SetActiveVideoAsync(Result video)
        {
            SelectedVideo = video;
            var widgets = await VideoIndexerClient.GetWidgetAsync(video.Id);
            PlayerWidgetUri = widgets.player;
            InsightsWidgetUri = widgets.insights;
        }

    }
}
