using System;
using System.Runtime.InteropServices;
using System.Text;
using static AutoComplete.Win32API;

namespace AutoComplete
{
    public class MessageHook
    {
        internal IntPtr hIMC;

        internal IntPtr handle;

        internal bool enabled = true;

        /// <summary>
        /// Message hook handle.
        /// </summary>
        private IntPtr messageHookHandle = IntPtr.Zero;

        /// <summary>
        /// Instance of message hook delegate.
        /// </summary>
        private HookProc messageHookProcedure;

        /// <summary>
        /// The method triggered when input is detected.
        /// </summary>
        internal Action<IntPtr> processAction = (lParam) => { };

        public MessageHook() { }

        public MessageHook(IntPtr handle)
        {
            this.handle = handle;
            hIMC = ImmGetContext(handle);
        }

        /// <summary>
        /// Install hook. 
        /// </summary>
        /// <returns>Whether hook is successfully installed.</returns>
        public bool InstallHook()
        {
            if (messageHookHandle == IntPtr.Zero)
            {
                messageHookProcedure = new HookProc(MessageHookProc);
                messageHookHandle = Win32API.SetWindowsHookEx((int)WH_CODE.WH_GETMESSAGE, messageHookProcedure,
                                                              IntPtr.Zero, Win32API.GetCurrentThreadId());
            }

            return messageHookHandle != IntPtr.Zero;
        }

        /// <summary>
        /// Uninstall the hook.
        /// </summary>
        /// <returns>Whether the hook is successfully uninstalled.</returns>
        public bool UnInstallHook() => UnhookWindowsHookEx(messageHookHandle);

        private string GetInputContent(IntPtr lParam)
        {
            var m = Marshal.PtrToStructure<MSG>(lParam);

            if (m.message == (uint)WM_IMM.WM_IME_COMPOSITION)
            {
                var res = m.wParam;
                string text = CurrentCompStr(this.handle);
                if (!string.IsNullOrEmpty(text))
                {
                    return (text);
                }
            }
            else if (m.message == (uint)WM_IMM.WM_CHAR)
            {
                char inputchar = (char)m.wParam;
                if (inputchar > 31 && inputchar < 127)
                {
                    return (inputchar.ToString());
                }
            }

            return string.Empty;
        }

        private string CurrentCompStr(IntPtr handle)
        {
            try
            {
                int strLen = ImmGetCompositionStringW(hIMC, (int)IMECompositionValue.GCS_RESULTSTR, null, 0);
                if (strLen > 0)
                {
                    var buffer = new byte[strLen];
                    ImmGetCompositionStringW(hIMC, (int)IMECompositionValue.GCS_RESULTSTR, buffer, strLen);
                    return Encoding.Unicode.GetString(buffer);
                }
                else
                {
                    return string.Empty;
                }
            }
            finally
            {
                ImmReleaseContext(handle, hIMC);
            }
        }

        private int MessageHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode == (int)HC_CODE.HC_ACTION && enabled)
            {
                processAction(lParam);
            }

            return CallNextHookEx(messageHookHandle, nCode, wParam, lParam);
        }
    }
}
