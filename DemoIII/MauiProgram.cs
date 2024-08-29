﻿using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DemoIII;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

#if MACCATALYST
       Microsoft.Maui.Handlers.HybridWebViewHandler.Mapper.AppendToMapping("Inspect", (handler, view) =>
	       {
		       if (OperatingSystem.IsMacCatalystVersionAtLeast(16, 4))
		       {
			       handler.PlatformView.Inspectable = true;
			       // For older versions .NET Mac Versions that don't include the Inspectable field.
			       // handler.PlatformView.SetValueForKey(Foundation.NSObject.FromObject(true), new Foundation.NSString("inspectable"));
		       }
       });
#endif

		return builder.Build();
	}
}