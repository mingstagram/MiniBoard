using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniBoard.DataContext;
using MiniBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Mvc.Core;

namespace MiniBoard.Controllers
{
    public class BoardController : Controller
    {
        /// <summary>
        ///  게시판 리스트
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(string searchString, string currentFilter, int? page)
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                // 로그인이 안된 상태
                return RedirectToAction("Login", "Account");
            }

            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            using (var db = new MiniBoardDbContext())
            {
                int pageSize = 3;
                int pageNumber = (page ?? 1);
                if (!String.IsNullOrEmpty(searchString))
                {
                    var list = db.Boards.ToList().OrderByDescending(n => n.NoteNo).Where(s => s.NoteTitle.Contains(searchString));
                    return View(list.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    var list = db.Boards.ToList().OrderByDescending(n => n.NoteNo);
                    return View(list.ToPagedList(pageNumber, pageSize));
                }
            }
        }

        /// <summary>
        /// 게시판 상세
        /// </summary>
        /// <param name="NoteNo"></param>
        /// <returns></returns>
        public IActionResult Detail(int NoteNo)
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                // 로그인이 안된 상태
                return RedirectToAction("Login", "Account");
            }
            using (var db = new MiniBoardDbContext())
            {
                var board = db.Boards.FirstOrDefault(b => b.NoteNo.Equals(NoteNo));
                return View(board);
            }
        }

        /// <summary>
        /// 게시물 추가
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                // 로그인이 안된 상태
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Add(Board model)
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                // 로그인이 안된 상태
                return RedirectToAction("Login", "Account");
            }
            model.UserNo = int.Parse(HttpContext.Session.GetInt32("USER_LOGIN_KEY").ToString());
            model.CreateDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                using (var db = new MiniBoardDbContext())
                {
                    db.Boards.Add(model);

                    if(db.SaveChanges() > 0)
                    {
                        return Redirect("Index");
                    }
                }
                ModelState.AddModelError(string.Empty, "게시물 저장에 실패 했습니다.");
            }

            return View(model);
        }

        /// <summary>
        /// 게시물 수정
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// 게시물 삭제
        /// </summary>
        /// <returns></returns>
        public IActionResult Delete()
        {
            return View();
        }

    }
}
