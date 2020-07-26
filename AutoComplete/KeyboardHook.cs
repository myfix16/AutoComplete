using System;
using System.Windows.Forms;

namespace AutoComplete
{
    /// <summary>
    /// Hook delegate.
    /// </summary>
    public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

    public class KeyboardHook
    {
        /// <summary>
        /// The method triggered when some key is pressed.
        /// </summary>
        internal Action processAction = () => { };

        //private bool enabled = true;

        /// <summary>
        /// Keyboard hook handle.
        /// </summary>
        private IntPtr mKeyboardHook = IntPtr.Zero;

        /// <summary>
        /// Instance of keyboard hook delegate. It will be invoked when some key is pressed.
        /// </summary>
        private HookProc mKeyboardHookProcedure;

        /// <summary>
        /// Judge whether a key is pressed.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>Whether a specific key is pressed.</returns>
        internal static bool IsKeyDown(Keys keys) => (Win32API.GetKeyState((int)keys) & 0x8000) == 0x8000;

        /// <summary>
        /// The processing function of keyboard hook.
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam">
        ///     Specifies whether the message is sent by the current process. In this case, it's key id.
        ///     If the message is sent by the current process, it is nonzero; otherwise, it is NULL.
        /// </param>
        /// <param name="lParam">
        ///     A pointer to a CWPRETSTRUCT structure that contains details about the message.
        /// </param>
        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode == (int)HC_CODE.HC_ACTION)
            {
                processAction();
                #region enable or disable
                /* if (IsKeyDown(Keys.ShiftKey) && IsKeyDown(Keys.ControlKey) && IsKeyDown(Keys.Menu)
                    && IsKeyDown(keyData) && IsKeyDown(Keys.O))
                {
                    enabled = !enabled;
                }

                if (enabled)
                    processAction(keyData);*/
                # endregion
            }

            return Win32API.CallNextHookEx(mKeyboardHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// Install hook. 
        /// </summary>
        /// <returns>Whether hook is successfully installed.</returns>
        public bool InstallHook()
        {
            if (mKeyboardHook == IntPtr.Zero)
            {
                mKeyboardHookProcedure = new HookProc(KeyboardHookProc);
                mKeyboardHook = Win32API.SetWindowsHookEx(WH_CODE.WH_KEYBOARD, mKeyboardHookProcedure, IntPtr.Zero,
                                                          Win32API.GetCurrentThreadId());
            }

            return mKeyboardHook != IntPtr.Zero;
        }

        /// <summary>
        /// Uninstall the hook.
        /// </summary>
        /// <returns>Whether the hook is successfully uninstalled.</returns>
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
