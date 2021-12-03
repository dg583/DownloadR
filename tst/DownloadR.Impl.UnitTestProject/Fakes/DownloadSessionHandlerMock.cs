using System.Threading.Tasks;
using DownloadR.Session;
using Microsoft.Extensions.Logging;

namespace DownloadR.Impl.UnitTestProject.Fakes
{
    public class DownloadSessionHandlerMock : DownloadSessionHandler {
        public DownloadSessionHandlerMock(DownloadHandlerOptions options, IDownloadTaskBuilder downloadTaskBuilder, ILogger<DownloadSessionHandler> logger) : base(options, downloadTaskBuilder, logger) {
        }

        public int StartSessionAsyncCalls { get; private set; }
        public override Task StartSessionAsync(DownloadSession downloadSession)
        {
            ++this.StartSessionAsyncCalls;

            return base.StartSessionAsync(downloadSession);
        }
    }
}