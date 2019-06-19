using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VideoIndexerSampleApp.Mvvm;
using VideoIndexerSampleApp.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace VideoIndexerSampleApp.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class CapturePage : Page, INotifyPropertyChanged
    {
        private MediaCapture _mediaCapture;
        private LowLagMediaRecording _mediaRecording;
        private StorageFile _video;

        private CapturePageViewModel ViewModel => DataContext as CapturePageViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isCaptureEnabled = true;
        public bool IsCaptureEnabled
        {
            get { return _isCaptureEnabled; }
            set { this.SetProperty(ref _isCaptureEnabled, value, PropertyChanged); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCaptureStopEnabled))); }
        }

        public bool IsCaptureStopEnabled => !IsCaptureEnabled;

        public CapturePage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _mediaCapture = new MediaCapture();
            await _mediaCapture.InitializeAsync();
            captureElement.Source = _mediaCapture;
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (_mediaRecording != null)
            {
                await _mediaRecording.StopAsync();
                await _mediaRecording.FinishAsync();
            }

            _mediaCapture.Dispose();
        }

        private async void CaptureStartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsCaptureEnabled) { return; }
            IsCaptureEnabled = false;
            await _mediaCapture.StartPreviewAsync();

            _video = await ApplicationData.Current.LocalFolder.CreateFileAsync($"{Guid.NewGuid()}.mp4");
            _mediaRecording = await _mediaCapture.PrepareLowLagRecordToStorageFileAsync(
                MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Auto), _video);
            await _mediaRecording.StartAsync();
        }

        private async void CaptureStopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ = uploadProgressDialog.ShowAsync();
                if (!IsCaptureEnabled)
                {
                    IsCaptureEnabled = true;
                    await _mediaRecording.StopAsync();
                    await _mediaRecording.FinishAsync();
                    await _mediaCapture.StopPreviewAsync();


                    await ViewModel.UploadVideoAsync(_video);
                    await _video.DeleteAsync();
                    _mediaRecording = null;
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            finally
            {
                uploadProgressDialog.Hide();
            }
        }
    }
}
