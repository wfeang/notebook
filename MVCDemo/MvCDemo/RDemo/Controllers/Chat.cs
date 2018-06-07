using RDemo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDemo.Controllers
{
    public class Chat : Controller
    {
        // Chat Index
        public ActionResult Index()
        {
            return View(new ChatMachineViews());
        }
    }
}
