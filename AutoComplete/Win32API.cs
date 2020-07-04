using System;
using System.Runtime.InteropServices;

namespace AutoComplete
{
    public class Win32API
    {
        #region 导入API

        /// <summary>
        /// 安装钩子
        /// </summary>
        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowsHookEx(WH_CODE idHook, HookProc lpfn, IntPtr pInstance, uint threadId);

        /// <summary>
        /// 卸载钩子
        /// </summary>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(IntPtr pHookHandle);

        /// <summary>
        /// 传递钩子
        /// </summary>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(IntPtr pHookHandle, int nCodem, int wParam, IntPtr lParam);

        /// <summary>
        /// 获取全部按键状态
        /// </summary>
        /// <param name="pbKeyState"></param>
        /// <returns>非0表示成功</returns>
        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        /// <summary>
        /// 获取程序集模块的句柄
        /// </summary>
        /// <param name="lpModuleName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// 获取指定句柄的线程ID
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        /// <summary>
        /// 获取当前进程中的当前线程ID
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetCurrentThreadId();

        /// <summary>
        /// 获取指定按键状态
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetKeyState(int keyCode);

        #endregion
    }
}
