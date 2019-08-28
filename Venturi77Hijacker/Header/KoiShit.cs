using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using dnlib.DotNet;
using dnlib.DotNet.MD;
using dnlib.IO;

namespace Venturi77Hijacker {
    class KoiShit {
        public unsafe static VMData DoShit(byte[] stream, Assembly asm) {
            byte[] message = stream;
            IntPtr memIntPtr = Marshal.AllocHGlobal(message.Length);
            byte* memBytePtr = (byte*)memIntPtr.ToPointer();
            UnmanagedMemoryStream writeStream = new UnmanagedMemoryStream(memBytePtr, message.Length, message.Length, FileAccess.Write);
            writeStream.Write(message, 0, message.Length);
            writeStream.Close();
            UnmanagedMemoryStream readStream = new UnmanagedMemoryStream(memBytePtr, message.Length, message.Length, FileAccess.Read);
            var ptr = AllocateKoi((void*)readStream.PositionPointer, (uint)readStream.Length);
            return new VMData(asm.GetModules()[0], ptr);
        }
        [DllImport("kernel32.dll")]
        private unsafe static extern void CopyMemory(void* dest, void* src, uint count);

        // Token: 0x060001D8 RID: 472 RVA: 0x0000C72C File Offset: 0x0000A92C
        private unsafe static void* AllocateKoi(void* ptr, uint len) {
            void* ptr2 = (void*)Marshal.AllocHGlobal((int)len);
            CopyMemory(ptr2, ptr, len);
            return ptr2;
        }
        internal class VMData {
            // Token: 0x060001C3 RID: 451 RVA: 0x0000BE2C File Offset: 0x0000A02C
            public unsafe VMData(Module module, void* data) {
                bool flag = ((VMData.VMDAT_HEADER*)data)->MAGIC != 1752394086u;
                if (flag) {
                    throw new InvalidProgramException();
                }
                this.references = new Dictionary<uint, VMData.RefInfo>();
                this.strings = new Dictionary<uint, string>();
                this.exports = new Dictionary<uint, VMExportInfo>();
                byte* ptr = (byte*)data + sizeof(VMData.VMDAT_HEADER);
                int num = 0;
                while ((long)num < (long)((ulong)((VMData.VMDAT_HEADER*)data)->MD_COUNT)) {
                    uint key = Utils.ReadCompressedUInt(ref ptr);
                    int token = (int)Utils.FromCodedToken(Utils.ReadCompressedUInt(ref ptr));
                    this.references[key] = new VMData.RefInfo {
                        module = module,
                        token = token
                    };
                    num++;
                }
                int num2 = 0;
                while ((long)num2 < (long)((ulong)((VMData.VMDAT_HEADER*)data)->STR_COUNT)) {
                    uint key2 = Utils.ReadCompressedUInt(ref ptr);
                    uint num3 = Utils.ReadCompressedUInt(ref ptr);
                    this.strings[key2] = new string((char*)ptr, 0, (int)num3);
                    ptr += num3 << 1;
                    num2++;
                }
                int num4 = 0;
                while ((long)num4 < (long)((ulong)((VMData.VMDAT_HEADER*)data)->EXP_COUNT)) {
                    this.exports[Utils.ReadCompressedUInt(ref ptr)] = new VMExportInfo(ref ptr, module);
                    num4++;
                }
                this.KoiSection = (byte*)data;
                this.Module = module;
                VMData.moduleVMData[module] = this;
            }

            // Token: 0x17000061 RID: 97
            // (get) Token: 0x060001C4 RID: 452 RVA: 0x00002574 File Offset: 0x00000774
            public Module Module { get; }

            // Token: 0x17000062 RID: 98
            // (get) Token: 0x060001C5 RID: 453 RVA: 0x0000257C File Offset: 0x0000077C
            // (set) Token: 0x060001C6 RID: 454 RVA: 0x00002584 File Offset: 0x00000784
            public unsafe byte* KoiSection { get; set; }

            // Token: 0x060001C7 RID: 455 RVA: 0x0000BF80 File Offset: 0x0000A180


            // Token: 0x060001C8 RID: 456 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
            public MemberInfo LookupReference(uint id) {

                return this.references[id].Member;

            }

            // Token: 0x060001C9 RID: 457 RVA: 0x0000C00C File Offset: 0x0000A20C
            public string LookupString(uint id) {
                bool flag = id == 0u;
                string result;
                if (flag) {
                    result = null;
                } else {
                    result = this.strings[id];
                }
                return result;
            }

            // Token: 0x060001CA RID: 458 RVA: 0x0000C038 File Offset: 0x0000A238
            public VMExportInfo LookupExport(uint id) {
                return this.exports[id];
            }

            // Token: 0x040000D1 RID: 209
            private static readonly Dictionary<Module, VMData> moduleVMData = new Dictionary<Module, VMData>();

            // Token: 0x040000D2 RID: 210
            public readonly Dictionary<uint, VMExportInfo> exports;

            // Token: 0x040000D3 RID: 211
            public readonly Dictionary<uint, VMData.RefInfo> references;

            // Token: 0x040000D4 RID: 212
            public readonly Dictionary<uint, string> strings;

            // Token: 0x02000086 RID: 134
            private struct VMDAT_HEADER {
                // Token: 0x040000D7 RID: 215
                public readonly uint MAGIC;

