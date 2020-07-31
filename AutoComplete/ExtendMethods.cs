using System;

namespace AutoComplete
{
    public static class ExtendMethods
    {
        public static IntPtr FindWindowEx(this IntPtr intptr, uint hwndChildAfter, string lpszClass, string lpszWindow)
            => Win32API.FindWindowEx(intptr, hwndChildAfter, lpszClass, lpszWindow);
    }
}
