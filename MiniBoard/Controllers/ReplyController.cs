using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniBoard.DataContext;
using MiniBoard.Models;
using MiniBoard.Models.Dto.Reply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplyController : Controller
    {
        // POST: api/Reply/Save
        [HttpPost("Save")]
        public JsonResult Save(Reply model)
        {
            

            if (ModelState.IsValid)
            {
                using (var db = new MiniBoardDbContext())
                {
                    // 작성자
                    var writeUser = db.Users.
                        FirstOrDefault(u => u.UserNo.Equals(HttpContext.Session.GetInt32("USER_LOGIN_KEY")));
                    model.UserName = writeUser.UserName;
                    model.CreateDate = DateTime.Now;
                    db.Replys.Add(model);
                    db.SaveChanges();
                }
                ModelState.AddModelError(string.Empty, "게시물 저장에 실패 했습니다.");
            }

            return Json(model);
        }
    }
}
