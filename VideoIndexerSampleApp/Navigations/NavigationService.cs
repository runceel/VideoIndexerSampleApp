using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Windows.UI.Xaml.Controls;

namespace VideoIndexerSampleApp.Navigations
{
    public class NavigationService : INavigationService
    {
        private INavigationResult Success => new NavigationResult { Success = true };
        private readonly Frame _frame;
        private readonly IUnityContainer _container;

        public NavigationService(Frame frame, IUnityContainer container)
        {
            _frame = frame;
            _container = container;
        }

        public Task<INavigationResult> GoBackAsync()
        {
            if (!_frame.CanGoBack)
            {
                return Task.FromResult<INavigationResult>(new NavigationResult { Success = false });
            }

            _frame.GoBack();
            return Task.FromResult(Success);
        }

        public Task<INavigationResult> GoBackAsync(INavigationParameters parameters)
        {
            throw new NotSupportedException();
        }

        public Task<INavigationResult> NavigateAsync(Uri uri)
        {
            throw new NotSupportedException();
        }

        public Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameters)
        {
            throw new NotSupportedException();
        }

        public Task<INavigationResult> NavigateAsync(string name)
        {
            return Task.FromResult<INavigationResult>(new NavigationResult
            {
                Success = _frame.Navigate(_container.Registrations.First(x => x.Name == name).RegisteredType)
            });
        }

        public Task<INavigationResult> NavigateAsync(string name, INavigationParameters parameters)
        {
            return Task.FromResult<INavigationResult>(new NavigationResult
            {
                Success = _frame.Navigate(_container.Registrations.First(x => x.Name == name).RegisteredType, parameters)
            });
        }
    }
}
