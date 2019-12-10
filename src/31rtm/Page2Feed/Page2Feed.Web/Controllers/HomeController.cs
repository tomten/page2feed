using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GoogleSignInTest.FacebookOauth;
using GoogleSignInTest.GoogleOidc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Page2Feed.Web.Models;

namespace Page2Feed.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #region Federated authentication

        private GoogleOidcHandler _googleOidcHandler;
        private string GoogleClientId;
        private string GoogleClientSecret;
        private FacebookOauthHandler _facebookOauthHandler;
        private string FacebookAppId;
        private string FacebookAppSecret;

        public async Task<IActionResult> GoogleSignInResponse(
            AuthorizationResponse googleAuthorizationResponse
        )
        {
            var email =
                await _googleOidcHandler.HandleSignInResponse(
                    GoogleClientId,
                    googleAuthorizationResponse.Code,
                    GoogleClientSecret
                ).ConfigureAwait(false);
            return Ok(email);
        }

        /// <exception cref="T:System.ArgumentNullException"><paramref name="facebookDialogResponse"/> is <see langword="null"/></exception>
        public async Task<IActionResult> FacebookSignInResponse(
            DialogResponse facebookDialogResponse
        )
        {
            if (facebookDialogResponse == null)
                throw new ArgumentNullException(nameof(facebookDialogResponse));
            if (facebookDialogResponse.Code == null)
                throw new ArgumentNullException(nameof(facebookDialogResponse), "Code is null");
            var email =
                await _facebookOauthHandler.HandleSignInResponse(
                    FacebookAppId,
                    facebookDialogResponse.Code,
                    FacebookAppSecret
                ).ConfigureAwait(false);
            return Ok(email);
        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
