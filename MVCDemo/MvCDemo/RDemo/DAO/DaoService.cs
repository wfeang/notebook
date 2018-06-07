using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDemo.DAO
{
    public abstract class DaoService
    {
        public DaoService()
        {
            this.DataRepository = new DefaultDataRepository();
        }

        protected DefaultDataRepository DataRepository { get; }
    }
}
