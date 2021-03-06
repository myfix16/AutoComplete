﻿using System;
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

        [DllImport("imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hwnd);

        /// <summary>
        /// Retrieves the current conversion status.
        /// </summary>
        /// <param name="himc">Handle to the input context for which to retrieve status information.</param>
        /// <param name="lpdw">
        ///     Pointer to a variable in which the function retrieves a combination of conversion mode values. 
        ///     For more information, see IME Conversion Mode Values.
        /// </param>
        /// <param name="lpdw2">
        ///     Pointer to a variable in which the function retrieves a sentence mode value. 
        ///     For more information, see IME Sentence Mode Values.
        /// </param>
        /// <returns>A nonzero value if successful, or 0 otherwise.</returns>
        [DllImport("imm32.dll")]
        public static extern bool ImmGetConversionStatus(IntPtr himc, ref int lpdw, ref int lpdw2);

        #endregion
    }
}
