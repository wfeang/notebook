using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RDemo
{
    public class Routes
    {
        public Func<IControllers, ActionResult> LoadAction(RContexts rContexts, out IControllers controller)
        {
            controller = ActionsContainer.LoadController(rContexts);
            var returnType = typeof(ActionResult);
            var methods = controller.GetType().GetMethods();
            string actionName = rContexts.Action.ToLower();
            MethodInfo method = null;
            foreach (var item in methods)
            {
                if (item.ReturnType == returnType && item.Name.ToLower().Equals(actionName))
                {
                    method = item;
                    break;
                }
            }
            if (method == null)
            {
                throw new Exception(string.Format("被执行的action:{0}是不存在的", actionName));
            }
            var paramters = method.GetParameters();
            object[] param = GetParams(rContexts.Params, paramters);
            return (con) => { return (ActionResult)method.Invoke(con, param); };
        }

        private object[] GetParams(Dictionary<string, string> requestParams, System.Reflection.ParameterInfo[] paramters)
        {
            object[] param = new object[paramters.Length];
            int i = 0;
            foreach (var paramter in paramters)
            {
                string name = paramter.Name;
                if (!requestParams.ContainsKey(name))
                {
                    //参数是否是可空参数
                    if (paramter.HasDefaultValue)
                    {
                        param[i++] = paramter.DefaultValue;//默认值
                        continue;
                    }
                    throw new Exception("指定的参数不存在");
                }
                var data = requestParams[name];
                param[i++] = ChangeType(data, paramter.ParameterType);
            }
            return param;
        }
        private static object ChangeType(string data, Type parameterType)
        {
            if (parameterType.IsEnum)
            {
                var rdata = 0;
                if (int.TryParse(data, out rdata))
                {
                    return ChangeStructType(rdata, parameterType);
                }
                return ChangeStructType(data, parameterType);
            }
            else if (parameterType.IsValueType || parameterType == typeof(string))
            {
                return ChangeStructType(data, parameterType);
            }
            else if (parameterType.IsClass)
            {
                try
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject(data, parameterType);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("参数错误{0}", parameterType.Name));
                }
            }
            throw new Exception("暂时不支持该对象的数据转换");
        }
        private static object ChangeStructType(object value, Type type)
        {
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeStructType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
        }
    }
}
