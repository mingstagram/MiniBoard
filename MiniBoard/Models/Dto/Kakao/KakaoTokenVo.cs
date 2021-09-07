using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBoard.Models.Dto.Kakao
{
    public class KakaoTokenVo
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string refresh_token_expires_in { get; set; }
        public string scope { get; set; }
    }
}
