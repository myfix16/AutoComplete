using System;
using System.Windows.Forms;

namespace AutoComplete
{
    /// <summary>
    /// 钩子委托声明
    /// </summary>
    public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

    public delegate void ProcessAction(Keys keyData);

    public class Hook
    {
        #region 私有变量

        internal ProcessAction processAction = (k) => { };

        private bool enabled = true;

        /// <summary>
        /// 键盘钩子句柄
        /// </summary>
        private IntPtr mKeyboardHook = IntPtr.Zero;
        /// <summary>
        /// 键盘钩子委托实例
        /// </summary>
        private HookProc mKeyboardHookProcedure;

        #endregion

        internal static bool IsKeyDown(Keys keys) => (Win32API.GetKeyState((int)keys) & 0x8000) == 0x8000;

        /// <summary>
        /// 键盘钩子处理函数
        /// </summary>
        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode == (int)HC_CODE.HC_ACTION)
            {
                Keys keyData = (Keys)wParam;
                if (IsKeyDown(Keys.ShiftKey) && IsKeyDown(Keys.ControlKey) && IsKeyDown(Keys.Menu)
                    && IsKeyDown(keyData) && IsKeyDown(Keys.O))
                {
                    enabled = !enabled;
                }

                if (enabled)
                    processAction(keyData);
            }

            return Win32API.CallNextHookEx(mKeyboardHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// 安装钩子
        /// </summary>
        /// <returns></returns>
        public bool InstallHook()
        {
            if (mKeyboardHook == IntPtr.Zero)
            {
                mKeyboardHookProcedure = new HookProc(KeyboardHookProc);
                mKeyboardHook = Win32API.SetWindowsHookEx(WH_CODE.WH_KEYBOARD,
                                                          mKeyboardHookProcedure,
                                                          IntPtr.Zero,
                                                          Win32API.GetCurrentThreadId());
            }

            return mKeyboardHook != IntPtr.Zero;
        }

        /// <summary>
        /// 卸载钩子
        /// </summary>
        /// <returns></returns>
        public bool UnInstallHook()
        {
            bool result = true;
            if (mKeyboardHook != IntPtr.Zero)
            {
                result = Win32API.UnhookWindowsHookEx(mKeyboardHook) && result;
                mKeyboardHook = IntPtr.Zero;
            }

            return result;
        }
    }

}
