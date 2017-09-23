using System;
using System.Collections.Generic;

namespace ProgressBar
{
    public interface IProgressView
    {
        void InitStep(IEnumerable<StepInfo> stepList);
        void InitProgressable(IProgressable progress);
        void StartShow(Action showed = null);
        void EndShow(bool isCatchException, Action endShowAction);
    }

    public class StepInfo : NotificationObject
    {
        private readonly IList<StepInfo> _owner;

        public StepInfo(IList<StepInfo> owner)
        {
            _owner = owner;
        }

        private const int StepContainerWidth = 420;
        private const int ArrowWidth = 40;
        private const int ArrowMarginLeft = 5;
        private const int MaxCount = 8;

        private StepStatus _status;

        public StepStatus Status
        {
            get { return _status; }
            set
            {
                _status = value; 
                RaisePropertyChanged("Status");
            }
        }

        private string _shortDiscription;

        public string ShortDescription
        {
            get { return _shortDiscription; }
            set
            {
                _shortDiscription = value; 
                RaisePropertyChanged("ShortDescription");
            }
        }


    }

    public enum StepStatus
    {
        NotStarted,
        Processing,
        Done,
    }
}