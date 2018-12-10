using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurtazaSaleem_Assignment4.Models
{
    public enum UserRole : byte
    {
        User = 1,
        Manager
    }

    public class User
    {
        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please provide a user name.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        public string Password { get; set; }

        [Column("Type")]
        public UserRole Role { get; set; }

        /// <summary>
        /// Checks whether the password is correct for the given user
        /// </summary>
        /// <returns> whether the password is valid or not </returns>
        public bool CheckPassword()
        {
            //The requirements for the password is to be equal with the user name
            //in all lower-case and reversed
            return Password == new string(UserName.ToLower().Reverse().ToArray());
        }
    }
}
