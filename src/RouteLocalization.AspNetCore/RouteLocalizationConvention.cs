namespace RouteLocalization.AspNetCore
{
	using Microsoft.AspNetCore.Mvc.ApplicationModels;
	using Microsoft.Extensions.Options;

	public class RouteLocalizationConvention : IApplicationModelConvention
	{
		private static readonly object LockObject = new object();
		private static bool initialized;

		public RouteLocalizationConvention(RouteTranslator routeTranslator, IOptions<RouteTranslatorOptions> routeTranslatorOptions)
		{
			RouteTranslator = routeTranslator;
			RouteTranslatorOptions = routeTranslatorOptions.Value;
		}

		public RouteTranslatorOptions RouteTranslatorOptions { get; set; }

		public RouteTranslator RouteTranslator { get; set; }

		public void Apply(ApplicationModel application)
		{
			lock(LockObject)
			{
				if(!initialized)
				{
					initialized = true;
					RouteTranslatorOptions.RouteTranslatorAction(RouteTranslator);
					RouteTranslator.Apply(application);
				}
			}
		}
	}
}