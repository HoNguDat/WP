using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DemoCommon.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public int GroupId { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
