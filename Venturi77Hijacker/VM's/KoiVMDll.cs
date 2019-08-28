using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.MD;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
namespace Venturi77Hijacker.VM_s {
    public class KoiVMDll : VM {
        public static ModuleDefMD Module;
        public static string path;
        public Obfuscator GetObfuscator() {
            return Obfuscator.KoiVMDll;
        }

        public bool Inject(ModuleDefMD _Module, string _Path) {
            Module = _Module;
            path = _Path;
            bool injected = false;
            TypeDef[] array = Module.Types.ToArray<TypeDef>();
            for (int i = 0; i < array.Length; i++) {
                foreach (MethodDef method in array[i].Methods.ToArray<MethodDef>()) {
                    bool flag = method.HasBody && method.Body.HasInstructions && !method.FullName.Contains("My.") && !method.FullName.Contains(".My") && !method.IsConstructor && !method.DeclaringType.IsGlobalModuleType;
                    if (flag) {
                        for (int j = 0; j < method.Body.Instructions.Count - 1; j++) {
                            if (method.Body.Instructions[j].OpCode == OpCodes.Ldarg_2) {
                                if (method.Body.Instructions[j + 1].OpCode == OpCodes.Ldloc_2) {
                                    if (method.Body.Instructions[j + 2].OpCode == OpCodes.Ldloc_3) {
                                        if (method.Body.Instructions[j + 3].OpCode == OpCodes.Callvirt) {
                                            if (method.Body.Instructions[j + 4].OpCode == OpCodes.Stloc_S) {
                                                method.Body.Instructions[j + 3].OpCode = OpCodes.Call;
                                                Importer importer = new Importer(Module);
                                                IMethod Method;
                                                Method = importer.Import(typeof(Venturi77CallHijacker.Handler).GetMethod("HandleInvoke"));
                                                method.Body.Instructions[j + 3].Operand = Module.Import(Method);
                                                method.Body.Instructions[j + 3].OpCode = OpCodes.Call;
                                                injected = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            return injected;
        }

        public void Save() {
            ModuleWriterOptions nativeModuleWriterOptions = new ModuleWriterOptions(Module);
            nativeModuleWriterOptions.MetadataOptions.Flags = MetadataFlags.KeepOldMaxStack;
            nativeModuleWriterOptions.Logger = DummyLogger.NoThrowInstance;
            nativeModuleWriterOptions.MetadataOptions.Flags = MetadataFlags.PreserveAll;
            nativeModuleWriterOptions.Cor20HeaderOptions.Flags = new ComImageFlags?(ComImageFlags.ILOnly);
            var otherstrteams = Module.Metadata.AllStreams.Where(a => a.GetType() == typeof(DotNetStream));
            nativeModuleWriterOptions.MetadataOptions.PreserveHeapOrder(Module, addCustomHeaps: true);
            Module.Write(Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + "_Inj" + ".dll"), nativeModuleWriterOptions);
            File.Copy("Venturi77CallHijacker.dll", Path.GetDirectoryName(path) + "\\Venturi77CallHijacker.dll");
            File.Copy("Newtonsoft.Json.dll", Path.GetDirectoryName(path) + "\\Newtonsoft.Json.dll");
        }
    }
}
