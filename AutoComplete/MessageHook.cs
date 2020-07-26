using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoComplete
{
    public class MessageHook
    {        
        /// <summary>
        /// Message hook handle.
        /// </summary>
        private IntPtr messageHook = IntPtr.Zero;

        /// <summary>
        /// Instance of message hook delegate.
        /// </summary>
        private HookProc messageHookProcedure;

        /// <summary>
        /// Install hook. 
        /// </summary>
        /// <returns>Whether hook is successfully installed.</returns>
        public bool InstallHook()
        {
            if (messageHook == IntPtr.Zero)
            {
                messageHookProcedure = new HookProc(MessageHookProc);
                messageHook = Win32API.SetWindowsHookEx(WH_CODE.WH_GETMESSAGE, messageHookProcedure, IntPtr.Zero,
                                                        Win32API.GetCurrentThreadId());
            }

            return messageHook != IntPtr.Zero;
        }

        /// <summary>
        /// Uninstall the hook.
        /// </summary>
        /// <returns>Whether the hook is successfully uninstalled.</returns>
        public bool UnInstallHook()
        {
            bool result = true;
            if (messageHook != IntPtr.Zero)
            {
                result = Win32API.UnhookWindowsHookEx(messageHook) && result;
                messageHook = IntPtr.Zero;
            }

            return result;
        }

        private int MessageHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode == (int)HC_CODE.HC_ACTION)
            {
                MessageBox.Show(lParam.ToString());
                //MessageBox.Show(wParam.ToString());
            }
            return Win32API.CallNextHookEx(messageHook, nCode, wParam, lParam);
        }
    }
}
