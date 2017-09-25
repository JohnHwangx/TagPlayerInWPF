using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProgressBar
{
    public static class ProgressRunner
    {
        public static void Run(IProgressable progressable)
        {
            Run<ProgressView>(progressable);
        }

        private static void Run<T>(IProgressable progress) where T : IProgressView
        {
            IProgressView view =new ProgressView();
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