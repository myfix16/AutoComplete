using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

       private string GetCultureType()
       {
          var currentInputLanguage = InputLanguage.CurrentInputLanguage;
          var cultureInfo = currentInputLanguage.Culture;
          //同 cultureInfo.IetfLanguageTag;
          return cultureInfo.Name;
       }

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
            var lResult = Win32API.CallNextHookEx(messageHook, nCode, wParam, lParam);
            var msg = (MSG)Marshal.PtrToStructure(lParam, typeof(MSG));

            //if (nCode == (int)HC_CODE.HC_ACTION && msg.message == (int)WM_IMM.WM_IME_COMPOSITION)
            //{
            //    HIMC hIMC;
            //    HWND hWnd = pmsg->hwnd;
            //    DWORD dwSize;
            //    char ch;
            //    char lpstr[20];
            //    if (pmsg->lParam & GCS_RESULTSTR)
            //    {
            //        //先获取当前正在输入的窗口的输入法句柄
            //        hIMC = ImmGetContext(hWnd);
            //        if (!hIMC)
            //        {
            //            MessageBox(NULL, "ImmGetContext", "ImmGetContext", MB_OK);
            //        }

            //        // 先将ImmGetCompositionString的获取长度设为0来获取字符串大小.
            //        dwSize = ImmGetCompositionString(hIMC, GCS_RESULTSTR, NULL, 0);

            //        // 缓冲区大小要加上字符串的NULL结束符大小,
            //        //   考虑到UNICODE
            //        dwSize += sizeof(WCHAR);

            //        memset(lpstr, 0, 20);

            //        // 再调用一次.ImmGetCompositionString获取字符串
            //        ImmGetCompositionString(hIMC, GCS_RESULTSTR, lpstr, dwSize);

            //        //现在lpstr里面即是输入的汉字了。你可以处理lpstr,当然也可以保存为文件...
            //        MessageBox(NULL, lpstr, lpstr, MB_OK);
            //        ImmReleaseContext(hWnd, hIMC);
            //    }
            //}
            return lResult;
        }
    }
}
