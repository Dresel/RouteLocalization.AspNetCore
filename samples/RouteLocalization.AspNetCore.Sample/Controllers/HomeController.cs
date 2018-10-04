namespace RouteLocalization.AspNetCore.Sample.Controllers
{
	using System.Diagnostics;
	using Microsoft.AspNetCore.Localization;
	using Microsoft.AspNetCore.Mvc;
	using RouteLocalization.AspNetCore.Sample.Models;

	public class HomeController : Controller
	{
		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
			});
		}

		[HttpGet("Welcome")]
		public ActionResult Index(string culture)
		{
			// RouteLocalization adds culture to the RouteData dictionary, can be bound as parameter or requested via the RouteData dictionary
			string cultureFromRouteData = (string)RouteData.Values["culture"];

			return View();
		}

		[HttpPost("Welcome")]
		public virtual ActionResult Index(object value)
		{
			return View();
		}

		[HttpPost("NotYetTranslated")]
		public virtual ActionResult NotYetTranslated()
		{
			return View("Index");
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[HttpGet("")]
		public virtual ActionResult Start()
		{
			// Redirect to localized Index
			IRequestCultureFeature requestCultureFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();

			string action = Url.Action("Index", "Home", new
			{
				culture = requestCultureFeature.RequestCulture.Culture.Name,
			});

			// Fallback if localized route does not exist
			////if (string.IsNullOrEmpty(action))
			////{
			////}

			return LocalRedirect(action);
		}
	}
}