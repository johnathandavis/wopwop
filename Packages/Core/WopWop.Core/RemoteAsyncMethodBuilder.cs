using System.Diagnostics;
using System.Runtime.CompilerServices;
using WopWop.Core.Extensions;

namespace WopWop.Core;

public class RemoteAsyncMethodBuilder<T>
{
    #region fields

    Exception? _exception;
    bool _hasResult;
    SpinLock _lock;
    T? _result;
    TaskCompletionSource<T>? _source;

    #endregion

    #region properties

    public RemoteTask<T> Task
    {
        get
        {
            var lockTaken = false;
            try
            {
                _lock.Enter(ref lockTaken);
                if (_exception is not null)
                    return new RemoteTask<T>(ValueTask.FromException<T>(_exception));
                if (_hasResult)
                    return new RemoteTask<T>(ValueTask.FromResult(_result!));
                return new RemoteTask<T>(
                    new ValueTask<T>(
                        (_source ??= new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously))
                        .Task
                    )
                );
            }
            finally
            {
                if (lockTaken)
                    _lock.Exit();
            }
        }
    }

    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter,
        ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        InterceptOnCompleted(ref stateMachine);
        awaiter.OnCompleted(stateMachine.MoveNext);
    }

    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter,
        ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        InterceptOnCompleted(ref stateMachine);
        awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
    }

    private void InterceptOnCompleted<TStateMachine>(
        ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine
    {
        var ri = stateMachine.GetRuntimeInfo();
        var str = stateMachine.GetMachineStructure();
        var state = stateMachine.GetVariableState();
        Console.WriteLine(str.Tree);
    }

    #endregion

    #region methods

    public static RemoteAsyncMethodBuilder<T> Create() => new()
    {
        _lock = new SpinLock(Debugger.IsAttached)
    };

    public void SetException(Exception exception)
    {
        var lockTaken = false;
        try
        {
            _lock.Enter(ref lockTaken);
            if (Volatile.Read(ref _source) is { } source)
            {
                source.TrySetException(exception);
            }
            else
            {
                _exception = exception;
            }
        }
        finally
        {
            if (lockTaken)
                _lock.Exit();
        }
    }

    public void SetResult(T result)
    {
        var lockTaken = false;
        try
        {
            _lock.Enter(ref lockTaken);
            if (Volatile.Read(ref _source) is { } source)
            {
                source.TrySetResult(result);
            }
            else
            {
                _result = result;
                _hasResult = true;
            }
        }
        finally
        {
            if (lockTaken)
                _lock.Exit();
        }
    }

    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {
    }

    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine => stateMachine.MoveNext();

    #endregion
}