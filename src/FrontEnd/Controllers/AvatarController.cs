using FrontEnd.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace FrontEnd.Controllers
{
    public class AvatarController : Controller
    {
        private readonly IApiClient _apiClient;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AvatarController(IApiClient apiClient, IHostingEnvironment hostingEnvironment)
        {
            _apiClient = apiClient;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string sessionKey;
            byte[] content;

            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                sessionKey = $"AVATAR-{username}";

                if (HttpContext.Session.TryGetValue(sessionKey, out content))
                {
                    return File(content, "image/png");
                }

                var user = await _apiClient.GetAttendeeAsync(username);
                if (user != null && user.AvatarId.HasValue)
                {
                    var fileToRetrieve = await _apiClient.GetFileAsync(user.AvatarId.Value);

                    HttpContext.Session.Set(sessionKey, fileToRetrieve.Content);

                    return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
                }
            }

            var file = _hostingEnvironment.WebRootFileProvider.GetFileInfo("images/user-icon.png");
            sessionKey = "AVATAR-DEFAULT";

            if (HttpContext.Session.TryGetValue(sessionKey, out content))
            {
                return File(content, "image/png");
            }

            using (var stream = file.CreateReadStream())
            using (var reader = new BinaryReader(stream))
            {
                var bytes = reader.ReadBytes((int)file.Length);

                HttpContext.Session.Set(sessionKey, bytes);

                return File(bytes, "image/png");
            }
        }
    }
}
