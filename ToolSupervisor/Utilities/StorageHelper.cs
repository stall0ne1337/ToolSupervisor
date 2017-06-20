using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ToolSupervisor.Utilities
{
    public static class StorageHelper
    {
        public static void SaveTools(List<ManagedTool> scoreList)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, scoreList);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                Properties.Settings.Default.Tools = Convert.ToBase64String(buffer);
                Properties.Settings.Default.Save();
            }
        }

        public static List<ManagedTool> LoadTools()
        {
            if (String.IsNullOrWhiteSpace(Properties.Settings.Default.Tools))
                return new List<ManagedTool>();

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Properties.Settings.Default.Tools)))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (List<ManagedTool>)bf.Deserialize(ms);
            }
        }
    }
}
