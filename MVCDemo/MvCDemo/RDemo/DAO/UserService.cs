using RDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDemo.DAO
{
    public class UserService : DaoService
    {
        public User LoadUser(string account,string password)
        {
            string sql = "select * from user where account = @account and password = @password ";
            var result = this.DataRepository.Select<User>(sql, new { account = account, password = password });
            return result.FirstOrDefault();
        }
    }
}
