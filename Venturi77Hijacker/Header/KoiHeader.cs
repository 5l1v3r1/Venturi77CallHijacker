using dnlib.DotNet;
using dnlib.DotNet.MD;
using dnlib.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Venturi77Hijacker.KoiShit;

namespace Venturi77Hijacker {
     class KoiHeader {
        public static VMData Parse(string _path,string headername) {
            ModuleDefMD Module = ModuleDefMD.Load(_path);
            Assembly asm = Assembly.LoadFile(_path);
            asm.CreateInstance("g");
            foreach (DotNetStream stream in Module.Metadata.AllStreams) {
                if (headername != null) {
                    bool flag = stream.Name == headername;
                    if (flag) {
                        DataReader read = stream.CreateReader();
                        byte[] bytes = read.ToArray();
                        return KoiShit.DoShit(bytes, asm);
                    }
                } else {
                    bool flag = stream.Name == "#Koi" || stream.Name == "<<EMPTY_NAME>>" || stream.Name == "#Bed";
                    if (flag) {
                        DataReader read = stream.CreateReader();
                        byte[] bytes = read.ToArray();
                        return KoiShit.DoShit(bytes, asm);
                    }
                }
            }
            return null;
        }

    }
}
