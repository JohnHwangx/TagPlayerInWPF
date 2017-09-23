using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ProgressBar
{
    public static class WindowUtils
    {
        [DllImport("user32.dll",EntryPoint= "GetActiveWindow")]
        public static extern IntPtr GetActiveWindow();
        public static void SetParentWindow(Window childWindow)
        {
            var activeWindow = GetActiveWindow();
            if (activeWindow==IntPtr.Zero)
            {
                var process = Process.GetCurrentProcess();
                activeWindow = process.MainWindowHandle;
            }
            var windowInteropHelper = new WindowInteropHelper(childWindow)
            {
                Owner = activeWindow
            };
        }
    }
}