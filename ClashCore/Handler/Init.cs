using ClashN;
using ClashN.Mode;
using System.Collections.Generic;
using System.Linq;
namespace ClashCore.Handler
{
    class Init
    {
        private List<ClashN.Mode.CoreInfo> coreInfos;
        public CoreInfo GetCoreInfo(CoreKind coreType)
        {
            if (coreInfos == null)
            {
                InitCoreInfo();
            }
            return coreInfos.Where(t => t.coreType == coreType).FirstOrDefault();
        }

        private void InitCoreInfo()
        {
            coreInfos = new List<CoreInfo>();

            coreInfos.Add(new CoreInfo
            {
                coreType = CoreKind.ClashN,
                coreUrl = Global.NUrl,
                coreLatestUrl = Global.NUrl + "/latest",
                coreDownloadUrl32 = Global.NUrl + "/download/{0}/clashN-32.zip",
                coreDownloadUrl64 = Global.NUrl + "/download/{0}/clashN.zip",
            });

            coreInfos.Add(new CoreInfo
            {
                coreType = CoreKind.Clash,
                coreExes = new List<string> { "clash-windows-amd64-v3", "clash-windows-amd64", "clash-windows-386", "clash" },
                arguments = "-f config.yaml",
                coreUrl = Global.clashCoreUrl,
                coreLatestUrl = Global.clashCoreUrl + "/latest",
                coreDownloadUrl32 = Global.clashCoreUrl + "/download/{0}/clash-windows-386-{0}.zip",
                coreDownloadUrl64 = Global.clashCoreUrl + "/download/{0}/clash-windows-amd64-{0}.zip",
                match = "Clash"
            });

            coreInfos.Add(new CoreInfo
            {
                coreType = CoreKind.ClashMeta,
                coreExes = new List<string> {"Clash.Meta-windows-amd64-compatible", "Clash.Meta-windows-amd64", "Clash.Meta-windows-386", "Clash.Meta", "clash" },
                arguments = "-f config.yaml",
                coreUrl = Global.clashMetaCoreUrl,
                coreLatestUrl = Global.clashMetaCoreUrl + "/latest",
                coreDownloadUrl32 = Global.clashMetaCoreUrl + "/download/{0}/Clash.Meta-windows-386-{0}.zip",
                coreDownloadUrl64 = Global.clashMetaCoreUrl + "/download/{0}/Clash.Meta-windows-amd64"  + "-{0}.zip",
                match = "Clash Meta"
            });

            coreInfos.Add(new CoreInfo
            {
                coreType = CoreKind.ClashPremium,
                coreExes = new List<string> { "clash-windows-amd64-v3", "clash-windows-amd64", "clash-windows-386", "clash" },
                arguments = "-f config.yaml",
                coreUrl = Global.clashCoreUrl,
                coreLatestUrl = Global.clashCoreUrl + "/latest",
                coreDownloadUrl32 = Global.clashCoreUrl + "/download/{0}/clash-windows-386-{0}.zip",
                coreDownloadUrl64 = Global.clashCoreUrl + "/download/{0}/clash-windows-amd64-{0}.zip",
                match = "Clash"
            });
        }
    }
}
