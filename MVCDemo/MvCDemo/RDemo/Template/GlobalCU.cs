using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDemo.Template
{
    public class GlobalCU
    {
        private static GlobalCU CU = null;
        private static object lockc = new object();
        public Routes Routes { get; private set; }
        private GlobalCU(Routes routes) { this.Routes = routes; }
        public static GlobalCU Invoke(Routes routes)
        {
            lock (lockc)
            {
                if (CU == null)
                {
                    CU = new GlobalCU(routes);
                }
            }
            return CU;
        }

        public void Begin(RContexts defaultContext)
        {
            IControllers controllers = null;
            var action = this.Routes.LoadAction(defaultContext, out controllers);
            ActionResult actionResult = null;
            try
            {
                controllers.BeforeExecute();
                actionResult = action(controllers);
                controllers.AfterExecute();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            IViews views = null;
            try
            {
                if ((views = actionResult.View) != null)
                {
                    views.InitAppear(defaultContext);
                    views.Before();
                    views.Appear();
                    views.End();
                    DoNext(actionResult);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("绘制视图信息时出错", ex);
            }
        }

        private void DoNext(ActionResult actionResult)
        {
            var nextContext = actionResult.View.Moniter();
            try
            {
                Begin(nextContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DoNext(actionResult);
            }
        }
    }
}
