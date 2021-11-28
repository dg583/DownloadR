using System;
using System.Runtime.Serialization;

namespace DownloadR {

    [Serializable]
    public class ParamNotSetException : ArgumentException {
        
        public ParamNotSetException(string paramName)
            : base($"{paramName} ist nicht gesetzt") {
        }

        protected ParamNotSetException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {
        }
    }
}
