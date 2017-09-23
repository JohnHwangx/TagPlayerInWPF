using ProgressBar;
using System;
using System.ComponentModel;
using TagPlayer.ViewModels;

namespace TagPlayer.ProgressBar
{
    public class LoadProgressBar: BaseProgressable
    {
        private readonly TagsPanelViewModel _command;
        public LoadProgressBar(TagsPanelViewModel command, Action<IProgressable> workCompleted)
        {
            _command = command;
            WorkCompleted = workCompleted;
        }
        public override void DoWork()
        {
            _command.DoWithProgressable(RaiseProgressChanged);
        }

        private void RaiseProgressChanged(int stepIndex, int progress, string status)
        {
            RaiseProgressChanged(new ProgressChangedEventArgs(progress, new StatusWithStep { StepIndex = stepIndex, Status = status }));
        }
    }
}