using DemoCommon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCommon.ResModels
{
    public class PostResponse
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public string FullName { get; set; }
        public string GroupName { get; set; }
        public string PostImage { get; set; }
        public DateTime? CreatedDateTime { get; set; }

        public PostResponse()
        {
        }

        public PostResponse(Post p)
        {
            FullName = p.User.LastName + " " + p.User.FirstName;
            PostId = p.PostId;
            Content = p.Content;
            GroupName = p.Group.Name;
            PostImage = p.PostImage;
            CreatedDateTime = p.CreatedDateTime;
        }

    }
}
