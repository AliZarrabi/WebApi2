using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCCore.Models
{
    public class MessageViewModel
    {
        [Display(Name = "کد پیام")]
        public int MessageID { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "متن")]
        public string Content { get; set; }

        [Display(Name = "کد کاربر")]
        public string UserID { get; set; }

        public bool IsUser { get; set; }
    }
}
