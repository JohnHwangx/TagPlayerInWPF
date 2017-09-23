using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;
using Timer = System.Timers.Timer;
using Visibility = System.Windows.Visibility;

namespace ProgressBar
{
    public class ProgressViewModel:NotificationObject
    {
        public ProgressViewModel()
        {
            Max = 100;
        }
        private long? _lastProgressTicks;
        public IProgressable Progressable { get; private set; }
        private Timer Timer { get; set; }
        private DateTime StartTime { get; set; }

        public Visibility StepVisibility
        {
            get { return StepInfoList != null && StepInfoList.Count > 0 ? Visibility.Visible : Visibility.Collapsed; }
        }

        private List<StepInfo> _stepInfoList;

        public List<StepInfo> StepInfoList
        {
            get { return _stepInfoList; }
            set
            {
                _stepInfoList = value;
                RaisePropertyChanged("StepInfoList", "StepVisibility");
            }
        }

        private int _currentValue;

        public int CurrentValue
        {
            get { return _currentValue; }
            set
            {
                if (_currentValue!=value)
                {
                    _currentValue = value;
                    RaisePropertyChanged("CurrentValue","Tip");
                }
                
            }
        }

        public String Tip
        {
            get { return $"{(CurrentValue - Min + 1) * 100 / (Max - Min + 1)}"; }
        }

        private int _min;

        public int Min
        {
            get { return _min; }
            set
            {
                _min = value; 
                RaisePropertyChanged("Min");
            }
        }

        private int _max;

        public int Max
        {
            get { return _max; }
            set
            {
                _max = value; 
                RaisePropertyChanged("Max");
            }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value; 
                RaisePropertyChanged("Status");
            }
        }

        private string _id;

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value; 
                RaisePropertyChanged("Id");
            }
        }

        private string _userTime;

        public string UserTime
        {
            get { return _userTime; }
            set
            {
                _userTime = value; 
                RaisePropertyChanged("UserTime");
            }
        }

        private string _useTime;

        public string UseTime
        {
            get { return _useTime; }
            set
            {
                _useTime = value; 
                RaisePropertyChanged("UseTime");
            }
        }

        internal void InitStep(List<StepInfo> stepInfos)
        {
            StepInfoList = stepInfos;
            ProgressStep(0);
        }

        private void ProgressStep(int stepIndex)
        {
            if (StepInfoList!=null&&stepIndex<StepInfoList.Count)
            {
                for (int i = 0; i < stepIndex; i++)
                {
                    StepInfoList[i].Status = StepStatus.Done;
                }
                StepInfoList[stepIndex].Status = StepStatus.Processing;
            }
        }

        internal void InitProgressable(IProgressable progressable)
        {
            Progressable = progressable;
            progressable.ProgressChangedEvent += Progressable_ProgressChangedEvent;
            Timer = new Timer
            {
                Interval = 100,
                Enabled = true,
            };
            StartTime = DateTime.Now;
            Timer.AutoReset = true;
            Timer.Start();
        }

        private void Progressable_ProgressChangedEvent(object sender, ProgressChangedEventArgs e)
        {
            if (_lastProgressTicks==null)
            {
                _lastProgressTicks = DateTime.Now.Ticks;
                HandleProgressChangedEvent(e);
            }
            else
            {
                if (DateTime.Now.Ticks-_lastProgressTicks.Value>=500000)
                {
                    _lastProgressTicks = DateTime.Now.Ticks;
                    HandleProgressChangedEvent(e);
                }
            }
        }

        private void HandleProgressChangedEvent(ProgressChangedEventArgs e)
        {
            if (e!=null)
            {
                CurrentValue = e.ProgressPercentage;
                var statusWithStep = e.UserState as StatusWithStep;
                string status;
                var id = string.Empty;
                if (statusWithStep!=null)
                {
                    ProgressStep(statusWithStep.StepIndex);
                    status = statusWithStep.Status;
                    id = statusWithStep.Id;
                }
                else
                {
                    status = e.UserState as string;
                }

                if (!string.IsNullOrEmpty(status))
                {
                    Status = status;
                }
                if (!string.IsNullOrEmpty(id))
                {
                    Id = id;
                }
                UseTime = (DateTime.Now - StartTime).ToString("hh\\:mm\\:ss");
                DoEvent();
            }
        }

        private void DoEvent()
        {
            DispatcherFrame frame=new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        private object ExitFrame(object f)
        {
            ((DispatcherFrame) f).Continue = false;
            return null;
        }
    }
}