using System;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging {
    

    public static class LoggerScopeExtensions {
        public static LoggerScope<T> BeginScope<T>(
            this ILogger<T> logger, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0) {

            return new LoggerScope<T>(logger, new CallerInfo(memberName, fileName, lineNumber));
        }
    }

    public class CallerInfo {
        public string MemberName { get; }
        public string FileName { get; }
        public int LineNumber { get; }

        public CallerInfo(string memberName, string fileName, int lineNumber) {
            MemberName = memberName;
            FileName = fileName;
            LineNumber = lineNumber;
        }
    }

    /// <summary>
    /// Enables file and method based tracing
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //TODO: This should be used for tracing
    public class LoggerScope<T> : IDisposable {
        private readonly ILogger<T> _logger;
        private readonly CallerInfo _callerInfo;

        public LoggerScope(ILogger<T> logger, CallerInfo callerInfo) {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._callerInfo = callerInfo;

            this._logger.LogTrace(string.Format("Enter '{0}.{1}'", this._callerInfo.FileName, this._callerInfo.MemberName));

        }

        public void Dispose() {
            this._logger.LogTrace(string.Format("Leaving '{0}.{1}'", this._callerInfo.FileName, this._callerInfo.MemberName));

        }
    }
}
