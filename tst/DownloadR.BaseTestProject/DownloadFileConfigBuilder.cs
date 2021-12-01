using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace DownloadR.BaseTestProject
{
    public class DownloadFileConfigBuilder
    {
        public static DownloadFileConfig GetValid()
        {

            string outFile = Path.Combine(TestContext.CurrentContext.WorkDirectory, "out.dat");
            string uri = "https://file.dev/file.dat";

            DownloadFileConfig config = new DownloadFileConfig(outFile, uri);

            return config;
        }
    }
}
