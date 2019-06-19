using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity;
using VideoIndexerSampleApp.Navigations;
using VideoIndexerSampleApp.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class MainPage : Page
    {
        private AppSettings AppSettings { get; set; }
        public INavigationService NavigationService { get; private set; }

        public MainPage()
        {
            InitializeComponent();
        }

        private void NavigationView_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked || !AppSettings.IsValid)
            {
                _ = NavigationService.NavigateAsync("SettingsPage");
                return;
            }

            _ = NavigationService.NavigateAsync(NavigationPage.GetName(args.InvokedItemContainer));
        }

        public void SetNavigationServiceAndInitialize(NavigationService navigationService, AppSettings appSettings)
        {
            AppSettings = appSettings;
            NavigationService = navigationService;
            navigationView.SelectedItem = navigationView.MenuItems.First();
            _ = NavigationService.NavigateAsync(appSettings.IsValid ? NavigationPage.GetName((DependencyObject)navigationView.SelectedItem) : "SettingsPage");
        }
    }
}
