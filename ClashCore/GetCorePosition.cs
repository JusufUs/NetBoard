using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashCore
{
    class GetCorePosition
    {
        public static string GetExecution()
        {
            string fileName = string.Empty;
            fileName = System.Environment.CurrentDirectory + @"\bin\Clash\clash-windows-amd64.exe";
            return fileName;
        }
    }
}
