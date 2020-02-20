using System.Diagnostics;
using System.IO;

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
