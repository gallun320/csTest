
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Permissions;
using CsTest.Requester;
using CsTest.Attributes;


namespace CsTest.Extentions
{
    public static class Ext 
    {
        private static Dictionary<Type, ConstructorInfo> chacheDict = new Dictionary<Type, ConstructorInfo>() {};
        public static string ConverResult<T>(this T data)
        {
           
           var sb = new StringBuilder();
            data.To<string>(out var st);
           sb.Append("<html><head><meta charset='utf8'></head><body>Привет мир! ").Append(st).Append("</body></html>");

            return sb.ToString();
        }

        public static object ChangeType(this object o, Type type) =>
            o == null || type.IsValueType || o is IConvertible ?
                Convert.ChangeType(o, type, null) :
                o;

        public static T To<T>(this T o) => o;
        public static T To<T>(this T o, out T x) => x = o;
        public static T To<T>(this object o) => (T) ChangeType(o, typeof(T));
        public static T To<T>(this object o, out T x) => x = (T) ChangeType(o, typeof(T));
        public static bool TryGetRequestClass<T>(this string prop, out dynamic result)
        {
            
            var typeAsm = typeof(T);

            if(chacheDict.TryGetValue(typeAsm, out var cs)) {
                result = cs.Invoke(new object[] {});
                return true;
            }

            var typesAsm = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(s => s.GetTypes())
                            .Where(t => typeAsm.IsAssignableFrom(t)).ToList(); 

            var atr = typesAsm.Select(item => new {
                                    Attr = item.GetCustomAttribute<HttpMethod>(),
                                    Item = item
                                })
                                .Where(at => at.Attr.method.Equals(prop))
                                .FirstOrDefault();

            if(atr == null) 
            { 
                result = default(T);
                return false;
            }

            var itemCl = atr.Item.GetConstructor(Type.EmptyTypes);

            chacheDict.TryAdd(typeAsm, itemCl);

            result = itemCl.Invoke(new object[] {});
            return true;
        }

    }
}