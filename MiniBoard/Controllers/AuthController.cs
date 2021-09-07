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
using MiniBoard.Models.Dto.Kakao;

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
            FbUserVo fbUser = null;

            var dataJsonValue = JObject.Parse(dataRes.Content).ToString();
            fbUser = JsonConvert.DeserializeObject<FbUserVo>(dataJsonValue);
            var fbUserId = "fb_" + fbUser.id;
            var fbUserPassword = fbUser.id; // id로 임시 비밀번호 설정
            // 가입 되어있는 사용자 일경우 자동 로그인
            using (var db = new MiniBoardDbContext())
            {
                var user = db.Users
                    .FirstOrDefault(u => u.UserId.Equals(fbUserId) &&
                                                            u.UserPassword.Equals(fbUserPassword));
                if (user != null)
                {
                    // 로그인 성공
                    // HttpContext.Session.SetInt32(key, value);
                    HttpContext.Session.SetInt32("USER_LOGIN_KEY", user.UserNo);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // 가입되지 않은 사용자 일 경우 sns 간편 회원가입 페이지로 이동
                    TempData["UserId"] = fbUserId;
                    TempData["UserPassword"] = fbUserPassword;
                    TempData["Oauth"] = "Facebook";
                    return RedirectToAction("SnsRegister", "Auth");
                }
            }

        }

        /// <summary>
        /// 카카오톡 로그인
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IActionResult Kakao(string code)
        {
            string baseurl = "https://kauth.kakao.com";
            string tokenReqPath = "/oauth/token";
            string dataReqPath = "/v2/user/me";

            // 토큰 요청 로직
            var client = new RestClient(baseurl);
            var tokenReq = new RestRequest(tokenReqPath, Method.POST);
            tokenReq.AddHeader("Content-type", "application/x-www-form-urlencoded;charset=utf-8");
            tokenReq.AddParameter("grant_type", "authorization_code");
            tokenReq.AddParameter("client_id", "14e39f5a6a7f6927aa4e09c080412b70");
            tokenReq.AddParameter("redirect_uri", "https://localhost:44335/Auth/Kakao");
            tokenReq.AddParameter("code", code);

            var tokenRes = client.Execute(tokenReq);

            KakaoTokenVo kakaoToken = null;

            var tokenJsonValue = JObject.Parse(tokenRes.Content).ToString();
            kakaoToken = JsonConvert.DeserializeObject<KakaoTokenVo>(tokenJsonValue);
            var token = kakaoToken.access_token;

            // 사용자 정보 가져오기
            baseurl = "https://kapi.kakao.com";
            client = new RestClient(baseurl);
            var dataReq = new RestRequest(dataReqPath, Method.POST);
            dataReq.AddHeader("Content-type", "application/x-www-form-urlencoded;charset=utf-8");
            dataReq.AddHeader("Authorization", "Bearer " + token);

            var dataRes = client.Execute(dataReq);

            var dataJsonValue = JObject.Parse(dataRes.Content).ToString();

            KakaoUserVo kakaoUser = null;

            kakaoUser = JsonConvert.DeserializeObject<KakaoUserVo>(dataJsonValue);
            var kakaoUserId = "Kakao_" + kakaoUser.id;
            var kakaoUserPassword = kakaoUser.id; // id로 임시 비밀번호 설정

            // 가입 되어있는 사용자 일경우 자동 로그인
            using (var db = new MiniBoardDbContext())
            {
                var user = db.Users
                    .FirstOrDefault(u => u.UserId.Equals(kakaoUserId) &&
                                                            u.UserPassword.Equals(kakaoUserPassword));
                if (user != null)
                {
                    // 로그인 성공
                    // HttpContext.Session.SetInt32(key, value);
                    HttpContext.Session.SetInt32("USER_LOGIN_KEY", user.UserNo);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // 가입되지 않은 사용자 일 경우 sns 간편 회원가입 페이지로 이동
                    TempData["UserId"] = kakaoUserId;
                    TempData["UserPassword"] = kakaoUserPassword;
                    TempData["Oauth"] = "Kakao";
                    return RedirectToAction("SnsRegister", "Auth");
                }
            }

        }


        public IActionResult SnsRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SnsRegister(User model)
        {
            model.CreateDate = DateTime.Now;
            // 회원 가입 후 로그인
            if (ModelState.IsValid)
            {
                using (var db = new MiniBoardDbContext())
                {
                    // 회원가입 후 로그인
                    db.Users.Add(model);
                    db.SaveChanges();
                    HttpContext.Session.SetInt32("USER_LOGIN_KEY", model.UserNo);
                    return RedirectToAction("Index", "Home");
                    
                }
                // 로그인 실패
                ModelState.AddModelError(string.Empty, "사용자 ID 혹은 비밀번호가 올바르지 않습니다.");
            }
            return View(model);
        }

    }
}
