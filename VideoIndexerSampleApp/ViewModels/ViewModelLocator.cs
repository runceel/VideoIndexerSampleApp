using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;
using VideoIndexerSampleApp.Mvvm;
using VideoIndexerSampleApp.Navigations;
using VideoIndexerSampleApp.Repositories;
using VideoIndexerSampleApp.UseCases;
using VideoIndexerSampleApp.VideoIndexer;
using VideoIndexerSampleApp.Views;
using Windows.UI.Xaml.Controls;

namespace VideoIndexerSampleApp.ViewModels
{
    public class ViewModelLocator
    {
        private IUnityContainer _container;

        public CapturePageViewModel CapturePageViewModel => _container.Resolve<CapturePageViewModel>();
        public VideosPageViewModel VideosPageViewModel => _container.Resolve<VideosPageViewModel>();
        public SettingsPageViewModel SettingsPageViewModel => _container.Resolve<SettingsPageViewModel>();

        public Page Initialize()
        {
            _container = new UnityContainer();

            // register infrastructur services
            var appSettings = new AppSettings();
            appSettings.Restore();
            _container.RegisterInstance(appSettings);
            _container.RegisterInstance(_container);
            _container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());

            // register services
            _container.RegisterType<IVideoManageUseCase, VideoManageUseCase>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IVideoIndexerClient, VideoIndexerClient>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IStorageRepository, StorageRepository>(new ContainerControlledLifetimeManager());

            // register pages
            _container.RegisterTypeForNavigation<CapturePage>();
            _container.RegisterTypeForNavigation<VideosPage>();
            _container.RegisterTypeForNavigation<SettingsPage>();

            return SetupNavigationService(appSettings);
        }

        private Page SetupNavigationService(AppSettings appSettings)
        {
            bool tryGetCurrentViewModel(Frame frame, out INavigationAware navigationAware)
            {
                if(frame.Content is Page currentPage && currentPage.DataContext is INavigationAware currentViewModel)
                {
                    navigationAware = currentViewModel;
                    return true;
                }

                navigationAware = null;
                return false;
            }

            var rootPage = new MainPage();
            var rootFrame = rootPage.MainFrame;
            rootFrame.Navigating += (s, e) =>
            {
                if (tryGetCurrentViewModel(rootFrame, out var currentViewModel))
                {
                    currentViewModel.OnNavigatedFrom(e.Parameter as INavigationParameters);
                }
            };
            rootFrame.Navigated += (s, e) =>
            {
                if (tryGetCurrentViewModel(rootFrame, out var currentViewModel))
                {
                    currentViewModel.OnNavigatingTo(e.Parameter as INavigationParameters);
                    currentViewModel.OnNavigatedTo(e.Parameter as INavigationParameters);
                }
            };

            var navigationService = new NavigationService(rootFrame, _container);
            _container.RegisterInstance<INavigationService>(navigationService);

            rootPage.SetNavigationServiceAndInitialize(navigationService, appSettings);
            return rootPage;
        }
    }

    static class UnityContainerExtensions
    {
        public static void RegisterTypeForNavigation<T>(this IUnityContainer self)
        {
            self.RegisterType<T>(typeof(T).Name);
        }
    }
}
