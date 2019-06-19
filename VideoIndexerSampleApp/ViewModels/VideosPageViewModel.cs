using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using VideoIndexerSampleApp.VideoIndexer.GetVideos;
using VideoIndexerSampleApp.Mvvm;
using VideoIndexerSampleApp.UseCases;

namespace VideoIndexerSampleApp.ViewModels
{
    public class VideosPageViewModel : ViewModelBase
    {
        public VideosPageViewModel(INavigationService navigationService, 
            IVideoManageUseCase videoManageUseCase, 
            IDialogService dialogService) : base(navigationService)
        {
            VideoManageUseCase = videoManageUseCase;
            DialogService = dialogService;
        }

        public IVideoManageUseCase VideoManageUseCase { get; }
        public IDialogService DialogService { get; }

        private Uri _playerWidgetUri;
        public Uri PlayerWidgetUri
        {
            get { return _playerWidgetUri; }
            private set { SetProperty(ref _playerWidgetUri, value); }
        }

        private Uri _insightsWidgetUri;
        public Uri InsightsWidgetUri
        {
            get { return _insightsWidgetUri; }
            set { SetProperty(ref _insightsWidgetUri, value); }
        }

        private bool _isLoadingVideos;
        public bool IsLoadingVideos
        {
            get { return _isLoadingVideos; }
            set { SetProperty(ref _isLoadingVideos, value); }
        }

        public ReadOnlyObservableCollection<Result> Videos => VideoManageUseCase.Videos;

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadVideosAsync();
        }

        public async void LoadViceos()
        {
            await LoadVideosAsync();
        }

        public async Task SetSelectedVideoAsync(Result video)
        {
            if (IsLoadingVideos) { return; }
            IsLoadingVideos = true;
            try
            {
                await VideoManageUseCase.SetActiveVideoAsync(video);
                PlayerWidgetUri = VideoManageUseCase.PlayerWidgetUri;
                InsightsWidgetUri = VideoManageUseCase.InsightsWidgetUri;
            }
            catch (Exception ex)
            {
                await DialogService.ShowAsync(ex.Message);
            }
            finally
            {
                IsLoadingVideos = false;
            }
        }

        private async Task LoadVideosAsync()
        {
            if (IsLoadingVideos) { return; }
            IsLoadingVideos = true;
            try
            {
                await VideoManageUseCase.ReloadVideosAsync();
            }
            catch (Exception ex)
            {
                await DialogService.ShowAsync(ex.Message);
            }
            finally
            {
                IsLoadingVideos = false;
            }
        }
    }
}
