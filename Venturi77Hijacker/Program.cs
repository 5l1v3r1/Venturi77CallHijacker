using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.MD;
using dnlib.DotNet.Writer;
using Newtonsoft.Json;
using Venturi77CallHijacker;
using System.Threading;
using static Venturi77Hijacker.CallHijacker;

namespace Venturi77Hijacker {
    class Program {
        static void Main(string[] args) {

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[1] Inject Dll\n[2] Build Config\n[3] Build Debug Config\n[4] Parse #Koi Header");
            var key = Console.ReadKey();
            Console.Clear();
            if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.NumPad1) {
                InjectDll();
                return;
            }
            if (key.Key == ConsoleKey.D2 || key.Key == ConsoleKey.NumPad2) {
                ConfigBuilder config = new ConfigBuilder(false);
                config.Build();
                return;
            }
            if (key.Key == ConsoleKey.D3 || key.Key == ConsoleKey.NumPad3) {
                ConfigBuilder config = new ConfigBuilder(true);
                config.Build();
                return;
            }
            if (key.Key == ConsoleKey.D4 || key.Key == ConsoleKey.NumPad4) {
                ParseKoiHeader();
                return;
            }
        }
        public static void ParseKoiHeader() {
            Console.WriteLine("Path: ");
            string Path = Console.ReadLine().Replace("\"", "");
            Console.Clear();
            Console.WriteLine("Custom #Koi Stream Name (y/n)");
            string choice = Console.ReadLine();
            Console.Clear();
            KoiShit.VMData data;
            if (choice.ToLower() == "y") {
                Console.WriteLine("Koi Header Name:");
                string name = Console.ReadLine();
                Console.Clear();
                data = KoiHeader.Parse(Path, name);
            } else {
                data = KoiHeader.Parse(Path, null);
            }

            if (data != null) {
                Console.WriteLine("Strings Found: ");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                foreach (KeyValuePair<uint, string> keyValuePair in data.strings) {
                    try {
                        Console.WriteLine("[+] " + keyValuePair.Key + "|" + keyValuePair.Value);
                    } catch {
                    }
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("References Found: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                foreach (KeyValuePair<uint, KoiShit.VMData.RefInfo> keyValuePair2 in data.references) {
                    try {
                        Console.WriteLine("[+] " + keyValuePair2.Key + "|" + keyValuePair2.Value.Member.ToString());
                    } catch {
                    }
                }
                Console.WriteLine();
                /*   Console.WriteLine("Exports Found: ");
                   foreach (var str in data.exports) {
                       WriteLine(str.Value.CodeOffset);
                   }
                   Console.WriteLine();*/
            } else {
                Console.WriteLine("Found nothing :(");
            }
            Console.ReadLine();
        }
        public static void InjectDll() {
            Obfuscator obf = Obfuscator.Unknown;
            Console.WriteLine("Path: ");
            string Path = Console.ReadLine().Replace("\"", "");
            ModuleDefMD Module = ModuleDefMD.Load(Path);
            Console.Clear();
            Console.WriteLine("AutoDetect Obfuscator/VM (y/n): ");
            string choice = Console.ReadLine();
            Console.Clear();
            Injector inj = new Injector(Module, Path);
            bool injected;
            if (choice.ToLower() == "y") {
              injected =  inj.Inject(true);
            } else {
                injected = inj.Inject(false);
            }
            if (injected) {
                Console.WriteLine("Successfully Injected And Saved File!");
            } else {
                Console.WriteLine("Failed Injecting File.");
            }
            Console.ReadLine();


        }
    }
}
