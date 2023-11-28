using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Logging
{
    internal class Program
    {
        private static string _keyName = "LogKey";
        private static string _loggFile = "logg.txt";

        static void Main(string[] args)
        {
            if (!CheckForWindowsPlatform())
            {
                Console.WriteLine("Only windows platform has access to registry windows.\n" +
                    "Start program on windows!");
                return;
            }

            if (AddNoteToRegister())
                return;

            AddNoteToFile();
           
        }

        /// <summary>
        /// Checks if program has been started on windows platform 
        /// </summary>
        /// <returns>True if on windows; False if on another OS</returns>
        static bool CheckForWindowsPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return true;

            else
                return false;
        }

        /// <summary>
        /// At first launch it creates a key in Windows Register in CurrentUser directory 
        /// and adds note about launching 
        /// </summary>
        /// <returns>True if note was added to register; False if the note already exist</returns>
        static bool AddNoteToRegister()
        {
            RegistryKey registryKey = Registry.CurrentUser;

            if (registryKey.OpenSubKey(_keyName) is null)
            {
                RegistryKey logKey = registryKey.CreateSubKey(_keyName);
                logKey.SetValue("HasBeenLaunched", "YES");
                logKey.Close();

                return true;
            }
            
            registryKey.Close();

            return false;
        }

        /// <summary>
        /// Adds launch time to logg file
        /// </summary>
        static void AddNoteToFile()
        {
            using (FileStream file = new FileStream(_loggFile, FileMode.Append))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine(DateTime.Now.ToString());
                }
            }
        }
    }
}