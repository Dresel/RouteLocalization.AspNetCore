namespace RouteLocalization.AspNetCore.Sample
{
	using System.Globalization;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Localization;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using RouteLocalization.AspNetCore.Sample.Controllers;

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			string defaultCulture = "en";

			CultureInfo[] supportedCultures = new[] { new CultureInfo(defaultCulture), new CultureInfo("de"), };

			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture(defaultCulture),

				// Formatting numbers, dates, etc.
				SupportedCultures = supportedCultures,

				// UI strings that we have localized.
				SupportedUICultures = supportedCultures,
			});

			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddLocalization();

			services.AddRouteLocalization(setup =>
			{
				setup.UseCulture("de")

					//.WhereController<HomeController>()
					//.WhereAction(action => action.Index(RouteLocalization.AspNetCore.With.Any<int>()))
					.WhereController(nameof(HomeController))
					.WhereAction(nameof(HomeController.Index))
					.TranslateAction("Willkommen");

				setup.UseCulture("de")
					.WhereController(nameof(SecondController))
					.WhereAction(nameof(SecondController.First))
					.TranslateAction("Erste");

				setup.UseCultures(new[] { "en", "de" })
					.WhereUntranslated()
					.Filter<ApiController>()
					.Filter<HomeController>(controller => controller.Start())
					.Filter<SecondController>(controller => controller.Second())
					.AddDefaultTranslation();

				setup.UseCultures(new[] { "en", "de" })
					.WhereTranslated()
					.Filter<SecondController>(controller => controller.First())
					.RemoveOriginalRoutes();
			});

			services.Configure<MvcOptions>(options => options.Conventions.Add(new CollectRoutesApplicationConvention()));
		}
	}
}