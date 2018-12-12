namespace RouteLocalization.AspNetCore
{
	using Microsoft.AspNetCore.Mvc.ApplicationModels;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Logging.Abstractions;
	using Microsoft.Extensions.Options;

	public class RouteTranslator
	{
		public RouteTranslator(IOptions<RouteTranslationConfiguration> routeTranslationConfiguration,
			IOptions<RouteTranslatorOptions> routeTranslatorOptions)
			: this(routeTranslationConfiguration, routeTranslatorOptions, NullLoggerFactory.Instance)
		{
		}

		public RouteTranslator(IOptions<RouteTranslationConfiguration> routeTranslationConfiguration,
			IOptions<RouteTranslatorOptions> routeTranslatorOptions, ILoggerFactory loggerFactory)
		{
			RouteTranslationConfiguration = routeTranslationConfiguration.Value;
			LoggerFactory = loggerFactory;

			routeTranslatorOptions.Value.RouteTranslatorAction(this);
		}

		public RouteTranslationConfiguration RouteTranslationConfiguration { get; set; }

		public RouteTranslationStore RouteTranslationStore { get; set; } = new RouteTranslationStore();

		protected ILoggerFactory LoggerFactory { get; set; }

		public void Apply(ApplicationModel applicationModel)
		{
			foreach (RouteSelectorProcessorPair configuration in RouteTranslationStore)
			{
				configuration.Processor.Process(configuration.Selector.Select(applicationModel));
			}
		}

		public RouteTranslationBuilder UseCulture(string culture)
		{
			return new RouteTranslationBuilder(RouteTranslationConfiguration.Clone(), RouteTranslationStore, LoggerFactory)
				.UseCulture(culture);
		}

		public RouteTranslationBuilder UseCultures(string[] cultures)
		{
			return new RouteTranslationBuilder(RouteTranslationConfiguration.Clone(), RouteTranslationStore, LoggerFactory)
				.UseCultures(cultures);
		}
	}
}