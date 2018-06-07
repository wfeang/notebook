using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDemo
{
    public interface IViews
    {
        RContexts Context { get; }
        void InitAppear(RContexts contexts);
        void Before();
        void Appear();
        void End();
        RContexts Moniter();
    }
    public class View : View<object>
    {
        public View()
        {

        }
        public View(object obj) : base(obj)
        { 
        }
        public View(object obj, RContexts contexts) : base(obj,contexts)
        {
        }
    }
    public class View<T> : IViews
    {
        public T ObjInfo { get; private set; }
        private RContexts _contexts;
        public RContexts Context { get { return _contexts; } }
        public View()
        {

        }
        public View(T obj) : this()
        {
            this.ObjInfo = obj;
        }
        public View(T obj,RContexts contexts) : this()
        {
            this.ObjInfo = obj;
            this._contexts = contexts;
        }

        public void InitAppear(RContexts contexts)
        {
            this._contexts = contexts;
        }
        public virtual void Before()
        {

        }
        public virtual void Appear()
        {
            try
            {
                string result = AnalysisAppear();
                FlushWirte(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual void End()
        {
        }
        public virtual RContexts Moniter()
        {
            string str = Read();
            try
            {
               return AnalysisRead(str);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.Moniter();
            }
        }

        protected virtual string AnalysisAppear()
        {
            return GetData(this.ObjInfo);
        }

        public void FlushWirte(string message)
        {
            Console.WriteLine(message);
        }
        public string Read()
        {
            var data = Console.ReadLine();
            if (data == "quit")
            {
                Environment.Exit(0);
            }
            return data;
        }

        protected RContexts AnalysisRead(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new Exception("参数错误");
            }
            string[] strs = str.Split(' ');
            strs = strs.Select(a => a.Trim()).Where(a => !string.IsNullOrEmpty(a)).ToArray();
            if (strs.Length < 2)
            {
                throw new Exception("参数错误,参数不足");
            }
            var result = new RContexts(strs[0], strs[1]);
            for (int i = 2; i < strs.Length; i++)
            {
                var data = strs[i].Split('=');
                if (result.Params.ContainsKey(data[0]))
                {
                    throw new Exception("参数重复" + data[0]);
                }
                if (data.Length != 2)
                {
                    throw new Exception("参数错误,输入的消息结构是不正确的");
                }
                result.Params.Add(data[0], data[1]);
            }
            return result;
        }

        private string GetData(object objInfo)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (ObjInfo == null)
            {
                return "控制器什么都没有返回";
            }
            if (ObjInfo.GetType().IsArray)
            {
                var data = this.ObjInfo as System.Collections.IEnumerable;
                foreach (var item in data)
                {
                    stringBuilder.Append(GetData(item));
                }
            }
            else
            {
                stringBuilder.Append(GetObjectDesc(this.ObjInfo));
            }
            return stringBuilder.ToString();
        }

        private string GetObjectDesc(object objInfo)
        {
            var type = objInfo.GetType();
            var properties = type.GetProperties();
            StringBuilder result = new StringBuilder();
            foreach (var item in properties)
            {
                if (item.PropertyType.IsClass && item.PropertyType != typeof(string))
                {
                    result.Append(GetData(objInfo));
                }
                else
                {
                    result.Append(string.Format("{0}:{1}",item.Name,item.GetValue(objInfo)));
                }
            }
            return result.ToString();
        }
    }
}
