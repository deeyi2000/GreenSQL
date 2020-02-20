using System.Collections.Generic;
using System.Configuration;
using GreenSQL.Infrastructure.Helper;
using Microsoft.Win32;

namespace GreenSQL.Domain.Model
{
    public class Image
    {
        private const string SQLSERVER_PATH = @"SOFTWARE\Microsoft\Microsoft SQL Server";

        public string Id { get; }
        public string Name { get; set; }
        public string Edition { get; set; }
        public string Version { get; set; }
        public string Source {get; set; }
        public string FullName { get => $"{Name} {Edition} {Version}"; }
        public string Path { get => $"images\\{Id}.X"; }

        public Image(string id) => Id = id;

        public bool Install(string instanceName, string path = ".")
        {
            instanceName = instanceName.ToUpper();
            path = $"{path.TrimEnd('\\')}\\{Id}.{instanceName}";
            FileHelper.Copy(Path, path);
            FileHelper.ExecutePS1($"{path}\\script.ps1 {instanceName} i", false).WaitForExit(10 * 1000);
            Registry.LocalMachine.OpenSubKey($"{SQLSERVER_PATH}\\{Id}.{instanceName}", true).SetValue("GreenSQL", 1);
            var regSQL = Registry.LocalMachine.OpenSubKey($"{SQLSERVER_PATH}\\", true);
            var strInstalls = regSQL.GetValue("InstalledInstances") as string[];
            if (null == strInstalls)
            {
                strInstalls = new string[1];
            } else {
                System.Array.Resize(ref strInstalls, strInstalls.Length + 1);
            }
            strInstalls[strInstalls.Length - 1] = instanceName;
            regSQL.SetValue("InstalledInstances", strInstalls);
            return true;
        }

        public static Image[] LoadAll()
        {
            var images = ConfigurationManager.GetSection("imageSettings") as List<Image>;
            return images?.ToArray();
        }
    }
}
