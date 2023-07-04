namespace Smart.Maui;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMauiInterfaces(this IServiceCollection services)
    {
        services.AddSingleton(_ => AppActions.Current);
        services.AddSingleton(_ => AppInfo.Current);
        services.AddSingleton(_ => Browser.Default);
        services.AddSingleton(_ => Launcher.Default);
        services.AddSingleton(_ => Map.Default);
        services.AddSingleton(_ => VersionTracking.Default);

        services.AddSingleton(_ => Contacts.Default);
        services.AddSingleton(_ => Email.Default);
        services.AddSingleton(_ => Connectivity.Current);
        services.AddSingleton(_ => PhoneDialer.Default);
        services.AddSingleton(_ => Sms.Default);
        services.AddSingleton(_ => WebAuthenticator.Default);

        services.AddSingleton(_ => DeviceDisplay.Current);
        services.AddSingleton(_ => DeviceInfo.Current);

        services.AddSingleton(_ => Battery.Default);

        services.AddSingleton(_ => Accelerometer.Default);
        services.AddSingleton(_ => Barometer.Default);
        services.AddSingleton(_ => Compass.Default);
        services.AddSingleton(_ => Gyroscope.Default);
        services.AddSingleton(_ => Magnetometer.Default);
        services.AddSingleton(_ => OrientationSensor.Default);

        services.AddSingleton(_ => Flashlight.Default);

        services.AddSingleton(_ => Geocoding.Default);
        services.AddSingleton(_ => Geolocation.Default);

        services.AddSingleton(_ => Vibration.Default);
        services.AddSingleton(_ => HapticFeedback.Default);

        services.AddSingleton(_ => MediaPicker.Default);
        services.AddSingleton(_ => Screenshot.Default);
        services.AddSingleton(_ => TextToSpeech.Default);

        services.AddSingleton(_ => Clipboard.Default);
        services.AddSingleton(_ => Share.Default);

        services.AddSingleton(_ => FilePicker.Default);
        services.AddSingleton(_ => FileSystem.Current);
        services.AddSingleton(_ => Preferences.Default);
        services.AddSingleton(_ => SecureStorage.Default);

        services.AddSingleton(_ => SemanticScreenReader.Default);

        return services;
    }
}
