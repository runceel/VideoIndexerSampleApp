using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace VideoIndexerSampleApp.Mvvm
{
    public class DialogService : IDialogService
    {
        public async Task ShowAsync(string content, string title = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                await new MessageDialog(content).ShowAsync();
            }
            else
            {
                await new MessageDialog(content, title).ShowAsync();
            }
        }
    }
}
