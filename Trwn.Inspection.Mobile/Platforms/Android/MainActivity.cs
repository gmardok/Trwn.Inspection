using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Trwn.Inspection.Mobile.Platforms.Android.Services;
using Trwn.Inspection.Mobile.Services;

namespace Trwn.Inspection.Mobile;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        // Forward the result to the PhotoPickerService
        var photoPickerService = MauiApplication.Current.Services.GetService<IPhotoPickerService>() as PhotoPickerService;
        photoPickerService?.OnActivityResult(requestCode, resultCode, data);
    }
}
