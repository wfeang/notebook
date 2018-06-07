using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDemo.Service.DAO
{
    /// <summary>
    /// 合并业务层可数据库链路层
    /// </summary>
    public abstract class DaoService
    {
        public DaoService()
        {
            this.DataRepository = new DefaultDataRepository();
        }

        protected DefaultDataRepository DataRepository { get; }
    }
}
