using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerSampleApp.Repositories
{
    public interface IStorageRepository
    {
        Task<Uri> UploadVideoAsync(string name, System.IO.Stream video);
    }
}
