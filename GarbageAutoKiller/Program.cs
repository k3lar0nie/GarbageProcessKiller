using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Threading;

namespace GarbageAutoKiller
{
    internal class Program
    {
        private static string killedName;

        static void Main(string[] args)
        {
            if(!IsAdministrator())
            {
                Console.WriteLine("RUN AS ADMIN!!");
                Console.ReadLine();
                
            }

            if (File.Exists("killListAuto.txt"))
            {
                string[] processNames = File.ReadAllLines("killListAuto.txt");
                List<Process> processes = new List<Process>();

                while (true)
                {
                    foreach (var x in processNames)
                    {
                        processes.AddRange(Process.GetProcessesByName(x));
                    }

                    for (int i = 0; i < processes.Count; i++)
                    {
                        if (!processes[i].HasExited)
                        {
                            killedName = processes[i].ProcessName;
                            processes[i].Kill();
                            Console.WriteLine($"Killed {killedName}");
                        }
                    }
                    Thread.Sleep(100);
                }
            }
            else
            {
                Console.WriteLine("ERROR KAKOY TO");
                Console.ReadLine();
            }
        }

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
