using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;
using MiniBoard.Models.Dto;
using Newtonsoft.Json;
using MiniBoard.Models.Dto.Facebook;
using MiniBoard.Models;
using MiniBoard.DataContext;
using Microsoft.AspNetCore.Http;

namespace MiniBoard.Controllers
{
    public class AuthController : Controller
    {
        /// <summary>
        /// 페이스북 로그인
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Facebook(string code)
        {
            // 요청을 보내는 URI 
            string baseurl = "https://graph.facebook.com";
            string tokenReqPath = "/v11.0/oauth/access_token";
            string client_id = "583538316016215";
            string redirect_uri = "https://localhost:44335/Auth/Facebook";
            string client_secret = "dd3c262f17f0e32ff5d019a7738457f2";
            string debugReqPath = "/debug_token";
            string userReqPath = "/me";

            // 토큰 요청 로직
            var client = new RestClient(baseurl);
            var tokenReq = new RestRequest(tokenReqPath, Method.GET);
            tokenReq.AddParameter("client_id", client_id);
            tokenReq.AddParameter("redirect_uri", redirect_uri);
            tokenReq.AddParameter("client_secret", client_secret);
            tokenReq.AddParameter("code", code);

            var tokenRes = client.Execute(tokenReq);

            FbTokenVo fbToken = null;

            var tokenJsonValue = JObject.Parse(tokenRes.Content).ToString();
            fbToken = JsonConvert.DeserializeObject<FbTokenVo>(tokenJsonValue);
            var token = fbToken.access_token;

            // 토큰 검사 요청 로직
            var debugReq = new RestRequest(debugReqPath, Method.GET);
            debugReq.AddParameter("input_token", token);
            debugReq.AddParameter("access_token", token);

            var debugRes = client.Execute(debugReq);
            var debugJsonValue = JObject.Parse(debugRes.Content).ToString();

            // 유저 데이터 요청 로직
            var dataReq = new RestRequest(userReqPath, Method.GET);
            dataReq.AddParameter("fields", "id, first_name, last_name, email");
            dataReq.AddParameter("access_token", token);

            var dataRes = client.Execute(dataReq);
            FbUserVo fbUserData = new FbUserVo();

            var dataJsonValue = JObject.Parse(dataRes.Content).ToString();
            fbUserData = JsonConvert.DeserializeObject<FbUserVo>(dataJsonValue);

            // 회원 가입 로직
            User fbUser = new User();
            fbUser.UserId = "fb_" + fbUserData.id;
            fbUser.UserName = fbUserData.first_name + fbUserData.last_name;
            fbUser.UserPassword = fbUserData.id; // id로 임시 비밀번호 설정
            fbUser.Oauth = "Facebook";
            fbUser.CreateDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                using (var db = new MiniBoardDbContext())
                {
                    db.Users.Add(fbUser);
                    db.SaveChanges();
                }
            }

            // 회원 가입 후 로그인

            if (ModelState.IsValid)
            {
                using (var db = new MiniBoardDbContext())
                {
                    var user = db.Users
                        .FirstOrDefault(u => u.UserId.Equals(fbUser.UserId) &&
                                                                u.UserPassword.Equals(fbUser.UserPassword));
                    if (user != null)
                    {
                        // 로그인 성공
                        // HttpContext.Session.SetInt32(key, value);
                        HttpContext.Session.SetInt32("USER_LOGIN_KEY", user.UserNo);
                        return RedirectToAction("Index", "Home");
                    }
                }
                // 로그인 실패
                ModelState.AddModelError(string.Empty, "사용자 ID 혹은 비밀번호가 올바르지 않습니다.");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
