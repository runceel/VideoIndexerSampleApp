using Prism.Navigation;
using System;
using System.IO;
using System.Threading.Tasks;
using VideoIndexerSampleApp.UseCases;
using Windows.Storage;
using VideoIndexerSampleApp.Mvvm;
using System.Collections.ObjectModel;
using VideoIndexerSampleApp.VideoIndexer.GetVideos;

namespace VideoIndexerSampleApp.ViewModels
{
    public class CapturePageViewModel : ViewModelBase
    {
        public CapturePageViewModel(INavigationService navigationService,
            IVideoManageUseCase videoManageUseCase,
            IDialogService dialogService) : base(navigationService)
        {
            VideoManageUseCase = videoManageUseCase;
            DialogService = dialogService;
        }

        public IVideoManageUseCase VideoManageUseCase { get; }
        public IDialogService DialogService { get; }

        public async Task UploadVideoAsync(StorageFile video)
        {
            try
            {
                using (var s = (await video.OpenReadAsync()).AsStreamForRead())
                {
                    var name = await VideoManageUseCase.UploadVideoAsync(video.Name, s);
                    await DialogService.ShowAsync($"アップロードが完了しました。ビデオ名：{name}");
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowAsync(ex.Message);
            }
        }

    }
}
