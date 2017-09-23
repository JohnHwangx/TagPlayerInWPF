using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ProgressBar
{
    /// <summary>
    /// ProgressView.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressView : IProgressView
    {
        public ProgressViewModel ProgressViewModel
        {
            get { return DataContext as ProgressViewModel;}
            set { DataContext = value; }
        }

        public ProgressView()
        {
            ProgressViewModel=new ProgressViewModel();
            InitializeComponent();

            WindowUtils.SetParentWindow(this);

            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        public void EndShow(bool isCatchException, Action endShowAction)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Close();
                endShowAction?.Invoke();
            }));
        }

        public void InitProgressable(IProgressable progress)
        {
            ProgressViewModel.InitProgressable(progress);

            CancelButton.Click += (sender, e) =>
            {
                if (!progress.IsCompleted)
                {
                    progress.Cancel();
                }
            };

            Closed += (sender, e) =>
            {
                if (!progress.IsCompleted)
                {
                    progress.Cancel();
                }
            };
        }

        public void InitStep(IEnumerable<StepInfo> stepList)
        {
            ProgressViewModel.InitStep(stepList.ToList());
        }

        public void StartShow(Action showed = null)
        {
            Loaded += (sender, e) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    showed?.Invoke();
                }));
            };
            ShowDialog();
        }
    }
}
