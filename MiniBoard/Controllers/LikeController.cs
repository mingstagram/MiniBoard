using Microsoft.AspNetCore.Mvc;
using MiniBoard.DataContext;
using MiniBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : Controller
    {
        /// <summary>
        /// 게시물 추천, 비추천
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/Like/Save
        [HttpPost("Save")]
        public JsonResult Save(Like model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MiniBoardDbContext())
                {
                    var like = db.Likes.FirstOrDefault(l => l.BoardNo.Equals(model.BoardNo) && l.UserNo.Equals(model.UserNo));
                    if(like == null)
                    {
                        // 추천
                        db.Likes.Add(model);
                        var board = db.Boards.
                            FirstOrDefault(b => b.NoteNo.Equals(model.BoardNo));
                        board.Like++;
                    }
                    else
                    {
                        // 비추천
                        db.Likes.Remove(like);
                        var board = db.Boards.
                            FirstOrDefault(b => b.NoteNo.Equals(model.BoardNo));
                        board.Like--;
                    }

                    if (db.SaveChanges() <= 0)
                    {
                        ModelState.AddModelError(string.Empty, "게시물 추천 실패");
                    }
                }
            }

            return Json(model);
        }

    }
}
