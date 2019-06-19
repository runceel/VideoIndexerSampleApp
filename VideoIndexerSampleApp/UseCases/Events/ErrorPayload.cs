using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerSampleApp.UseCases.Events
{
    public class ErrorPayload
    {
        public ErrorPayload(string errorMessage, Exception error)
        {
            ErrorMessage = errorMessage;
            Error = error;
        }

        public string ErrorMessage { get; }
        public Exception Error { get; }
    }
}
