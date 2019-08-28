using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    class ConfigBuilder {
        public bool debugging;
        public ConfigBuilder(bool debug) {
            debugging = debug;
        }
        public void Build() {
            if (!debugging) {


                CallHijacker.Config Config = new CallHijacker.Config();
                Config.Debug = false;
                Config.Functions = new CallHijacker.Function();
                Config.Functions.MDToken = new List<CallHijacker.MDToken>();
                Config.Functions.Methods = new List<Method>();
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Config.Functions = CreateFunction(Config.Functions);
                SaveConfig(Config);
                Console.WriteLine("Saved!");
                Console.ReadLine();
                Console.Clear();
            } else {
                CallHijacker.Config Config = new CallHijacker.Config();
                Config.Debug = true;
                File.AppendAllText("Config.Json", JsonConvert.SerializeObject(Config, Formatting.Indented, new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore
                }));
            }
        }
        public void SaveConfig(Config config) {
            File.AppendAllText("Config.Json", JsonConvert.SerializeObject(config, Formatting.Indented, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            }));
        }
        public  CallHijacker.Parameter GenerateParam() {
            CallHijacker.Parameter param = new CallHijacker.Parameter();
            Console.WriteLine("Parameter Index:");
            param.ParameterIndex = Convert.ToInt32(WriteInput());
            Console.Clear();
            Console.WriteLine("Replace Parameter Value With: (*null* if do nothing)");
            string input2 = WriteInput();
            param.ReplaceWith = ParseString(input2);
            Console.Clear();
            return param;
        }
        public Function CreateFunction(CallHijacker.Function func) {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Search By: " + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[1] MethodName" + Environment.NewLine + "[2] MDToken" + Environment.NewLine);
            int parse3 = Convert.ToInt32(WriteInput());
            if (parse3 == 1) {
                Method method = new Method();
                Console.WriteLine("Method Name:");
                method.MethodName = WriteInput();
                Console.Clear();
                Console.WriteLine("Replace Result With: (*null* if do nothing)");
                method.ReplaceResultWith = ParseString(WriteInput());
                Console.Clear();
                Console.WriteLine("[1] Add Edit Parameter\n[2]Continue");
                if (Convert.ToInt32(WriteInput()) == 1) {
                    method.Param = new List<CallHijacker.Parameter>();
                    for (; ; )
                        {
                        method.Param.Add(GenerateParam());
                        Console.WriteLine("[1] Add Edit Parameter\n[2]Continue");
                        if (Convert.ToInt32(WriteInput()) != 1)
                            break;
                    }
                }
                Console.Clear();
                func.Methods.Add(method);
                Console.WriteLine("[1] Continue\n[2] Save");
                int parse2 = Convert.ToInt32(WriteInput());
                if (parse2 == 1) {
                    CreateFunction(func);
                } else {
                    return func;
                }
            } else {
                if (parse3 == 3) {


                } else {
                    if (parse3 == 2) {
                        CallHijacker.MDToken MDTok = new CallHijacker.MDToken();
                        Console.WriteLine("MDToken:");
                        MDTok.MDTokenInt = Convert.ToInt32(WriteInput());
                        Console.Clear();
                        Console.WriteLine("Replace Result With: (*null* if do nothing)");
                        string input = WriteInput();
                        MDTok.ReplaceResultWith = ParseString(input);
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("[1] Add Edit Parameter\n[2]Continue");
                        if (Convert.ToInt32(WriteInput()) == 1) {
                            MDTok.Param = new List<CallHijacker.Parameter>();
                            for (; ; )
                                {
                                MDTok.Param.Add(GenerateParam());
                                Console.WriteLine("[1] Add Edit Parameter\n[2] Continue");
                                if (Convert.ToInt32(WriteInput()) != 1)
                                    break;
                            }
                        }
                        func.MDToken.Add(MDTok);
                        Console.Clear();
                        Console.WriteLine("[1] Continue\n[2] Save");
                        int parse2 = Convert.ToInt32(WriteInput());
                        if (parse2 == 1) {
                            CreateFunction(func);
                        } else {
                            return func;
                        }
                    }
                }
            }



            return func;

        }
        public  string WriteInput() {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Input: ");
            string input = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Clear();
            return input;
        }
        public  int WriteShit() {
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("[1] New Function");
            Console.WriteLine("[2] Save");
            Console.ForegroundColor = ConsoleColor.Cyan;

            int parse2 = Convert.ToInt32(WriteInput());
            Console.Clear();
            return parse2;
        }
        public  object ParseString(string input) {
            if (input == "*null*")
                return null;
            if (input == "*true*")
                return true;
            if (input == "*false*")
                return false;
            return input;

        }
    }
}
