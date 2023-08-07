using DemoCommon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCommon.ReqModels
{
    public class PostRequest
    {
        public string Content { get; set; }
        public int UserId { get; set; }
        public int? GroupId { get; set; }

        public PostRequest()
        {
        }

        public PostRequest(Post p)
        {
            Content = p.Content;
            UserId = p.UserId;
            GroupId = p.GroupId;
           
        }
    }
}
