using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess1
{

    public class UserDAL
    {
        private UserDBEntities objUserDbEntities;
        public UserDAL()
        {
            objUserDbEntities = new UserDBEntities();
        }
        public CustomBO AddUser(UserBO objUserBo)
        {
            CustomBO objCustomBo = new CustomBO();
            User objUser = new User()
            {
                UserAddress = objUserBo.UserAddress,
                UserEmail = objUserBo.UserEmail,
                UserMobile = objUserBo.UserPhone,
                UserName = objUserBo.UserName
            };
            objUserDbEntities.Users.Add(objUser);
            int returnValue = GetReturnValue();
            if (returnValue > 0)
            {
                objCustomBo.CustomMessage = "Data Successfully Added.";
                objCustomBo.CustomMessageNumber = returnValue;

            }
            else
            {
                objCustomBo.CustomMessage = "There is some problem to add User.";
                objCustomBo.CustomMessageNumber = returnValue;

            }
            return objCustomBo;

            int GetReturnValue()
            {
                return objUserDbEntities.SaveChanges();
            }
        }
    }
}
