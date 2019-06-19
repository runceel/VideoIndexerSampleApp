using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using VideoIndexerSampleApp.Mvvm;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace VideoIndexerSampleApp.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public static Brush ErrorBrush { get; } = new SolidColorBrush(Colors.Red);
        public static Brush WarnBrush { get; } = new SolidColorBrush(Colors.Yellow);
        public static Brush NormalBrush { get; } = new SolidColorBrush(Colors.Black);

        public SettingsPageViewModel(INavigationService navigationService, AppSettings appSettings, IDialogService dialogService) : base(navigationService)
        {
            AppSettings = appSettings;
            DialogService = dialogService;
        }

        public AppSettings AppSettings { get; }
        public IDialogService DialogService { get; }

        private string _message;
        public string Message
        {
            get { return _message; }
            private set { SetProperty(ref _message, value); }
        }

        private Brush _messageColor;
        public Brush MessageColor
        {
            get { return _messageColor; }
            private set { SetProperty(ref _messageColor, value); }
        }

        public async void Save()
        {
            AppSettings.Store();
            await DialogService.ShowAsync("設定を保存しました。");
        }

        public async void Restore()
        {
            AppSettings.Restore();
            await DialogService.ShowAsync("設定を元に戻しました。");
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            AppSettings.PropertyChanged += AppSettings_PropertyChanged;
            UpdateMessage();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            AppSettings.PropertyChanged -= AppSettings_PropertyChanged;
            if (AppSettings.IsDirty)
            {
                AppSettings.Restore();
            }
        }

        private void AppSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateMessage();
        }

        private void UpdateMessage()
        {
            if (!AppSettings.IsValid)
            {
                Message = "設定を入力してください。";
                MessageColor = ErrorBrush;
                return;
            }

            if (AppSettings.IsDirty)
            {
                Message = "設定を保存してください。";
                MessageColor = WarnBrush;
                return;
            }

            Message = "";
            MessageColor = NormalBrush;
        }
    }
}
