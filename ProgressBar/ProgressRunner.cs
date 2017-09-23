using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProgressBar
{
    public static class ProgressRunner
    {
        public static void Run(IProgressable progressable, IList<string> steps = null)
        {
            ExternalProgressRunner.Run(progressable, steps);
        }
    }

    public static class ExternalProgressRunner
    {
        public static void Run(IProgressable progress, IEnumerable<string> steps = null)
        {
            Run<ProgressView>(progress,steps);
        }

        private static void Run<T>(IProgressable progress, IEnumerable<string> steps) where T:IProgressView
        {
            var stepInfos=new List<StepInfo>();
            if (steps!=null)
            {
                foreach (var step in steps)
                {
                    var stepInfo=new StepInfo(stepInfos){ShortDescription=step};
                    stepInfos.Add(stepInfo);
                }
            }
            else
            {
                if (progress.Steps!=null)
                {
                    foreach (var step in progress.Steps)
                    {
                        var stepInfo=new StepInfo(stepInfos){ShortDescription=step};
                        stepInfos.Add(stepInfo);
                    }
                }
            }
            Run<T>(progress, stepInfos);
        }

        private static void Run<T>(IProgressable progress, List<StepInfo> stepInfos) where T : IProgressView
        {
            IProgressView view =new ProgressView();
            view.InitStep(stepInfos);
            view.InitProgressable(progress);
            view.StartShow(() =>
            {
                try
                {
                    Application.DoEvents();
                    progress.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                view.EndShow(true,null);
            });

            progress?.WorkCompleted(progress);
        }
    }
}