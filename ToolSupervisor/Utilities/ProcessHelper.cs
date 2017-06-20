using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSupervisor.Utilities
{
    public static class ProcessHelper
    {
        public static void KillProcess(string name)
        {
            if (name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                name = name.Replace(".exe", String.Empty);

            foreach (var process in Process.GetProcessesByName(name))
            {
                process.Kill();
            }
        }

        public static void StartProcess(string path)
        {
            Process.Start(new ProcessStartInfo() { WorkingDirectory = Path.GetDirectoryName(path), FileName = path });
        }

        public static bool IsProcessRunning(string name)
        {
            if (name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                name = name.Replace(".exe", String.Empty);

            return Process.GetProcessesByName(name).Count() > 0;
        }
    }
}
