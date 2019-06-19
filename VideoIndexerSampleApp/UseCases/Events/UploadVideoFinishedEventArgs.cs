using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerSampleApp.UseCases.Events
{
    public class UploadVideoFinishedEventArgs : EventArgs
    {
        public UploadVideoFinishedEventArgs(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
