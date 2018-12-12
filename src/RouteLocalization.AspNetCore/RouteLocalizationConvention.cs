namespace RouteLocalization.AspNetCore
{
	using Microsoft.AspNetCore.Mvc.ApplicationModels;

	public class RouteLocalizationConvention : IApplicationModelConvention
	{
		public RouteLocalizationConvention(RouteTranslator routeTranslator)
		{
			RouteTranslator = routeTranslator;
		}

		public RouteTranslator RouteTranslator { get; set; }

		public void Apply(ApplicationModel application)
		{
			RouteTranslator.Apply(application);
		}
	}
}