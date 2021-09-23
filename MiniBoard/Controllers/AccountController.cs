using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniBoard.DataContext;
using MiniBoard.Models;
using MiniBoard.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBoard.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 로그인
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login (LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MiniBoardDbContext())
                {
                    // Linq - 메서드 체이닝
                    var user = db.Users.
                        FirstOrDefault(u => u.UserId.Equals(model.UserId) && 
                                                   u.UserPassword.Equals(model.UserPassword));
                    if(user != null)
                    {
                        // 로그인 성공
                        HttpContext.Session.SetInt32("USER_LOGIN_KEY", user.UserNo);
                        return RedirectToAction("Index", "Home");
                    }
                }
                // 로그인 실패
                ModelState.AddModelError(string.Empty, "사용자 아이디 혹은 비밀번호가 올바르지 않습니다.");
            }
            return View(model); // 유효성 검사를 위해 model을 뷰페이지로 보냄
        }

        public IActionResult Logout()
        {
            // HttpContext.Session.Clear(); // 세션 전체 삭제
            HttpContext.Session.Remove("USER_LOGIN_KEY");
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 회원 가입
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MiniBoardDbContext())
                {
                    db.Users.Add(model);
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Home"); 
            }
            return View(model);
        }

        /// <summary>
        /// 회원 수정
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit()
        {
            using (var db = new MiniBoardDbContext())
            {
                var user = db.Users.
                    FirstOrDefault(u => u.UserNo.Equals(HttpContext.Session.GetInt32("USER_LOGIN_KEY")));
                return View(user);
            }
        }

        [HttpPost]
        public IActionResult Edit(User model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MiniBoardDbContext())
                {
                    db.Users.Update(model);
                    if (db.SaveChanges() > 0)
                    {
                        // 수정 성공
                        return RedirectToAction("Index", "Home");
                    }
                }
                // 로그인 실패
                ModelState.AddModelError(string.Empty, "수정 실패");
            }

            return View(model);
        }
    }
}
