﻿using FrontEndNetMaui.Model.EmailSorter;
using FrontEndNetMaui.Services;
using FrontEndNetMaui.View;
using FrontEndNetMaui.ViewModel;
using Serilog;

namespace FrontEndNetMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("logs/frontendmaui.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

        // Initialise community toolkit
        builder.UseMauiApp<App>().UseMauiCommunityToolkitCore();

        builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<HttpClient>();

		builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

		builder.Services.AddSingleton<IApiGatewayService, ApiGatewayV1Service>();
		builder.Services.AddSingleton<IDisplayService, DisplayService>();
		builder.Services.AddSingleton<IEmailSorter, EmailSorterV1>();

		builder.Services.AddSingleton<MainPageViewModel>();

		builder.Services.AddSingleton<MainPage>();

		return builder.Build();
	}
}
