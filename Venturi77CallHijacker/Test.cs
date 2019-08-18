using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Venturi77CallHijacker.CallHijacker;

namespace Venturi77CallHijacker {
    static class Worker {
        static Worker() {
          for(int i = 0; i<Utils.Configuration.Functions.Methods.Count();i++) {
                methods.Add(i, Utils.Configuration.Functions.Methods[i]);
            }
            for (int i = 0; i < Utils.Configuration.Functions.MDToken.Count(); i++) {
                mdtokens.Add(i, Utils.Configuration.Functions.MDToken[i]);
            }
        }
        public static int LookupMethod(string name) {
            var shit = methods.Where(q => q.Value.MethodName.ToUpper() == name.ToUpper()).ToArray();
            if (shit.Count() > 0) {
                return shit[0].Key;
            }
            return 69420;
        }
        public static int LookupMDTOken(int mdtok) {
            var shit = mdtokens.Where(q => q.Value.MDTokenInt == mdtok).ToArray();
            if(shit.Count() >0) {
                return shit[0].Key;
            }
            return 69420;
        }
        private static readonly Dictionary<int, Method> methods = new Dictionary<int, Method>();
        private static readonly Dictionary<int, MDToken> mdtokens = new Dictionary<int, MDToken>();
    }
}
