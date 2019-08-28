using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venturi77Hijacker {
    public class Injector {
        public static ModuleDefMD Module;
        public static string _Path;
        public static Dictionary<Obfuscator, VM> Injectors = new Dictionary<Obfuscator, VM>();
        public Injector(ModuleDefMD _module, string _path) {
            foreach (Type type in typeof(Injector).Assembly.GetTypes()) {
                bool flag = typeof(VM).IsAssignableFrom(type) && !type.IsAbstract;
                if (flag) {
                    VM opCode = (VM)Activator.CreateInstance(type);
                    Injectors[opCode.GetObfuscator()] = opCode;
                }
            }
            Module = _module;
            _Path = _path;
        }
        public bool Inject(bool detect) {
            Obfuscator obf;
            if (detect) {
                obf = DetectObf();
                Console.Clear();
            } else {
                Console.WriteLine("[1] KoiVM\n[2] EazVM\n[3] AgileVM\n[4] KoiVM Dll");
                obf = (Obfuscator)(Convert.ToInt32(Console.ReadLine()) - 1);
                Console.Clear();
            }
            Console.WriteLine("Injecting " + obf.ToString() + " VM");
            if (obf == Obfuscator.Unknown) {
                Console.WriteLine("Unknown Obfuscator!");
                return false;
            }
            if (obf != Obfuscator.AgileVM && obf != Obfuscator.KoiVMDll) {
                bool injected = Injectors[obf].Inject(Module, _Path);
                if (injected) {
                    Injectors[obf].Save();
                } else {
                    return false;
                }
                return true;
            } else {
                if (obf == Obfuscator.AgileVM) {
                    string pathh = Path.Combine(Path.GetDirectoryName(_Path), "AgileDotNet.VMRuntime.dll");
                    ModuleDefMD Module = ModuleDefMD.Load(pathh);
                    bool injected = Injectors[obf].Inject(Module, pathh);
                    if (injected) {
                        Injectors[obf].Save();
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    string pathh = Path.Combine(Path.GetDirectoryName(_Path), "KoiVM.Runtime.dll");
                    ModuleDefMD Module = ModuleDefMD.Load(pathh);
                    bool injected = Injectors[obf].Inject(Module, pathh);
                    if (injected) {
                        Injectors[obf].Save();
                        return true;
                    } else {
                        return false;
                    }
                }
            }

        }
        public Obfuscator DetectObf() {
            if (Module.GetAssemblyRefs().Where(q => q.ToString().Contains("AgileDotNet.VMRuntime")).ToArray().Count() > 0) {
                return Obfuscator.AgileVM;
            }
            if (Module.Types.SingleOrDefault(t => t.HasFields && t.Fields.Count == 119) != null) {
                return Obfuscator.KoiVM;
            }
            if (Module.GetAssemblyRefs().Where(q => q.ToString().Contains("KoiVM.Runtime")).ToArray().Count() > 0) {
                return Obfuscator.KoiVMDll;
            }
            foreach (var type in Module.Types) {
                foreach (var method in type.Methods) {
                    if (method.HasBody && method.Body.HasInstructions && method.Body.Instructions.Count() >= 6) {
                        if (method.Body.Instructions[0].IsLdarg()) {
                            if (method.Body.Instructions[1].IsLdarg()) {
                                if (method.Body.Instructions[2].IsLdarg()) {
                                    if (method.Body.Instructions[3].IsLdarg()) {
                                        if (method.Body.Instructions[5].OpCode == OpCodes.Pop) {
                                            if (method.Body.Instructions[6].OpCode == OpCodes.Ret) {
                                                if (method.Body.Instructions[4].OpCode == OpCodes.Call) {

                                                    if (method.Body.Instructions[4].ToString().Contains("(System.IO.Stream,System.String,System.Object[])")) {
                                                        return Obfuscator.EazVM;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Obfuscator.Unknown;
        }

    }
}
