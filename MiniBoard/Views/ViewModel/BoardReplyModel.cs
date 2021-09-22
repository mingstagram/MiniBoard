using MiniBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBoard.Views.ViewModel
{
    public class BoardReplyModel
    {
        public Board Board { get; set; }
        public IEnumerable<Reply> Reply { get; set; }
    }
}
