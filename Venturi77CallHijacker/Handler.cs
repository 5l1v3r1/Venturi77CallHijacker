using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;
namespace Venturi77CallHijacker {
    public class Handler {
        // Token: 0x06000002 RID: 2 RVA: 0x0000205C File Offset: 0x0000025C
        public static object HandleInvoke(MethodBase method, object obj2, object[] Parameters) {
            try {
                if(Utils.Configuration.Debug) {
                    object objj = null;
                    try {
                        objj = method.Invoke(obj2, Parameters);
                    } catch {
                        objj = null;
                    }
                    string debug = "Calling Method: " + method.ToString() + "\nMethodName: " + method.Name + "\nParameters: \n";
                   for(int i = 0; i<Parameters.Length; i++) {
                        debug += "Param[" + i + "]" + " Value: " + Parameters[i].ToString()+ "\n";
                    }
                    debug += "MDToken: " + method.MetadataToken+"\n";
                    System.IO.File.AppendAllText("Debug.txt", debug + Environment.NewLine +"------------------------" + Environment.NewLine);
                    return objj;
                }
                foreach(var methodd in Utils.Configuration.Functions.Methods) {

                    object shit = Utils.SearchMethodByMethodName(methodd, method, out Parameters,Parameters);
                    bool flag = shit is string;
                    if (!flag) {
                        return shit;
                    }
                    bool flag2 = shit.ToString() == "Rip";
                    if (flag2) {
                        return method.Invoke(obj2, Parameters);
                    }
                    return shit;
                }
                foreach(var mdtok in Utils.Configuration.Functions.MDToken) {
                    object shit = Utils.SearchMethodByMDToken(mdtok, method, out Parameters, Parameters);
                    bool flag = shit is string;
                    if (!flag) {
                        return shit;
                    }
                    bool flag2 = shit.ToString() == "Rip";
                    if (flag2) {
                        return method.Invoke(obj2, Parameters);
                    } else {
                        return shit;
                    }
                }
            } catch {
                return null;
            }
            return null;
        }
    }
}
