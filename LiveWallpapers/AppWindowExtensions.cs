using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace LiveWallpapers
{
    internal static class AppWindowExtensions
    {
        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(IntPtr windowHandle,
                                                       uint Msg,
                                                       IntPtr wParam,
                                                       IntPtr lParam,
                                                       SendMessageTimeoutFlags flags,
                                                       uint timeout,
                                                       out IntPtr result);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr hWndChildAfter, string className, string windowTitle);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, UInt32 uFlags);

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int smIndex);


        public static AppWindow GetAppWindowFromWPFWindow(this Window wpfWindow)
        {
            // Get the HWND of the top level WPF window.
            IntPtr hwnd = new WindowInteropHelper(wpfWindow).EnsureHandle();

            // Get the WindowId from the HWND.
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);

            // Return AppWindow from the WindowId.
            return AppWindow.GetFromWindowId(windowId);
        }

        /// <summary>
        /// Opens a native Windows FolderPicker
        /// </summary>
        /// <param name="folderPicker">Change FolderPicker behaviour</param>
        /// <returns></returns>
        public static async Task<StorageFolder> OpenFolderPicker(FolderPicker folderPicker = null)
        {
            if (folderPicker == null)
                folderPicker = new();

            MainWindow window = (LiveWallpapers.MainWindow)App.Current.MainWindow;
            IntPtr hwnd = new WindowInteropHelper(window).EnsureHandle();
            InitializeWithWindow.Initialize(folderPicker, hwnd);

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            return folder;
        }

        public static void RenderAppAsBackground(Window window)
        {
            IntPtr windowHandle = new WindowInteropHelper(window).Handle;

            IntPtr result = IntPtr.Zero;
            SendMessageTimeout(windowHandle,
                       0x052C,
                       new IntPtr(0),
                       IntPtr.Zero,
                       SendMessageTimeoutFlags.SMTO_NORMAL,
                       1000,
                       out result);

            IntPtr workerw = IntPtr.Zero;
            EnumWindows(new EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", "");

                if (p != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerw = FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", "");
                }

                return true;
            }), IntPtr.Zero);

            SetParent(windowHandle, workerw);
            SetWindowLong(windowHandle, -16, 0x80000000L); // Borderless
        }

        public static void SetWindowPosition(Window window, int x, int y)
        {
            IntPtr windowHandle = new WindowInteropHelper(window).Handle;

            int w = GetSystemMetrics(0);
            int h = GetSystemMetrics(1);
            SetWindowPos(windowHandle, IntPtr.Zero, x, y, w, h, 0x0020);
        }
    }
}
