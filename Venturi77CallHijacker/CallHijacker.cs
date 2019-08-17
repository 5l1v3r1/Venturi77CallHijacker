using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venturi77CallHijacker {
    internal class CallHijacker {
        internal class Config {
            public bool Debug { get; set; }
            //  public uint GetMethodByMDToken { get; set; }
            public Function Functions { get; set; }

        }
        internal class Function {
            public List<MDToken> MDToken { get; set; }
            public List<Method> Methods { get; set; }
            //   public List<Parameter> Parameters { get; set; }
          //  public CallHijacker.Function ContinueAdvanced { get; set; }
        }
        internal class Parameter {
            public int ParameterIndex { get; set; }
            public object ReplaceWith { get; set; }
            // public string ReplaceResultWith { get; set; }
        }
        internal class Method {
            public string MethodName { get; set; }
            public List<Parameter> Param { get; set; }
            public object ReplaceResultWith { get; set; }
        }
        internal class MDToken {
            public int MDTokenInt { get; set; }
            public List<Parameter> Param { get; set; }
            public object ReplaceResultWith { get; set; }
        }
    }
}
