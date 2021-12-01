using DownloadR.EventArguments;

namespace DownloadR {
    public static class Extensions {

        /// <summary>
        /// Converts a <see cref="DownloadFileNotificationEventArgs"/>into a <see cref="FileDownloadStatusReport"/>
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public static FileDownloadStatusReport ToFileDownloadStatusReport(
            this DownloadFileNotificationEventArgs eventArgs) {
            if(eventArgs.Status == FileDownloadStatusType.Failed)
                return new FileDownloadStatusReport(eventArgs.Context.DownloadConfiguration, eventArgs.Progress,
                    eventArgs.Exception);

            return new FileDownloadStatusReport(eventArgs.Context.DownloadConfiguration, eventArgs.Progress, eventArgs.Status);
        }
    }
}
