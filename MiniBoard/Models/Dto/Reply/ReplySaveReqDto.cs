using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBoard.Models.Dto.Reply
{
    public class ReplySaveReqDto
    {
        public int UserNo { get; set; }
        public int BoardNo { get; set; }
        public string ReplyContents { get; set; }

    }
}
