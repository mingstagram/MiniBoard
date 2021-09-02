using System.ComponentModel.DataAnnotations;

namespace MiniBoard.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "사용자 아이디를 입력해주세요.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "사용자 비밀번호를 입력해주세요.")]
        public string UserPassword { get; set; }
    }
}
