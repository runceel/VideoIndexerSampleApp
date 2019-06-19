using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace VideoIndexerSampleApp
{
    public class AppSettings : BindableBase
    {
        private static PropertyChangedEventArgs IsValidPropertyChangedEventArgs { get; } = new PropertyChangedEventArgs(nameof(IsValid));

        private string _storageAccountName;
        public string StorageAccountName
        {
            get { return _storageAccountName; }
            set { SetProperty(ref _storageAccountName, value, PropertyUpdated); }
        }

        private string _videoBlobContainerName = "videos";
        [JsonIgnore]
        public string VideoBlobContainerName
        {
            get { return _videoBlobContainerName; }
            set { SetProperty(ref _videoBlobContainerName, value, PropertyUpdated); }
        }

        private string _storageAccessKey;
        public string StorageAccessKey
        {
            get { return _storageAccessKey; }
            set { SetProperty(ref _storageAccessKey, value, PropertyUpdated); }
        }

        private string _videoIndexerAccountId;
        public string VideoIndexerAccountId
        {
            get { return _videoIndexerAccountId; }
            set { SetProperty(ref _videoIndexerAccountId, value, PropertyUpdated); }
        }

        private string _videoIndexerRegion;
        public string VideoIndexerRegion
        {
            get { return _videoIndexerRegion; }
            set { SetProperty(ref _videoIndexerRegion, value, PropertyUpdated); }
        }

        private string _videoIndexerApiKey;
        public string VideoIndexerApiKey
        {
            get { return _videoIndexerApiKey; }
            set { SetProperty(ref _videoIndexerApiKey, value, PropertyUpdated); }
        }

        private void PropertyUpdated()
        {
            OnPropertyChanged(IsValidPropertyChangedEventArgs);
            IsDirty = true;
        }

        [JsonIgnore]
        public bool IsValid =>
            !string.IsNullOrEmpty(StorageAccountName) &&
            !string.IsNullOrEmpty(StorageAccessKey) &&
            !string.IsNullOrEmpty(VideoIndexerAccountId) &&
            !string.IsNullOrEmpty(VideoIndexerRegion) &&
            !string.IsNullOrEmpty(VideoIndexerApiKey);

        private bool _isDirty;
        [JsonIgnore]
        public bool IsDirty
        {
            get { return _isDirty; }
            private set { SetProperty(ref _isDirty, value); }
        }

        public void Store()
        {
            ApplicationData.Current.LocalSettings.Values[nameof(AppSettings)] = JsonConvert.SerializeObject(this);
            IsDirty = false;
        }

        public void Restore()
        {
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(nameof(AppSettings), out var value) && value is string json)
            {
                JsonConvert.PopulateObject(json, this);
            }
            else
            {
                StorageAccountName = "";
                StorageAccessKey = "";
                VideoIndexerAccountId = "";
                VideoIndexerRegion = "";
                VideoIndexerApiKey = "";
            }

            IsDirty = false;
        }
    }
}
