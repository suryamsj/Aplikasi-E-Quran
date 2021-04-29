using Alquran.url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alquran.connection
{
    class RequestWeb : IRequestWeb
    {
        public string GetReleases(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = APIUrl.UserAgentValue;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            var content = string.Empty;
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Gagal mengambil data, silahkan coba lagi", "Gagal",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            return content;
        }
    }
}
