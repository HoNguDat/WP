using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DemoCommon.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string Content { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string? PostImage { get; set; }
        public virtual User User { get; set; }
        public virtual Group Group { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
       

    }
}
