using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ProgressBar
{
    public interface IProgressable
    {
        event Action<IProgressable, ProgressChangedEventArgs> ProgressChangedEvent;
        IEnumerable<string> Steps { get; }
        Action<IProgressable> WorkCompleted { get; set; }
        bool IsCanceled { get; }
        bool IsEnd { get; }
        bool IsCompleted { get; }
        void Start();
        void DoWork();
        void Cancel();
    }
}
