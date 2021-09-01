using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBoard.Models
{
    public class Reply
    {
        /// <summary>
        /// 댓글 번호
        /// </summary>
        [Key]
        public int ReplyNo { get; set; }

        /// <summary>
        /// 댓글 내용
        /// </summary>
        [Required]
        public string ReplyContents { get; set; }

        /// <summary>
        /// 댓글 작성일
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 댓글 작성한 게시물 번호
        /// </summary>
        [Required]
        public int BoardNo { get; set; }

        /// <summary>
        /// 댓글 작성한 작성자 번호
        /// </summary>
        [Required]
        public int UserNo { get; set; }

        //[ForeignKey("BoardNo")] // JOIN을 위한 FK 설정
        //public virtual Board Board { get; set; }
        //[ForeignKey("UserNo")] // JOIN을 위한 FK 설정
        //public virtual User User { get; set; }

    }
}
