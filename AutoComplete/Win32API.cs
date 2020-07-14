using System;
using System.Runtime.InteropServices;

namespace AutoComplete
{
    public class Win32API
    {
        #region Import windows API

        /// <summary>
        /// Install hook.
        /// </summary>
        /// <returns>The handle of this hook.</returns>
        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowsHookEx(WH_CODE idHook, HookProc lpfn, IntPtr pInstance, uint threadId);

        /// <summary>
        /// Uninstall hook.
        /// </summary>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(IntPtr pHookHandle);

        /// <summary>
        /// Pass the hook to the next entity. Invoke it when you finish with the hook processing.
        /// </summary>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(IntPtr pHookHandle, int nCodem, int wParam, IntPtr lParam);

        /// <summary>
        /// Get current thread id.
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetCurrentThreadId();

        /// <summary>
        /// Retrieves the status of the specified virtual key. 
        /// The status specifies whether the key is up, down, or toggled.
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns>
        ///     The return value specifies the status of the specified virtual key, as follows:
        ///     If the high-order bit is 1, the key is down; otherwise, it is up.
        ///     If the low-order bit is 1, the key is toggled.
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetKeyState(int keyCode);

        #region unused api
        /* /// <summary>
        /// Get the handle of module.
        /// </summary>
        /// <param name="lpModuleName"></param>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// Get windows thread process id of a certain handle.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="ID"></param>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        /// <summary>
        /// Copies the status of the 256 virtual keys to the specified buffer.
        /// </summary>
        /// <param name="pbKeyState"></param>
        /// <returns>0 if it fails, non-zero integer otherwise.</returns>
        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] pbKeyState);*/
        #endregion

        #endregion
    }
}
