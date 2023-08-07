using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DemoCommon.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int MemberQuantity { get; set; }
       
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
