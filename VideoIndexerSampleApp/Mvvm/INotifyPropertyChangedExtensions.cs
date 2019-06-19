using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;

namespace VideoIndexerSampleApp.Mvvm
{
    public static class INotifyPropertyChangedExtensions
    {
        public static bool SetProperty<T>(this INotifyPropertyChanged self, ref T field, T value, PropertyChangedEventHandler handler, [CallerMemberName] string propertyName = null) => SetProperty(self, ref field, value, null, handler, propertyName);
        public static bool SetProperty<T>(this INotifyPropertyChanged self, ref T field, T value, Action onChanged, PropertyChangedEventHandler handler, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            handler?.Invoke(self, new PropertyChangedEventArgs(propertyName));
            onChanged?.Invoke();
            return true;
        }
    }
}
