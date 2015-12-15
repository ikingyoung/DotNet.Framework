using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace DotNet.Framework.Common.Extensions
{
    public static class ExtensionGroup
    {
        private static ConcurrentDictionary<Type, Type> _cache = new ConcurrentDictionary<Type, Type>();

        public static T As<T>(this string v) where T : IExtension<string>
        {
            return As<T, string>(v);
        }

        public static T As<T, V>(this V v) where T : IExtension<V>
        {
            Type t;
            var valueType = typeof(V);

            if (!_cache.TryGetValue(valueType, out t))
            {
                t = CreateType<T, V>();
                _cache.AddOrUpdate(valueType, t, (key, value) => t);
            }

            var result = Activator.CreateInstance(t, v);

            return (T)result;
        }

        /// <summary>
        /// 通过反射构建继承自IConvertXxx的ConvertXxx实体类
        /// </summary>
        /// <typeparam name="T">处理接口类型,如:IConvertXxxxx</typeparam>
        /// <typeparam name="V">值类型：需要处理的值</typeparam>
        /// <returns></returns>
        private static Type CreateType<T,V>() where T : IExtension<V>
        {
            // 获取值类型
            var targetInterfaceType = typeof(T);
            string generatedClassName = targetInterfaceType.Name.Remove(0, 1);

            // 动态构建程序集
            var assemblyName = new AssemblyName("ExtensionDynamicAssembly");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            var typeBuilder = moduleBuilder.DefineType(generatedClassName, TypeAttributes.Public);

            // 通过反射实现相应的扩展类接口
            typeBuilder.AddInterfaceImplementation(typeof(T));
            // value字段
            var valueFiled = typeBuilder.DefineField("value", typeof(V), FieldAttributes.Private);
            // 构造函数
            var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(V) });
            var ctorIL = ctor.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_1);
            ctorIL.Emit(OpCodes.Stfld, valueFiled);
            ctorIL.Emit(OpCodes.Ret);
            //// GetValue方法
            //var getValueMethod = typeBuilder.DefineMethod("GetValue",MethodAttributes.Public|MethodAttributes.Virtual,typeof(V),Type.EmptyTypes);
            //var numberGetIL = getValueMethod.GetILGenerator();
            //numberGetIL.Emit(OpCodes.Ldarg_0);
            //numberGetIL.Emit(OpCodes.Ldfld,valueFiled);
            //numberGetIL.Emit(OpCodes.Ret);
            // 定义 Self 属性
            PropertyBuilder selfPropertyBuilder = typeBuilder.DefineProperty(
                "Self",
                PropertyAttributes.None,
                typeof(V),
                Type.EmptyTypes
            );
            // 定义 Self 属性的 get 方法
            MethodBuilder get_SelfMethod = typeBuilder.DefineMethod(
                "get_Self",
                MethodAttributes.Public | MethodAttributes.Virtual,
                typeof(string),
                Type.EmptyTypes
            );
            ILGenerator get_SelfIL = get_SelfMethod.GetILGenerator();
            get_SelfIL.Emit(OpCodes.Ldarg_0);
            get_SelfIL.Emit(OpCodes.Ldfld, valueFiled);
            get_SelfIL.Emit(OpCodes.Ret);
            // 注册到 Self 属性生成器中
            selfPropertyBuilder.SetGetMethod(get_SelfMethod);

            //// 接口实现
            //var getValueInfo = targetInterfaceType.GetInterfaces()[0].GetMethod("GetValue");
            //typeBuilder.DefineMethodOverride(getValueMethod,getValueInfo);

            var t =typeBuilder.CreateType();
            return t;
        }
    }
}
