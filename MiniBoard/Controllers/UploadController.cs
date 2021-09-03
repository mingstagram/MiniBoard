using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBoard.Controllers
{
    public class UploadController : Controller
    {
        // DI(의존성 주입)
        private readonly IHostingEnvironment _environment;
        // 생성자 단축키 -> ctor 탭탭
        public UploadController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        [Route("api/upload")] // http://www.example.com/api/upload
        
         public IActionResult ImageUpload(IFormFile file)
         // public async Task<IActionResult> ImageUpload(IFormFile file) - 비동기 처리시 
        {
            // # 이미지나 파일을 업로드 할 때 필요한 구성
            // 1. Path(경로) - 어디에 저장할지 결정
            var path = Path.Combine(_environment.WebRootPath, @"images\upload"); // wwwroot\images\upload
            // 2. Name(이름) - DateTime, GUID(전역 고유 식별자) + GUID
            // 3. Extension(확장자) - jpg, png, txt ......
            var nowDate = DateTime.Now.ToString("yyyyMMddhhmmss");
            // 중복방지를 위해 난수생성 및 길이가 너무 길어서 split으로 문자열 나눔
            var guid = Guid.NewGuid().ToString().Split("-")[0]; 
            // 확장자를 얻기위해 split으로 분류한 뒤 확장자 가져옴
            var extensionName = file.FileName.Split(".")[1];
            // fileName = '현재날짜시간'_'난수'.'확장자'
            var fileName = $"{nowDate}_{guid}.{extensionName}"; // ex) 20210903014402_b252af11.jpg

            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                // await file.CopyToAsync(fileStream); - 비동기 처리시 async - await
                file.CopyTo(fileStream);
            };
            return Ok(new { file = "/images/upload/" + fileName, success = true });

            // # URL 접근 방식
            // ASP.NET - 호스트명/ + api/upload
            // JavaScript - 호스트명 + /api/upload
        }
    }
}
