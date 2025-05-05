using CommunityToolkit.Maui;
using Maui.FreakyControls.Extensions;
using Microsoft.Extensions.Logging;
using Trwn.Inspection.Mobile.Services;
using Trwn.Inspection.Mobile.ViewModels;

namespace Trwn.Inspection.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#if IOS
    builder.Services.AddSingleton<IPhotoPickerService, Trwn.Inspection.Mobile.Platforms.iOS.Services.PhotoPickerService>();
#endif
#if ANDROID
        builder.Services.AddSingleton<IPhotoPickerService, Trwn.Inspection.Mobile.Platforms.Android.Services.PhotoPickerService>();
#endif
		builder.Services.AddTransient<PhotoDetailsViewModel>();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.InitializeFreakyControls();

        return builder.Build();
	}
}
