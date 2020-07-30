using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AutoComplete
{
    internal class ConversionStatusChecker
    {
        /// <summary>
        /// See whether IME is in full shape mode.
        /// </summary>
        /// <param name="HIme">A handle to current input window.</param>
        /// <returns>true if IME is in full shape mode, or false otherwise.</returns>
        internal static bool IsFullShape(IntPtr HIme)
        {
            int iMode = 0;
            int iSentence = 0;
            _ = Win32API.ImmGetConversionStatus(HIme, ref iMode, ref iSentence);
            return (iMode & (int)IMEConversionMode.IME_CMODE_FULLSHAPE) > 0;
        }
    }
}