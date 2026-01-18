using UniRx;
using CompositeDisposable = UniRx.CompositeDisposable;

namespace VSystem.External.Extensions.UniRx;

public static class UniRxExtension
{
    public static CompositeDisposable Refresh(this CompositeDisposable composite)
    {
        composite?.Dispose();

        return new CompositeDisposable();
    }

    public static IDisposable LoopedTimer(float initialDelay, float interval, Action callback)
    {
        return Observable
            .Timer(TimeSpan.FromSeconds(initialDelay), TimeSpan.FromSeconds(interval))
            .Subscribe(_ => callback?.Invoke());
    }

    public static IDisposable CountedTimer(
        float initialDelay,
        float interval,
        int repeatingCount,
        Action<int> callback,
        Action totalCompleteCallback = null)
    {
        int workIndex = 0;

        return Observable
            .Timer(TimeSpan.FromSeconds(initialDelay), TimeSpan.FromSeconds(interval))
            .Take(repeatingCount)
            .Subscribe(_ =>
            {
                callback?.Invoke(workIndex);
                workIndex++;
            }, () => { totalCompleteCallback?.Invoke(); });
    }

    public static IDisposable Delay(float delay, Action callback, bool withNativeTimescale = true)
    {
        return Observable
            .Timer(TimeSpan.FromSeconds(delay), TimeSpan.FromSeconds(0))
            .Take(1)
            .Subscribe(_ => callback?.Invoke());
    }
}