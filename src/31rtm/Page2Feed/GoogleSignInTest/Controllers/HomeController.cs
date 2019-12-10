using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GoogleSignInTest.FacebookOauth;
using GoogleSignInTest.GoogleOidc;
using GoogleSignInTest.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GoogleSignInTest.Web.Controllers
{

    public class HomeController : Controller
    {

        private readonly GoogleOidcHandler _googleOidcHandler;
        private readonly FacebookOauthHandler _facebookOauthHandler;

        private const string GoogleClientId = "810635674435-fhasjeu2lto7b22uuda8h8m4jqhh3l3i.apps.googleusercontent.com";
        private const string GoogleClientSecret = "9wmkGcaVzEQpvR5rzTjGR413";
        public const string FacebookAppId = "6315403257";
        public const string FacebookAppSecret = "750490f9d74458d1bb1776079b5c10f9";

        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            GoogleOidcHandler googleOidcHandler,
            FacebookOauthHandler facebookOauthHandler
            )
        {
            _logger = logger;
            _googleOidcHandler = googleOidcHandler;
            _facebookOauthHandler = facebookOauthHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ExternalSignIn()
        {

            var googleOidcSignInResponseUri = Url.Action(nameof(GoogleSignInResponse), "home", null, Request.Scheme, Request.Host.ToString());
            var googleOidcSignInUri = await _googleOidcHandler.CreateGoogleSignInUri(
                    GoogleClientId,
                    googleOidcSignInResponseUri
                    ).ConfigureAwait(false);
            ViewData["GoogleOidcSignInUri"] = googleOidcSignInUri;

            var facebookOauthSignInResponseUri = Url.Action(nameof(FacebookSignInResponse), "home", null, Request.Scheme, Request.Host.ToString());
            var facebookOauthSignInUri = await _facebookOauthHandler.MakeFacebookSignInUri(
                    FacebookAppId,
                    facebookOauthSignInResponseUri.Uri()
                    ).ConfigureAwait(false);
            ViewData["FacebookOauthSignInUri"] = facebookOauthSignInUri;

            return View();
        }

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
