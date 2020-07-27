using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    public static class ExtendMethods
    {
        public static IntPtr FindWindowEx(this IntPtr intptr, uint hwndChildAfter, string lpszClass, string lpszWindow)
        {
            return Win32API.FindWindowEx(intptr, hwndChildAfter, lpszClass, lpszWindow);
        }
    }
}
