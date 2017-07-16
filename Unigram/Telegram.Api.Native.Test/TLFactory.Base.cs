﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Telegram.Api.Native.TL;

namespace Telegram.Api.TL
{
    public static partial class TLFactory
    {
        public static T Read<T>(TLBinaryReader from)
        {
            if (typeof(T) == typeof(UInt32)) return (T)(Object)from.ReadUInt32();
            else if (typeof(T) == typeof(Int32)) return (T)(Object)from.ReadInt32();
            else if (typeof(T) == typeof(UInt64)) return (T)(Object)from.ReadUInt64();
            else if (typeof(T) == typeof(Int64)) return (T)(Object)from.ReadInt64();
            else if (typeof(T) == typeof(Double)) return (T)(Object)from.ReadDouble();
            else if (typeof(T) == typeof(Boolean)) return (T)(Object)from.ReadBoolean();
            else if (typeof(T) == typeof(String)) return (T)(Object)from.ReadString();
            else if (typeof(T) == typeof(Byte[])) return (T)(Object)from.ReadByteArray();

            var type = from.ReadUInt32();
            if ((TLType)type == TLType.Vector)
            {
                if (typeof(T) != typeof(object) && typeof(T) != typeof(TLObject))
                {
                    return (T)(Object)Activator.CreateInstance(typeof(T), from);
                }
                else
                {
                    var length = from.ReadUInt32();
                    if (length > 0)
                    {
                        var inner = from.ReadUInt32();
                        from.Position -= 8;

                        var innerType = Type.GetType($"Telegram.Api.TL.TL{(TLType)inner}");
                        if (innerType != null)
                        {
                            var baseType = innerType.GetTypeInfo().BaseType;
                            if (baseType.Name != "TLObject")
                            {
                                innerType = baseType;
                            }

                            var d1 = typeof(TLVector<>);
                            var typeArgs = new Type[] { innerType };
                            var makeme = d1.MakeGenericType(typeArgs);
                            return (T)(Object)Activator.CreateInstance(makeme, from);
                        }
                        else
                        {
                            // A base type collection (int, long, double, bool)
                            // TODO:
                            return (T)(Object)null;
                        }
                    }
                    else
                    {
                        // An empty collection, so we can't determine the generic type
                        // TODO:
                        return (T)(Object)new TLVectorEmpty();
                    }
                }
            }
            else if (type == 0x997275b5 || type == 0x3fedd339)
            {
                return (T)(Object)true;
            }
            else if (type == 0xbc799737)
            {
                return (T)(Object)false;
            }
            else
            {
                return Read<T>(from, (TLType)type);
            }
        }

        public static void Write(TLBinaryWriter to, object value)
        {
            if (value == null)
            {
                to.WriteUInt32(0x56730BCC);
                return;
            }

            var type = value.GetType();
            if (type == typeof(UInt32)) to.WriteUInt32((uint)value);
            else if (type == typeof(Int32)) to.WriteInt32((int)value);
            else if (type == typeof(UInt64)) to.WriteUInt64((ulong)value);
            else if (type == typeof(Int64)) to.WriteInt64((long)value);
            else if (type == typeof(Double)) to.WriteDouble((double)value);
            else if (type == typeof(Boolean)) to.WriteBoolean((bool)value);
            else if (type == typeof(String)) to.WriteString((string)value ?? string.Empty);
            else if (type == typeof(Byte[])) to.WriteByteArray((byte[])value);
            else ((TLObject)value).Write(to);
        }
    }
}