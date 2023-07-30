using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCommon.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
