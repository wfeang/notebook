using RDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDemo.Views
{
    public class LoginViews : View<User>
    {
        public LoginViews(User obj) : base(obj)
        {
        }
        protected override string AnalysisAppear()
        {
            return string.Format("账号{0},请输入登录密码", this.ObjInfo.Account);
        }
        public override RContexts Moniter()
        {
            string password = this.Read();
            var result = new RContexts("Home", "CheckLogin");
            result.Params.Add("account", this.ObjInfo.Account);
            result.Params.Add("password", password);
            return result;
        }
    }
}
