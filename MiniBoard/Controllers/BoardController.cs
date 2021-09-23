using javax.jws;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniBoard.DataContext;
using MiniBoard.Models;
using MiniBoard.Models.Dto.Reply;
using MiniBoard.Views.ViewModel;
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

            if (searchString != null)
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
                int pageSize = 5;
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
            ViewBag.UserNo = HttpContext.Session.GetInt32("USER_LOGIN_KEY");

            using (var db = new MiniBoardDbContext())
            {
                // 조회수 기능
                var boardUser = db.Boards.
                    FirstOrDefault(b => b.NoteNo.Equals(NoteNo));
                // 자신이 쓴 게시물의 조회수는 증가하지 않는다.
                var loginUser = HttpContext.Session.GetInt32("USER_LOGIN_KEY");
                if (loginUser != boardUser.UserNo)
                {
                    boardUser.Count++;
                    db.Boards.Update(boardUser);
                    db.SaveChanges();
                }

                // 기존에 한개의 board 모델만 보내는 방식에서
                // ViewModel을 추가해서 Board모델과 Reply모델을 보내는데 
                // Reply모델의 경우 board모델과는 달리 IEnumerable<List> 방식으로 보내야하기때문에
                // ViewModel을 추가할때 IEnumerable<List>로 설정
                var board = db.Boards.FirstOrDefault(b => b.NoteNo.Equals(NoteNo));
                var replyList = db.Replys.ToList().OrderByDescending(r => r.BoardNo).Where(r => r.BoardNo.Equals(NoteNo));
                BoardReplyModel boardReply = new BoardReplyModel();
                boardReply.Board = board;
                boardReply.Reply = replyList;
                return View(boardReply);
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
