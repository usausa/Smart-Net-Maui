namespace Smart.Maui;

using System;
using System.Reactive.Linq;

using Microsoft.Maui.Devices.Sensors;

public static class EventExtensions
{
    // Connectivity

    public static IObservable<ConnectivityChangedEventArgs> ObserveConnectivityChanged(this IConnectivity connectivity) =>
        Observable.FromEvent<EventHandler<ConnectivityChangedEventArgs>, ConnectivityChangedEventArgs>(h => (_, e) => h(e), h => connectivity.ConnectivityChanged += h, h => connectivity.ConnectivityChanged -= h);

    public static IObservable<ConnectivityChangedEventArgs> ObserveConnectivityChangedOnCurrentContext(this IConnectivity connectivity) =>
        connectivity.ObserveConnectivityChanged().ObserveOn(SynchronizationContext.Current!);

    // Battery

    public static IObservable<BatteryInfoChangedEventArgs> ObserveBatteryInfoChanged(this IBattery battery) =>
        Observable.FromEvent<EventHandler<BatteryInfoChangedEventArgs>, BatteryInfoChangedEventArgs>(h => (_, e) => h(e), h => battery.BatteryInfoChanged += h, h => battery.BatteryInfoChanged -= h);

    public static IObservable<BatteryInfoChangedEventArgs> ObserveBatteryInfoChangedOnCurrentContext(this IBattery battery) =>
        battery.ObserveBatteryInfoChanged().ObserveOn(SynchronizationContext.Current!);

    // Accelerometer

    public static IObservable<AccelerometerChangedEventArgs> ObserveReadingChanged(this IAccelerometer accelerometer) =>
        Observable.FromEvent<EventHandler<AccelerometerChangedEventArgs>, AccelerometerChangedEventArgs>(h => (_, e) => h(e), h => accelerometer.ReadingChanged += h, h => accelerometer.ReadingChanged -= h);

    public static IObservable<AccelerometerChangedEventArgs> ObserveReadingChangedOnCurrentContext(this IAccelerometer accelerometer) =>
        accelerometer.ObserveReadingChanged().ObserveOn(SynchronizationContext.Current!);

    // Barometer

    public static IObservable<BarometerChangedEventArgs> ObserveReadingChanged(this IBarometer barometer) =>
        Observable.FromEvent<EventHandler<BarometerChangedEventArgs>, BarometerChangedEventArgs>(h => (_, e) => h(e), h => barometer.ReadingChanged += h, h => barometer.ReadingChanged -= h);

    public static IObservable<BarometerChangedEventArgs> ObserveReadingChangedOnCurrentContext(this IBarometer barometer) =>
        barometer.ObserveReadingChanged().ObserveOn(SynchronizationContext.Current!);

    // Compass

    public static IObservable<CompassChangedEventArgs> ObserveReadingChanged(this ICompass compass) =>
        Observable.FromEvent<EventHandler<CompassChangedEventArgs>, CompassChangedEventArgs>(h => (_, e) => h(e), h => compass.ReadingChanged += h, h => compass.ReadingChanged -= h);

    public static IObservable<CompassChangedEventArgs> ObserveReadingChangedOnCurrentContext(this ICompass compass) =>
        compass.ObserveReadingChanged().ObserveOn(SynchronizationContext.Current!);

    // Gyroscope

    public static IObservable<GyroscopeChangedEventArgs> ObserveReadingChanged(this IGyroscope gyroscope) =>
        Observable.FromEvent<EventHandler<GyroscopeChangedEventArgs>, GyroscopeChangedEventArgs>(h => (_, e) => h(e), h => gyroscope.ReadingChanged += h, h => gyroscope.ReadingChanged -= h);

    public static IObservable<GyroscopeChangedEventArgs> ObserveReadingChangedOnCurrentContext(this IGyroscope gyroscope) =>
        gyroscope.ObserveReadingChanged().ObserveOn(SynchronizationContext.Current!);

    // Magnetometer

    public static IObservable<MagnetometerChangedEventArgs> ObserveReadingChanged(this IMagnetometer magnetometer) =>
        Observable.FromEvent<EventHandler<MagnetometerChangedEventArgs>, MagnetometerChangedEventArgs>(h => (_, e) => h(e), h => magnetometer.ReadingChanged += h, h => magnetometer.ReadingChanged -= h);

    public static IObservable<MagnetometerChangedEventArgs> ObserveReadingChangedOnCurrentContext(this IMagnetometer magnetometer) =>
        magnetometer.ObserveReadingChanged().ObserveOn(SynchronizationContext.Current!);

    // OrientationSensor

    public static IObservable<OrientationSensorChangedEventArgs> ObserveReadingChanged(this IOrientationSensor orientation) =>
        Observable.FromEvent<EventHandler<OrientationSensorChangedEventArgs>, OrientationSensorChangedEventArgs>(h => (_, e) => h(e), h => orientation.ReadingChanged += h, h => orientation.ReadingChanged -= h);

    public static IObservable<OrientationSensorChangedEventArgs> ObserveReadingChangedOnCurrentContext(this IOrientationSensor orientation) =>
        orientation.ObserveReadingChanged().ObserveOn(SynchronizationContext.Current!);
}
