using RDemo.Service.DAO;
using RDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDemo.Views
{
    public class ChatMachineViews : View<ChatModel>
    {
        public ChatMachineViews() : base()
        {

        }
        protected override string AnalysisAppear()
        {
           string message = string.Format(@"1、输入q退出答问 \n
2、数据消息进入答问
");
            return message;
        }
        public override RContexts Moniter()
        {
            //这里不能返回一直处于聊天状态
            OnController();
            FlushWirte("退出答问状态");
            return base.Moniter();
        }

        private void OnController()
        {
            try
            {
                while (true)
                {
                    string message = base.Read();
                    if ("q".Equals(message))
                    {
                        break;
                    }
                    //AnalysisChat(message);
                    //调用Model
                    FlushWirte("数据库部分还未链接,暂时不提供数据分析功能");
                }
            }
            catch (Exception ex)
            {
                FlushWirte("运行故障,退出答问状态");
            }
        }

        private void AnalysisChat(string message)
        {
            ChatService service = new ChatService();
            var result = service.CheckChat(message);
            if (result == null)
            {
                FlushWirte(result.Answer);
            }
            else
            {
                FlushWirte("语言库未查询到你要的数据");
            }
        }
    }
}
