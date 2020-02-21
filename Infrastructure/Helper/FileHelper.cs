using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace GreenSQL.Infrastructure.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <param name="overwrite"></param>
        public static void Copy(string src, string dest, bool overwrite = true)
        {
            src = src.TrimEnd('\\');
            dest = dest.TrimEnd('\\');

            if(File.Exists(src)) {
                Directory.CreateDirectory(dest.Substring(0, dest.LastIndexOf('\\')));
                File.Copy(src, dest, overwrite);
            } else {
                Directory.CreateDirectory(dest);
                foreach(var s in Directory.GetFileSystemEntries(src)) {
                    Copy(s, $"{dest}{s.Substring(s.LastIndexOf("\\"))}", overwrite);
                }
            }
        }

        public static void Delete(string path)
        {
            if(File.Exists(path)) {
                File.Delete(path);
            } else {
                Directory.Delete(path, true);
            }
        }

        public static bool Download(string url, string dest)
        {
            if(File.Exists(dest)) return false;

            var req = HttpWebRequest.Create(url);
            var resp = (HttpWebResponse)req.GetResponse();
            if(HttpStatusCode.OK != resp.StatusCode) return false;

            var inStream = resp.GetResponseStream();
            var outStream = new FileStream(dest, FileMode.OpenOrCreate);
            inStream.CopyTo(outStream);
            inStream.Close();
            outStream.Flush();
            outStream.Close();
            return true;
        }
        
        public static bool Extract(string src, string dest)
        {
            ZipFile.ExtractToDirectory(src, dest);
            return true;
        }

        /// <summary>
        ///  计算指定文件的SHA1值
        /// </summary>
        /// <param name="fileName">指定文件的完全限定名称</param>
        /// <returns>返回值的字符串形式</returns>
        public static bool CheckSHA1(string fileName, string sha1)
        {
            var hashSHA1 = string.Empty;
            if (File.Exists(fileName))
            {
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    System.Security.Cryptography.SHA1 calculator = System.Security.Cryptography.SHA1.Create();
                    byte[] buffer = calculator.ComputeHash(fs);
                    calculator.Clear();
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("X2"));
                    }
                    hashSHA1 = stringBuilder.ToString();
                }
            }
            return hashSHA1.Equals(sha1.ToUpper());
        }

        public static long SizeOf(string path) {
            if(File.Exists(path)) {
                return (new FileInfo(path)).Length;
            } else {
                long size = 0;
　　　　         foreach (var p in Directory.GetFileSystemEntries(path))
                    size += SizeOf(p);
                return size;
            }
        }

        public static Process Execute(string exe, string args = null, bool isHidden = false, bool useShell = false)
        {
            var p = new Process();
            p.StartInfo.FileName = exe;
            p.StartInfo.Arguments = args;
            p.StartInfo.CreateNoWindow = isHidden;
            p.StartInfo.UseShellExecute = useShell;
            p.Start();
            return p;
        }
        public static Process ExecutePS1(string ps1, bool isHidden = true) => Execute("powershell.exe", $"-NoLogo -NoExit -NoProfile -NonInteractive -File {ps1}", isHidden);
    }
}
