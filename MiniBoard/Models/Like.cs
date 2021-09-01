using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBoard.Models
{
    public class Like
    {
        /// <summary>
        /// 좋아요 번호
        /// </summary>
        [Key]
        public int LikeNo { get; set; }
        
        /// <summary>
        /// 좋아요 누른 게시물 번호
        /// </summary>
        [Required]
        public int BoardNo { get; set; }

        /// <summary>
        /// 좋아요 누른 사용자 번호
        /// </summary>
        [Required]
        public int UserNo { get; set; }

        //[ForeignKey("BoardNo")] // JOIN을 위한 FK 설정
        //public virtual Board Board { get; set; }
        //[ForeignKey("UserNo")] // JOIN을 위한 FK 설정
        //public virtual User User { get; set; }
    }
}
