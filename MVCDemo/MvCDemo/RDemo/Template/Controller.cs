using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RDemo
{
    public interface IControllers
    {
        RContexts Context { get; }
        void InitExecute(RContexts contexts);
        void BeforeExecute();
        void AfterExecute();
    }
    public abstract class Controller : IControllers
    {
        public Controller()
        {
        }
        private RContexts _contexts;
        public RContexts Context { get { return this._contexts; } }


        public void InitExecute(RContexts contexts)
        {
            this._contexts = contexts;
        }
        public virtual void BeforeExecute()
        {

        }
        

        public virtual void AfterExecute()
        {

        }

        protected ActionResult View(object obj)
        {
            var defaultv = new View(obj);
            return new ActionResult(defaultv, this.Context);
        }
        protected ActionResult View(IViews views)
        {
            return new ActionResult(views, this.Context);
        }
    }

}
