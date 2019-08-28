using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venturi77Hijacker {
    public interface VM {
        Obfuscator GetObfuscator();
        bool Inject(ModuleDefMD Module,string Path);
        void Save();
    }
}
