using RDemo.Models;
using RDemo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDemo.Controllers
{
    // Home Login account='wfeang'
    public class Home : Controller
    {
        public ActionResult Index()
        {
            return View(new { 系统 = "控制管理平台" });
        }
        public ActionResult Login(string account)
        {
            return View(new LoginViews(new User() { Account = account }));
        }
        public ActionResult CheckLogin(string account, string password)
        {
            return View(new { Msg = "登录成功" });
        }
    }
}
