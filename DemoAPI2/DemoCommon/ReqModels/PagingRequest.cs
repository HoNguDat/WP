using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCommon.ReqModels
{
    public abstract class PagingRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public PagingRequest()
        { 
            Page = 1;
            PageSize = 10;    
        }   
    }
}
