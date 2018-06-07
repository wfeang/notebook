using RDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDemo.DAO
{
    public class ChatService : DaoService
    {
        public ChatService() : base()
        {
        }
        
        public ChatModel CheckChat(string request)
        {
            var chats = DataRepository.Select<ChatModel>("select * from chat_message where request like concat('%',@request,'%') limit 1", new { request = request });
            return chats.FirstOrDefault();
        }
    }
}
