namespace Smart.Maui;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMauiServices(this IServiceCollection services)
    {
        services.AddSingleton(static _ => AppActions.Current);
        services.AddSingleton(static _ => AppInfo.Current);
        services.AddSingleton(static _ => Browser.Default);
        services.AddSingleton(static _ => Launcher.Default);
        services.AddSingleton(static _ => Map.Default);
        services.AddSingleton(static _ => VersionTracking.Default);

        services.AddSingleton(static _ => Contacts.Default);
        services.AddSingleton(static _ => Email.Default);
        services.AddSingleton(static _ => Connectivity.Current);
        services.AddSingleton(static _ => PhoneDialer.Default);
        services.AddSingleton(static _ => Sms.Default);
        services.AddSingleton(static _ => WebAuthenticator.Default);

        services.AddSingleton(static _ => DeviceDisplay.Current);
        services.AddSingleton(static _ => DeviceInfo.Current);

        services.AddSingleton(static _ => Battery.Default);

        services.AddSingleton(static _ => Accelerometer.Default);
        services.AddSingleton(static _ => Barometer.Default);
        services.AddSingleton(static _ => Compass.Default);
        services.AddSingleton(static _ => Gyroscope.Default);
        services.AddSingleton(static _ => Magnetometer.Default);
        services.AddSingleton(static _ => OrientationSensor.Default);

        services.AddSingleton(static _ => Flashlight.Default);

        services.AddSingleton(static _ => Geocoding.Default);
        services.AddSingleton(static _ => Geolocation.Default);

        services.AddSingleton(static _ => Vibration.Default);
        services.AddSingleton(static _ => HapticFeedback.Default);

        services.AddSingleton(static _ => MediaPicker.Default);
        services.AddSingleton(static _ => Screenshot.Default);
        services.AddSingleton(static _ => TextToSpeech.Default);

        services.AddSingleton(static _ => Clipboard.Default);
        services.AddSingleton(static _ => Share.Default);

        services.AddSingleton(static _ => FilePicker.Default);
        services.AddSingleton(static _ => FileSystem.Current);
        services.AddSingleton(static _ => Preferences.Default);
        services.AddSingleton(static _ => SecureStorage.Default);

        services.AddSingleton(static _ => SemanticScreenReader.Default);

        return services;
    }
}
