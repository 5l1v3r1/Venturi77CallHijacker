using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class AgileVM : VM {
        public static ModuleDefMD Module;
        public static string path;
        public Obfuscator GetObfuscator() {
            return Obfuscator.AgileVM;
        }

        public bool Inject(ModuleDefMD _Module,string _Path) {
            Module = _Module;
            path = _Path;
            bool injected = false;
            TypeDef[] array = Module.Types.ToArray<TypeDef>();
            for (int i = 0; i < array.Length; i++) {
                foreach (MethodDef method in array[i].Methods.ToArray<MethodDef>()) {
                    if (method.HasBody && method.Body.HasInstructions) {
                        for (int d = 0; d < method.Body.Instructions.Count(); d++) {

                            if (method.Body.Instructions[d].OpCode == OpCodes.Callvirt) {
                                if (method.Body.Instructions[d].ToString().Contains("System.Reflection.MethodBase::Invoke(System.Object,System.Object[])") && method.Body.Instructions[d + 1].IsStloc() && method.Body.Instructions[d - 1].IsLdarg() && method.Body.Instructions[d - 3].IsLdarg()) {
                                    method.Body.Instructions[d].Operand = method.Module.Import(ModuleDefMD.Load("Venturi77CallHijacker.dll").Types.Single(t => t.IsPublic && t.Name == "Handler").Methods.Single(m => m.Name == "HandleInvoke"));
                                    method.Body.Instructions[d].OpCode = OpCodes.Call;
                                    injected = true;
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
