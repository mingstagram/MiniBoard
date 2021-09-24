using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBoard.Models
{
    public class Board
    {
        /// <summary>
        /// 게시물 번호
        /// </summary>
        [Key]
        public int NoteNo { get; set; }

        /// <summary>
        /// 게시물 제목
        /// </summary>
        [Required(ErrorMessage = "제목을 입력하세요.")]
        public string NoteTitle { get; set; }

        /// <summary>
        /// 게시물 내용
        /// </summary>
        [Required(ErrorMessage = "내용을 입력하세요.")]
        public string NoteContents { get; set; }

        /// <summary>
        /// 게시물 작성일
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 게시물 조회수
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 게시물 좋아요
        /// </summary>
        public int Like { get; set; }

        /// <summary>
        /// 작성자 이름
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 작성자 번호
        /// </summary>
        [Required]
        public int UserNo { get; set; }

        [ForeignKey("UserNo")] // JOIN을 위한 FK 설정
        public virtual User User { get; set; }

    }
}
