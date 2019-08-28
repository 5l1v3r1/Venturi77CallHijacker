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
    public class EazVM : VM {
        public static ModuleDefMD Module;
        public static string path;
        public Obfuscator GetObfuscator() {
            // throw new NotImplementedException();
            return Obfuscator.EazVM;
        }

        public bool Inject(ModuleDefMD _Module, string _Path) {
            Module = _Module;
            path = _Path;
            bool injected = false;
            TypeDef[] array = Module.Types.ToArray<TypeDef>();
            for (int i = 0; i < array.Length; i++) {
                foreach (MethodDef method in array[i].Methods.ToArray<MethodDef>()) {
                    bool flag = method.HasBody && method.Body.HasInstructions && method.Body.Instructions.Count() >= 15 && !method.FullName.Contains("My.") && !method.FullName.Contains(".My") && !method.IsConstructor && !method.DeclaringType.IsGlobalModuleType;
                    if (flag) {
                        for (int j = 0; j < method.Body.Instructions.Count - 1; j++) {
                            if (method.Body.Instructions[j].OpCode == OpCodes.Callvirt) {
                                string operand = method.Body.Instructions[j].Operand.ToString();
                                if (operand.Contains("System.Object System.Reflection.MethodBase::Invoke(System.Object,System.Object[])") && method.Body.Instructions[j - 1].IsLdarg() && method.Body.Instructions[j - 2].IsLdarg() && method.Body.Instructions[j - 3].IsLdarg()) {
                                    Importer importer = new Importer(Module);
                                    IMethod myMethod;
                                    myMethod = importer.Import(typeof(Venturi77CallHijacker.Handler).GetMethod("HandleInvoke"));
                                    method.Body.Instructions[j].Operand = Module.Import(myMethod);
                                    method.Body.Instructions[j].OpCode = OpCodes.Call;
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
            ModuleWriterOptions moduleWriterOptions = new ModuleWriterOptions(Module);
            moduleWriterOptions.MetadataOptions.Flags = MetadataFlags.KeepOldMaxStack;
            moduleWriterOptions.Logger = DummyLogger.NoThrowInstance;
            moduleWriterOptions.MetadataOptions.Flags = MetadataFlags.PreserveAll;
            moduleWriterOptions.MetadataOptions.PreserveHeapOrder(Module, true);
            moduleWriterOptions.Cor20HeaderOptions.Flags = new ComImageFlags?(ComImageFlags.ILOnly | ComImageFlags.Bit32Required);
            Module.Write(path + "_Injected.exe", moduleWriterOptions);
            File.Copy("Venturi77CallHijacker.dll", Path.GetDirectoryName(path) + "\\Venturi77CallHijacker.dll");
            File.Copy("Newtonsoft.Json.dll", Path.GetDirectoryName(path) + "\\Newtonsoft.Json.dll");
        }
    }
}
