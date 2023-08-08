using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;

namespace garbage_process_killer
{
    internal class Program
    {
        static float totalMemoryCleared;
        
        
        static void Main(string[] args)
        {
            if(!IsAdministrator())
            {
                Console.WriteLine("RUN AS ADMINISTRATOR!");
                Console.ReadLine();
                return;
            }
            if(loadKillList() != null)
            {
                foreach (var name in loadKillList())
                {
                    Kill(name);
                }
            }

            //Process.Start("GarbageProcessKillerWithUI.exe");
            //Process.Start("GarbageAutoKiller.exe");
            ExecuteAsAdmin("GarbageAutoKiller.exe");
            Console.ReadLine();
        }
        static void ExecuteAsAdmin(string fileName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }


        static void Kill(string processName)
        {
            if(Process.GetProcessesByName(processName).Length == 0) {
                return;
            }

            foreach (var process in Process.GetProcessesByName(processName))
            {
                totalMemoryCleared += process.WorkingSet64 / 1048576;
                Console.WriteLine($"Killed {process.ProcessName}.exe, {process.WorkingSet64} bytes ({process.WorkingSet64 / 1048576} mb.) Total cleared: {totalMemoryCleared}mb");
                process.Kill();
            }
        }
        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        static List<string> loadKillList()
        {
            if(!File.Exists("killList.txt"))
            {
                Console.WriteLine("Cannot found killList.txt!");
                return null;
            }
            
            List<string> loaded = new List<string>();   
            
            foreach(var x in File.ReadAllLines("killList.txt"))
            {
                loaded.Add(x);
            }
        
            return loaded;
        } 
    }
}
