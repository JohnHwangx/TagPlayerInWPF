using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ProgressBar
{
    public abstract class BaseProgressable : IProgressable
    {
        protected void RaiseProgressChanged(ProgressChangedEventArgs eventArgs)
        {
            ProgressChangedEvent?.Invoke(this, eventArgs);
        }

        public IEnumerable<string> Steps { get; protected set; }

        public Action<IProgressable> WorkCompleted { get; set ; }

        public virtual bool IsCanceled { get; protected set; }

        public virtual bool IsCompleted { get; protected set; }

        public bool IsEnd { get; private set; }

        public event Action<IProgressable, ProgressChangedEventArgs> ProgressChangedEvent;

        public virtual void Cancel()
        {
            IsCanceled = true;
        }

        public abstract void DoWork();

        public void Start()
        {
            IsCanceled = false;
            IsCompleted = false;
            DoWork();
            IsEnd = true;
            if (!IsCanceled)
            {
                IsCompleted = true;
            }
        }
    }
}
