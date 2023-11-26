using System.Runtime.CompilerServices;

namespace WopWop.Core;

[AsyncMethodBuilder(typeof(RemoteAsyncMethodBuilder<>))]
public readonly struct RemoteTask<T>
{
    
    readonly ValueTask<T> _valueTask;

    public RemoteTask(ValueTask<T> valueTask)
    {
        _valueTask = valueTask;
    }

    public ValueTaskAwaiter<T> GetAwaiter() => _valueTask.GetAwaiter();
}