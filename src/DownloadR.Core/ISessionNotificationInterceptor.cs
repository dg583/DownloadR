﻿using DownloadR.Session;

namespace DownloadR
{
    /// <summary>
    /// Provides methods to intercept <see cref="FileDownloadStatusReport"/> for a <see cref="DownloadSession"/>
    /// </summary>
    public interface ISessionNotificationInterceptor
    {
        void StartSession(IDownloadSessionHandler downloadSessionHandler);

        void SessionCompleted();

        void OnFileDownloadStatusReport(FileDownloadStatusReport fileDownloadStatusReport);
    }
}