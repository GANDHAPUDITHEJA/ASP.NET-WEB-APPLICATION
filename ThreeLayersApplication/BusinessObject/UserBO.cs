using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace BusinessObject
{
    public class UserBO
    {
        public int UserId {  get; set; }
        [Display(Name ="User Name")]
        [Required(ErrorMessage ="User name is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "User Address is required.")]
        [Display(Name = "User Address")]
        public string UserAddress { get; set; }
        [Required(ErrorMessage = "User Email is required.")]
        [Display(Name = "User Email")]
        [EmailAddress(ErrorMessage ="please enter valid email address.")]
        public string UserEmail { get; set; }
        [Required(ErrorMessage = "User Phone is required.")]
        [Display(Name = "User Phone")]
        public string UserPhone { get; set; }
    }
}
