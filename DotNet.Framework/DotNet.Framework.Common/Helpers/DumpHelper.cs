using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Helpers
{
    [Flags]
    public enum ObjectDumpOptions
    {
        Fields = 0x00000001,
        Properties = 0x00000002,

        NonPublic = 0x00000010,
        Static = 0x00000020,

        Recurisive = 0x00001000,
        ExpandArray = 0x00002000,

        WithType = 0x00010000,
        UsingFullTypeName = 0x00040000,
        UsingTypeKeywords = 0x00080000,
        WithName = 0x00100000,

        SingleLine = 0x01000000,
        NullIsNull = 0x02000000,

        Default = Fields | Recurisive | UsingTypeKeywords | WithName | WithType | ExpandArray,
        DefaultBrief = Fields | Recurisive | UsingTypeKeywords | WithType | WithName | SingleLine,
        DefaultBriefMultiLines = Fields | Recurisive | UsingTypeKeywords | WithType | WithName
    }

    public static class DumpHelper
    {
        public static string DumpObject(object obj, ObjectDumpOptions options)
        {
            return DumpObject(obj, options, 0);
        }

        public static string DumpObject(object obj, ObjectDumpOptions options, int indent)
        {
            StringBuilder sb = new StringBuilder();

            if (obj == null)
            {
                if ((options & ObjectDumpOptions.NullIsNull) != 0)
                {
                    return null;
                }
                else
                {
                    return "<null>";
                }
            }
            else
            {
                Type type = obj.GetType();

                if ((options & ObjectDumpOptions.WithType) != 0)
                {
                    sb.Append('(');

                    if ((options & ObjectDumpOptions.UsingTypeKeywords) != 0)
                    {
                        if (type.IsArray)
                        {
                            sb.Append("array ");
                        }

                        if (type.IsClass && type != typeof(string))
                        {
                            if (type.IsSubclassOf(typeof(Delegate)))
                            {
                                sb.Append("delegate ");
                            }
                            else if (type.IsValueType)
                            {
                                sb.Append("struct ");
                            }
                            else
                            {
                                sb.Append("class ");
                            }
                        }

                        if (type.IsAnsiClass || type.IsUnicodeClass || type.IsAutoClass)
                        {
                            if (type.IsValueType && !type.IsPrimitive && !type.IsEnum)
                            {
                                sb.Append("struct ");
                            }
                        }

                        if (type.IsEnum)
                        {
                            sb.Append("enum ");
                        }

                        if (type.IsInterface)
                        {
                            sb.Append("interface ");
                        }
                    }

                    if ((options & ObjectDumpOptions.UsingFullTypeName) != 0)
                    {
                        sb.Append(ConvertToTypeKeywords(type, true));
                    }
                    else
                    {
                        sb.Append(ConvertToTypeKeywords(type, false));
                    }

                    sb.Append(')');
                }

                if (type.IsPrimitive)
                {
                    if (type == typeof(char))
                    {
                        sb.Append('\'' + obj.ToString() + '\'');
                    }
                    else
                    {
                        sb.Append(obj.ToString());
                    }
                }
                else if (type.IsEnum)
                {
                    sb.Append(obj.ToString());
                }
                else if (type == typeof(string))
                {
                    sb.Append('"' + obj.ToString() + '"');
                }
                else if (type.IsSubclassOf(typeof(Delegate)))
                {
                    Delegate d = (Delegate)obj;

                    if ((options & ObjectDumpOptions.UsingFullTypeName) != 0)
                    {
                        sb.Append(d.Method.DeclaringType.FullName + '.' + d.Method.Name);
                    }
                    else
                    {
                        sb.Append(d.Method.DeclaringType.Name + '.' + d.Method.Name);
                    }
                }
                else if (type.IsArray)
                {
                    Array array = (Array)obj;

                    if ((options & ObjectDumpOptions.ExpandArray) != 0)
                    {
                        sb.Append('[');
                        if ((options & ObjectDumpOptions.SingleLine) == 0)
                        {
                            Indent(ref indent, sb);
                        }

                        bool first = true;

                        for (int i = array.GetLowerBound(0); i < array.GetUpperBound(0); ++i)
                        {
                            if (!first)
                            {
                                sb.Append(", ");

                                if ((options & ObjectDumpOptions.SingleLine) == 0)
                                {
                                    NewLine(indent, sb);
                                }
                            }

                            sb.AppendFormat("array[{0}] = ", i);
                            sb.Append(DumpObject(array.GetValue(i), GetRecurisiveOptions(options), indent));
                            first = false;
                        }

                        if ((options & ObjectDumpOptions.SingleLine) == 0)
                        {
                            Unindent(ref indent, sb);
                        }
                        sb.Append(']');
                    }
                    else
                    {
                        sb.AppendFormat("<{0} elements>", array.Length);
                    }
                }
                else
                {
                    sb.Append('[');
                    if ((options & ObjectDumpOptions.SingleLine) == 0)
                    {
                        Indent(ref indent, sb);
                    }

                    bool isFirst = true;

                    if ((options & ObjectDumpOptions.Fields) != 0)
                    {
                        foreach (FieldInfo field in type.GetFields(GetBindingFlags(options)))
                        {
                            if (!isFirst)
                            {
                                sb.Append(", ");

                                if ((options & ObjectDumpOptions.SingleLine) == 0)
                                {
                                    NewLine(indent, sb);
                                }
                            }

                            if ((options & ObjectDumpOptions.WithName) != 0)
                            {
                                sb.Append(field.Name);
                                sb.Append(" = ");
                            }

                            sb.Append(DumpObject(field.GetValue(obj), GetRecurisiveOptions(options), indent));
                            isFirst = false;
                        }
                    }

                    if ((options & ObjectDumpOptions.Properties) != 0)
                    {
                        foreach (PropertyInfo property in type.GetProperties(GetBindingFlags(options)))
                        {
                            if (!isFirst)
                            {
                                sb.Append(", ");

                                if ((options & ObjectDumpOptions.SingleLine) == 0)
                                {
                                    NewLine(indent, sb);
                                }
                            }

                            if ((options & ObjectDumpOptions.WithName) != 0)
                            {
                                sb.Append(property.Name);
                                sb.Append(" = ");
                            }

                            ParameterInfo[] parameters = property.GetIndexParameters();

                            if (parameters == null || parameters.Length == 0)
                            {
                                sb.Append(DumpObject(property.GetValue(obj, null), GetRecurisiveOptions(options), indent));
                            }
                            else
                            {
                                sb.Append("<>");
                            }

                            isFirst = false;
                        }
                    }
                    if ((options & ObjectDumpOptions.SingleLine) == 0)
                    {
                        Unindent(ref indent, sb);
                    }
                    sb.Append(']');
                }

                return sb.ToString();
            }
        }

        public static string DumpMethod(ObjectDumpOptions options, string methodName, object result, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            int indent = 0;

            sb.Append(methodName);
            sb.Append('(');

            if ((options & ObjectDumpOptions.SingleLine) == 0 && args.Length != 0)
            {
                Indent(ref indent, sb);
            }

            bool isFirst = true;

            foreach (object arg in args)
            {
                if (!isFirst)
                {
                    sb.Append(", ");

                    if ((options & ObjectDumpOptions.SingleLine) == 0)
                    {
                        NewLine(indent, sb);
                    }
                }

                sb.Append(DumpObject(arg, options, indent));
                isFirst = false;
            }

            if ((options & ObjectDumpOptions.SingleLine) == 0 && args.Length != 0)
            {
                Unindent(ref indent, sb);
            }

            sb.Append(") = ");
            sb.Append(DumpObject(result, options, indent));

            return sb.ToString();
        }

        private static string ConvertToTypeKeywords(Type type, bool fullName)
        {
            Debug.Assert(type != null);

            if (type == typeof(bool))
            {
                return "bool";
            }

            if (type == typeof(char))
            {
                return "char";
            }

            if (type == typeof(byte))
            {
                return "byte";
            }

            if (type == typeof(sbyte))
            {
                return "sbyte";
            }

            if (type == typeof(short))
            {
                return "short";
            }

            if (type == typeof(ushort))
            {
                return "ushort";
            }

            if (type == typeof(int))
            {
                return "int";
            }

            if (type == typeof(uint))
            {
                return "uint";
            }

            if (type == typeof(long))
            {
                return "long";
            }

            if (type == typeof(ulong))
            {
                return "ulong";
            }

            if (type == typeof(float))
            {
                return "float";
            }

            if (type == typeof(double))
            {
                return "double";
            }

            if (type == typeof(decimal))
            {
                return "decimal";
            }

            if (type == typeof(string))
            {
                return "string";
            }

            if (type == typeof(Nullable))
            {
                return ConvertToTypeKeywords(Nullable.GetUnderlyingType(type), fullName) + "?";
            }

            if (fullName)
            {
                return type.FullName;
            }
            else
            {
                return type.Name;
            }
        }

        private static BindingFlags GetBindingFlags(ObjectDumpOptions options)
        {
            BindingFlags flags = BindingFlags.Public;

            if ((options & ObjectDumpOptions.NonPublic) != 0)
            {
                flags |= BindingFlags.NonPublic;
            }

            if ((options & ObjectDumpOptions.Static) != 0)
            {
                flags |= BindingFlags.Static;
            }
            else
            {
                flags |= BindingFlags.Instance;
            }

            return flags;
        }

        private static ObjectDumpOptions GetRecurisiveOptions(ObjectDumpOptions options)
        {
            if ((options & ObjectDumpOptions.Recurisive) == 0)
            {
                options &= ~ObjectDumpOptions.Fields;
                options &= ~ObjectDumpOptions.Properties;
                options &= ~ObjectDumpOptions.ExpandArray;
            }

            return options;
        }

        private static void Indent(ref int indent, StringBuilder sb)
        {
            ++indent;
            NewLine(indent, sb);
        }

        private static void Unindent(ref int indent, StringBuilder sb)
        {
            --indent;
            NewLine(indent, sb);
        }

        private static void NewLine(int indent, StringBuilder sb)
        {
            sb.Append(Environment.NewLine);
            sb.Append(new string(' ', indent * 2));
        }
    }
}
