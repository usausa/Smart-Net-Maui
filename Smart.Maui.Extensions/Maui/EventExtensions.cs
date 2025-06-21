namespace Smart.Maui;

using System;
using System.Reactive.Linq;

using Microsoft.Maui.Devices.Sensors;

public static class EventExtensions
{
    // Timer

    public static IObservable<EventArgs> TickChangedAsObservable(this IDispatcherTimer timer) =>
        Observable.FromEvent<EventHandler, EventArgs>(static h => (_, e) => h(e), h => timer.Tick += h, h => timer.Tick -= h);

    // Connectivity

    public static IObservable<ConnectivityChangedEventArgs> ConnectivityChangedAsObservable(this IConnectivity connectivity) =>
        Observable.FromEvent<EventHandler<ConnectivityChangedEventArgs>, ConnectivityChangedEventArgs>(static h => (_, e) => h(e), h => connectivity.ConnectivityChanged += h, h => connectivity.ConnectivityChanged -= h);

    // Battery

    public static IObservable<BatteryInfoChangedEventArgs> BatteryInfoChangedAsObservable(this IBattery battery) =>
        Observable.FromEvent<EventHandler<BatteryInfoChangedEventArgs>, BatteryInfoChangedEventArgs>(static h => (_, e) => h(e), h => battery.BatteryInfoChanged += h, h => battery.BatteryInfoChanged -= h);

    // Accelerometer

    public static IObservable<AccelerometerChangedEventArgs> ReadingChangedAsObservable(this IAccelerometer accelerometer) =>
        Observable.FromEvent<EventHandler<AccelerometerChangedEventArgs>, AccelerometerChangedEventArgs>(static h => (_, e) => h(e), h => accelerometer.ReadingChanged += h, h => accelerometer.ReadingChanged -= h);

    // Barometer

    public static IObservable<BarometerChangedEventArgs> ReadingChangedAsObservable(this IBarometer barometer) =>
        Observable.FromEvent<EventHandler<BarometerChangedEventArgs>, BarometerChangedEventArgs>(static h => (_, e) => h(e), h => barometer.ReadingChanged += h, h => barometer.ReadingChanged -= h);

    // Compass

    public static IObservable<CompassChangedEventArgs> ReadingChangedAsObservable(this ICompass compass) =>
        Observable.FromEvent<EventHandler<CompassChangedEventArgs>, CompassChangedEventArgs>(static h => (_, e) => h(e), h => compass.ReadingChanged += h, h => compass.ReadingChanged -= h);

    // Gyroscope

    public static IObservable<GyroscopeChangedEventArgs> ReadingChangedAsObservable(this IGyroscope gyroscope) =>
        Observable.FromEvent<EventHandler<GyroscopeChangedEventArgs>, GyroscopeChangedEventArgs>(static h => (_, e) => h(e), h => gyroscope.ReadingChanged += h, h => gyroscope.ReadingChanged -= h);

    // Magnetometer

    public static IObservable<MagnetometerChangedEventArgs> ReadingChangedAsObservable(this IMagnetometer magnetometer) =>
        Observable.FromEvent<EventHandler<MagnetometerChangedEventArgs>, MagnetometerChangedEventArgs>(static h => (_, e) => h(e), h => magnetometer.ReadingChanged += h, h => magnetometer.ReadingChanged -= h);

    // OrientationSensor

    public static IObservable<OrientationSensorChangedEventArgs> ReadingChangedAsObservable(this IOrientationSensor orientation) =>
        Observable.FromEvent<EventHandler<OrientationSensorChangedEventArgs>, OrientationSensorChangedEventArgs>(static h => (_, e) => h(e), h => orientation.ReadingChanged += h, h => orientation.ReadingChanged -= h);
}
