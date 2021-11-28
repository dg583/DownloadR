namespace DownloadR {
    public static class Verify {
        public static void ThrowIfNotSet(this string value, string paramName) {
            if(string.IsNullOrEmpty(value?.Trim())) {
                throw new ParamNotSetException(paramName);
            }
        }
    }
}