                // Token: 0x040000D8 RID: 216
                public readonly uint MD_COUNT;

                // Token: 0x040000D9 RID: 217
                public readonly uint STR_COUNT;

                // Token: 0x040000DA RID: 218
                public readonly uint EXP_COUNT;
            }

            // Token: 0x02000087 RID: 135
            public class RefInfo {
                // Token: 0x17000063 RID: 99
                // (get) Token: 0x060001CC RID: 460 RVA: 0x0000C058 File Offset: 0x0000A258
                public MemberInfo Member {
                    get {
                        MemberInfo result;
                        if ((result = this.resolved) == null) {
                            result = (this.resolved = this.module.ResolveMember(this.token));
                        }
                        return result;
                    }
                }

                // Token: 0x040000DB RID: 219
                public Module module;

                // Token: 0x040000DC RID: 220
                public MemberInfo resolved;

                // Token: 0x040000DD RID: 221
                public int token;
            }
        }
        internal struct VMExportInfo {
            // Token: 0x060001CE RID: 462 RVA: 0x0000C08C File Offset: 0x0000A28C
            public unsafe VMExportInfo(ref byte* ptr, Module module) {
                this.CodeOffset = *ptr;
                ptr += 4;
                bool flag = this.CodeOffset > 0u;
                if (flag) {
                    this.EntryKey = *ptr;
                    ptr += 4;
                } else {
                    this.EntryKey = 0u;
                }
                this.Signature = new VMFuncSig(ref ptr, module);
            }

            // Token: 0x040000DE RID: 222
            public readonly uint CodeOffset;

            // Token: 0x040000DF RID: 223
            public readonly uint EntryKey;

            // Token: 0x040000E0 RID: 224
            public readonly VMFuncSig Signature;
        }
        internal class VMFuncSig {
            // Token: 0x060001D1 RID: 465 RVA: 0x0000C188 File Offset: 0x0000A388
            public unsafe VMFuncSig(ref byte* ptr, Module module) {
                this.module = module;
                byte* ptr2 = ptr;
                ptr = ptr2 + 1;
                this.Flags = *ptr2;
                this.paramToks = new int[Utils.ReadCompressedUInt(ref ptr)];
                for (int i = 0; i < this.paramToks.Length; i++) {
                    this.paramToks[i] = (int)Utils.FromCodedToken(Utils.ReadCompressedUInt(ref ptr));
                }
                this.retTok = (int)Utils.FromCodedToken(Utils.ReadCompressedUInt(ref ptr));
            }

            // Token: 0x17000064 RID: 100
            // (get) Token: 0x060001D2 RID: 466 RVA: 0x0000C200 File Offset: 0x0000A400
            public Type[] ParamTypes {
                get {
                    bool flag = this.paramTypes != null;
                    Type[] result;
                    if (flag) {
                        result = this.paramTypes;
                    } else {
                        Type[] array = new Type[this.paramToks.Length];
                        for (int i = 0; i < array.Length; i++) {
                            array[i] = this.module.ResolveType(this.paramToks[i]);
                        }
                        this.paramTypes = array;
                        result = array;
                    }
                    return result;
                }
            }

            // Token: 0x17000065 RID: 101
            // (get) Token: 0x060001D3 RID: 467 RVA: 0x0000C268 File Offset: 0x0000A468
            public Type RetType {
                get {
                    Type result;
                    if ((result = this.retType) == null) {
                        result = (this.retType = this.module.ResolveType(this.retTok));
                    }
                    return result;
                }
            }

            // Token: 0x040000E2 RID: 226
            private readonly int[] paramToks;

            // Token: 0x040000E3 RID: 227
            private readonly int retTok;

            // Token: 0x040000E4 RID: 228
            public byte Flags;

            // Token: 0x040000E5 RID: 229
            private readonly Module module;

            // Token: 0x040000E6 RID: 230
            private Type[] paramTypes;

            // Token: 0x040000E7 RID: 231
            private Type retType;
        }
        internal static class Utils {
            // Token: 0x06000008 RID: 8 RVA: 0x0000259C File Offset: 0x0000079C
            public unsafe static uint ReadCompressedUInt(ref byte* ptr) {
                uint num = 0u;
                int num2 = 0;
                byte* ptr2;
                do {
                    num |= (uint)((uint)(*ptr & 127) << num2);
                    num2 += 7;
                    ptr2 = ptr;
                    ptr = ptr2 + 1;
                }
                while ((*ptr2 & 128) > 0);
                return num;
            }

            // Token: 0x06000009 RID: 9 RVA: 0x000025E0 File Offset: 0x000007E0
            public static uint FromCodedToken(uint codedToken) {
                uint num = codedToken >> 3;
                uint result;
                switch (codedToken & 7u) {
                    case 1u:
                        result = (num | 33554432u);
                        break;
                    case 2u:
                        result = (num | 16777216u);
                        break;
                    case 3u:
                        result = (num | 452984832u);
                        break;
                    case 4u:
                        result = (num | 167772160u);
                        break;
                    case 5u:
                        result = (num | 100663296u);
                        break;
                    case 6u:
                        result = (num | 67108864u);
                        break;
                    case 7u:
                        result = (num | 721420288u);
                        break;
                    default:
                        result = num;
                        break;
                }
                return result;
            }

            // Token: 0x0600000A RID: 10 RVA: 0x00002668 File Offset: 0x00000868

        }
    }
}
