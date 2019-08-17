using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Venturi77CallHijacker.CallHijacker;

namespace Venturi77CallHijacker {
    internal class Utils {
       
        public static object SearchMethodByMethodName(Method method,MethodBase methodd,out object[] Parameters, object[] paramss) {
            Parameters = paramss;
            if(method.MethodName.ToUpper() == methodd.Name.ToUpper()) {
                if(method.Param != null) {
                    foreach(var param in method.Param) {
                        Parameters[param.ParameterIndex] = param.ReplaceWith;
                    }
                }
                if(method.ReplaceResultWith != null) {
                    return method.ReplaceResultWith;
                }
            }
            return "Rip";
        }
        public static object SearchMethodByMDToken(MDToken MDToken, MethodBase methodd, out object[] Parameters, object[] paramss) {
            Parameters = paramss;
            if (MDToken.MDTokenInt == methodd.MetadataToken) {
                if (MDToken.Param != null) {
                    foreach (var param in MDToken.Param) {
                        Parameters[param.ParameterIndex] = param.ReplaceWith;
                    }
                }
                if (MDToken.ReplaceResultWith != null) {
                    return MDToken.ReplaceResultWith;
                }
            }
            return "Rip";
        }


        public static CallHijacker.Config Configuration = JsonConvert.DeserializeObject<CallHijacker.Config>(File.ReadAllText("Config.Json"));
    }
}
