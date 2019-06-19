using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerSampleApp.VideoIndexer.GetVideos;

namespace VideoIndexerSampleApp.UseCases
{
    public interface IVideoManageUseCase : INotifyPropertyChanged
    {
        AppSettings AppSettings { get; }
        System.Collections.ObjectModel.ReadOnlyObservableCollection<Result> Videos { get; }
        Result SelectedVideo { get; }
        Uri InsightsWidgetUri { get; }
        Uri PlayerWidgetUri { get; }
        bool IsValidSettings { get; }

        Task ReloadVideosAsync();
        Task SetActiveVideoAsync(Result video);
        Task<string> UploadVideoAsync(string name, System.IO.Stream video);
    }
}
