using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBoard.Models
{
    public class User
    {
        /// <summary>
        /// 사용자 번호
        /// </summary>
        [Key] // PK 설정
        public int UserNo { get; set; }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        [Required(ErrorMessage = "사용자 이름을 입력해주세요.")] // Not Null 설정
        public string UserName { get; set; }

        /// <summary>
        /// 사용자 아아디
        /// </summary>
        [Required(ErrorMessage = "사용자 아이디를 입력해주세요.")]
        public string UserId { get; set; }

        /// <summary>
        /// 사용자 비밀번호
        /// </summary>
        [Required(ErrorMessage = "사용자 비밀번호를 입력해주세요.")]
        public string UserPassword { get; set; }

        /// <summary>
        /// SNS 사용 유무
        /// </summary>
       
        public string Oauth { get; set; }

        /// <summary>
        /// 가입일
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
