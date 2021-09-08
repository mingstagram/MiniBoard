using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBoard.Models.Dto.Naver
{
    public class NaverUserVo
    {
        public string resultcode { get; set; }
        public string message { get; set; }
        public Response response { get; set; }
        public class Response
        {
            public string id { get; set; }
            public string email { get; set; }
            public string name { get; set; }
        }
    }
}
