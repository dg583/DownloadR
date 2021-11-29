using System;

namespace DownloadR.Session {

    /// <summary>
    /// Definition of a file to download
    /// </summary>
    public class DownloadFileOptions : IEquatable<DownloadFileOptions>, ICloneable {
        private string _sha1 = string.Empty;
        private string _sha256 = string.Empty;
        private string _file = string.Empty;
        private string _url = string.Empty;

        /// <summary>
        /// <see cref="Uri"/> to the file that should be downloaded
        /// </summary>
        public string Url {
            get => this._url;
            set => this._url = value ?? string.Empty;
        }

        /// <summary>
        /// The name for the downloaded file
        /// </summary>
        public string File {
            get => this._file;
            set => this._file = value ?? string.Empty;
        }

        /// <summary>
        /// Determines whether an existing file should be deleted or not
        /// </summary>
        public bool Overwrite { get; set; } = false;
        //TODO: What, it false and file exists?
        //TODO: Default? False to avoid unwanted override

        /// <summary>
        /// Expected SHA256 of the downloaded file
        /// </summary>
        public string Sha256 {
            get => this._sha256;
            set => this._sha256 = value ?? string.Empty;
        }

        /// <summary>
        /// Expected SHA1 of the downloaded file
        /// </summary>
        public string Sha1 {
            get => this._sha1;
            set => this._sha1 = value ?? string.Empty;
        }


        public bool Equals(DownloadFileOptions other) {
            if(ReferenceEquals(null, other)) return false;
            if(ReferenceEquals(this, other)) return true;
            return this.GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj)) return false;
            if(ReferenceEquals(this, obj)) return true;
            if(obj.GetType() != this.GetType()) return false;
            return Equals((DownloadFileOptions)obj);
        }

        public override int GetHashCode() {
            // ReSharper disable once NonReadonlyMemberInGetHashCode: null is handled in HashCode.Combine
            return HashCode.Combine(this.Url, this.File, this.Overwrite, this.Sha256, this.Sha1);
        }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}
