using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RDemo
{
    public class ActionsContainer
    {
        protected static Dictionary<string, Type> Infos = new Dictionary<string, Type>();
        public static IControllers LoadController(RContexts contexts)
        {
            //加载程序集信息
            InitController();
            string controller = contexts.Controller.ToLower();
            if (Infos.ContainsKey(controller))
            {
                var type = Infos[controller];
                return type.Assembly.CreateInstance(type.FullName) as IControllers;
            }
            throw new Exception("你访问的控制器不存在");
        }

        private static void InitController()
        {
            if (Infos.Count != 0)
            {
                return;
            }
            var assembly = Assembly.GetAssembly(typeof(ActionsContainer));
            var types = assembly.GetTypes();
            foreach (var item in types)
            {
                if (typeof(IControllers).IsAssignableFrom(item) && !item.IsAbstract)
                {
                    string controllerName = item.Name.ToLower();
                    if (Infos.ContainsKey(controllerName))
                    {
                        throw new Exception("控制器名称重复");
                    }
                    Infos[controllerName] = item;
                }
            }
        }
    }
    public class ActionResult
    {
        public ActionResult(RContexts contexts)
        {
            this.Context = contexts;
        }
        public ActionResult(IViews views, RContexts contexts)
        {
            this.Context = contexts;
            this.View = views;
        }
        public RContexts Context{get;set;}
        public IViews View { get; set; }
    }
    public class RContexts
    {
        public Dictionary<string, string> Params { get; private set; }
        public string Controller { get; private set; }
        public string Action { get; private set; }
        public RContexts(string controller,string action)
        {
            this.Params = new Dictionary<string, string>();
            this.Action = action;
            this.Controller = controller;
        }
    }
}
