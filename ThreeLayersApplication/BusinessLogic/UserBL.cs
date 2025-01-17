using BusinessObject;
using DataAccess1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public  class UserBL
    {
        public CustomBO AddUser(UserBO objUserBo)
        {
            return new UserDAL().AddUser(objUserBo);
        }
    }
}
