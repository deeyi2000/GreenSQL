using GreenSQL.Infrastructure.Helper;
using Microsoft.Win32;
using System.Collections.Generic;

namespace GreenSQL.Domain.Model
{
    public class Instance
    {
        private const string SQLSERVER_PATH = @"SOFTWARE\Microsoft\Microsoft SQL Server";

        public string InstanceName { get; private set; }
        public string InstanceId { get; private set; }
        public string Edition { get; private set; }
        public string Version { get; private set; }
        public string Path { get; private set; }
        public bool isGreen { get; private set; }
        public long Size { get => FileHelper.SizeOf($"{Path}\\{InstanceId}"); }

        private RegistryKey _regInst;

        public bool Uninstall()
        {
            if (!isGreen)
                return false;

            var regSQL = Registry.LocalMachine.OpenSubKey($"{SQLSERVER_PATH}\\", true);
            var strInstalls = regSQL.GetValue("InstalledInstances") as string[];
            if (null != strInstalls && 0 != strInstalls.Length)
            {
                strInstalls = System.Array.FindAll(strInstalls, i => i != InstanceName);
                regSQL.SetValue("InstalledInstances", strInstalls);
            }
            FileHelper.ExecutePS1($"{Path}\\{InstanceId}\\script.ps1 {InstanceName} un");
            return true;
        }

        public bool Start() {
            if (!isGreen)
                return false;

            FileHelper.ExecutePS1($"{Path}\\{InstanceId}\\script.ps1 {InstanceName} start");
            return true;
        }

        public bool Stop() {
            if (!isGreen)
                return false;

            FileHelper.ExecutePS1($"{Path}\\{InstanceId}\\script.ps1 {InstanceName} stop");
            return true;
        }

        public static Instance Load(string id)
        {
            var regInst = Registry.LocalMachine.OpenSubKey($"{SQLSERVER_PATH}\\{id}");
            if (null == regInst)
                return null;

            return new Instance()
            {
                _regInst = regInst,
                InstanceName = id.Substring(id.IndexOf('.') + 1),
                InstanceId = id,
                Edition = regInst.OpenSubKey("Setup").GetValue("Edition") as string,
                Version = regInst.OpenSubKey("Setup").GetValue("PatchLevel") as string,
                Path = regInst.OpenSubKey("Setup").GetValue("SqlProgramDir") as string,
                isGreen = (null != regInst.GetValue("GreenSQL") ? true : false)
            };
        }

        public static Instance[] LoadAll()
        {
            var strInstalls = Registry.LocalMachine.OpenSubKey(SQLSERVER_PATH)?.GetValue("InstalledInstances") as string[];
            if (null == strInstalls)
                return null;

            var insts = new List<Instance>(strInstalls.Length);
            var regInstName = Registry.LocalMachine.OpenSubKey($"{SQLSERVER_PATH}\\Instance Names\\SQL");
            string strTmp;
            Instance tmp;
            foreach (var i in strInstalls)
            {
                strTmp = regInstName.GetValue(i) as string;
                tmp = Load(strTmp);
                if (null != tmp) insts.Add(tmp);
            }
            return insts.ToArray();
        }
    }
}
