using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace JazzLoadRemover
{
    public class JazzpunkFinder
    {
        static string _SteamPath;
        public static string SteamPath => _SteamPath = _SteamPath 
                    ?? (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam","InstallPath", null)
                    ?? (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam","InstallPath", null);

        static string _JazzPath;
        public static string JazzpunkPath => _JazzPath = _JazzPath ?? FindJazzpunkFolder();

        static string _JazzAssemblyPath;
        public static string JazzAssemblyPath => _JazzAssemblyPath = _JazzAssemblyPath ?? (JazzpunkPath + @"\windows\jazzpunk_Data\Managed\Assembly-CSharp.dll");

        static string CheckForJazzpunk(string dir) => Directory.GetDirectories(dir, "Jazzpunk", SearchOption.AllDirectories).FirstOrDefault();

        static IEnumerable<string> GetInsallDirs()
        {
            yield return SteamPath + @"\steamapps\common";

            string libPath = SteamPath + @"\steamapps\libraryfolders.vdf";
            if (File.Exists(libPath))
            {
                string[] lines = File.ReadAllLines(libPath);

                //I'm lazy and will not write a parser for vdf files. 
                int index;
                foreach (var line in lines)
                    if ((index = line.IndexOf(":\\\\")) != -1)
                        yield return line.Substring(index - 1).TrimEnd('"') + @"\steamapps\common";
            }
        }

        static string FindJazzpunkFolder()
        {
            string ret;
            foreach(var path in GetInsallDirs())
            {
                if (!Directory.Exists(path)) continue;
                ret = CheckForJazzpunk(path);

                if (ret != null)
                    return ret;
            }

            return null;
        }

        static string GetMD5(string path)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
                using (var stream = File.OpenRead(path))
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
        }

        static void ExtractAssembly(string path)
        {
            File.WriteAllBytes(path, Properties.Resources.Assembly_CSharp);
        }

        public static void InstallAssembly()
        {
            if (!File.Exists(JazzAssemblyPath))
            {
                System.Windows.Forms.MessageBox.Show("Could not find the jazzpunk assembly!\nIf you're not using steam you'll have to install it manually","JazzSplitter");
                return;
            }

            if (GetMD5(JazzAssemblyPath) == Properties.Resources.AssemblyMD5)
                return;

            ExtractAssembly(JazzAssemblyPath);
            var process = Process.GetProcessesByName("Jazzpunk").FirstOrDefault();
            if (process != null)
                process.Kill();
             
            System.Windows.Forms.MessageBox.Show("Successfully installed the jazzpunk assembly.", "JazzSplitter");
        }

    }
}
