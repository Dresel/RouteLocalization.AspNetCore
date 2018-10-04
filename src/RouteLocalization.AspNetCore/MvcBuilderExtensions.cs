namespace RouteLocalization.AspNetCore
{
	using System;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Options;

	public static class MvcBuilderExtensions
	{
		public static IServiceCollection AddRouteLocalization(this IServiceCollection serviceCollection, Action<RouteTranslator> setupAction)
		{
			serviceCollection.AddSingleton<RouteTranslationConfiguration>();
			serviceCollection.AddSingleton<RouteTranslator>();
			serviceCollection.AddSingleton<RouteLocalizationConvention>();
			serviceCollection.AddSingleton<IConfigureOptions<MvcOptions>, RouteLocalizationConventionConfigureOptions>();

			serviceCollection.Configure<RouteTranslatorOptions>(options => options.RouteTranslatorAction = setupAction);

			return serviceCollection;
		}
	}
}