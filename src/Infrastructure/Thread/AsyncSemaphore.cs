﻿using Ardalis.GuardClauses;

namespace Infrastructure.Thread;

/// <summary>
///     异步信号量
/// </summary>
public class AsyncSemaphore
{
    private static readonly Task Completed = Task.FromResult(true);
    private readonly Queue<TaskCompletionSource<bool>> _waiters = new();
    private int _currentCount;

    /// <summary>
    ///     初始化一个<see cref="AsyncSemaphore" />类型的新实例
    /// </summary>
    public AsyncSemaphore(int initialCount)
    {
        Guard.Against.NegativeOrZero(initialCount, nameof(initialCount));
        _currentCount = initialCount;
    }

    /// <summary>
    ///     等待同步
    /// </summary>
    public Task WaitAsync()
    {
        lock (_waiters)
        {
            if (_currentCount > 0)
            {
                --_currentCount;
                return Completed;
            }

            var waiter = new TaskCompletionSource<bool>();
            _waiters.Enqueue(waiter);
            return waiter.Task;
        }
    }

    /// <summary>
    ///     释放
    /// </summary>
    public void Release()
    {
        TaskCompletionSource<bool>? toRelease = null;
        lock (_waiters)
        {
            if (_waiters.Count > 0)
                toRelease = _waiters.Dequeue();
            else
                ++_currentCount;
        }

        toRelease?.SetResult(true);
    }
}