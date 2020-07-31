using System;
using System.Runtime.InteropServices;

namespace AutoComplete
{
    #region Message

    /// <summary>
    /// Windows hook id
    /// </summary>
    public enum WH_CODE : int
    {
        WH_JOURNALRECORD = 0,
        WH_JOURNALPLAYBACK = 1,

        /// <summary>
        /// 进程钩子
        /// </summary>
        WH_KEYBOARD = 2,

        WH_GETMESSAGE = 3,
        WH_CALLWNDPROC = 4,
        WH_CBT = 5,
        WH_SYSMSGFILTER = 6,
        /// <summary>
        /// 进程钩子
        /// </summary>
        WH_MOUSE = 7,
        WH_HARDWARE = 8,
        WH_DEBUG = 9,
        WH_SHELL = 10,
        WH_FOREGROUNDIDLE = 11,
        WH_CALLWNDPROCRET = 12,
        /// <summary>
        /// Global键盘钩子
        /// </summary>
        WH_KEYBOARD_LL = 13,

        /// <summary>
        /// Global鼠标钩子
        /// </summary>
        WH_MOUSE_LL = 14
    }

    public enum WM_KEYBOARD : int
    {
        /// <summary>
        /// 非系统按键按下
        /// </summary>
        WM_KEYDOWN = 0x100,

        /// <summary>
        /// 非系统按键释放
        /// </summary>
        WM_KEYUP = 0x101,

        /// <summary>
        /// 系统按键按下
        /// </summary>
        WM_SYSKEYDOWN = 0x104,

        /// <summary>
        /// 系统按键释放
        /// </summary>
        WM_SYSKEYUP = 0x105
    }

    /// <summary>
    /// Messages from IMM.
    /// </summary>
    public enum WM_IMM : int
    {
        WM_INPUTLANGCHANGE = 0x51,
        WM_KEYUP = 0x101,
        WM_CHAR = 0x102,
        WM_CONVERTREQUESTEX = 0x108,
        WM_IME_STARTCOMPOSITION = 0x10D,
        WM_IME_ENDCOMPOSITION = 0x10E,
        WM_IME_COMPOSITION = 0x10F,
        WM_IME_SETCONTEXT = 0x281,
        WM_IME_NOTIFY = 0x282,
        WM_IME_CONTROL = 0x283,
        WM_IME_COMPOSITIONFULL = 0x284,
        WM_IME_SELECT = 0x285,
        WM_IME_CHAR = 0x286,
        WM_IME_REQUEST = 0x0288,
        WM_IME_KEYDOWN = 0x290,
        WM_IME_KEYUP = 0x291,
    }

    public enum HC_CODE : int
    {
        HC_ACTION = 0,
        HC_GETNEXT = 1,
        HC_SKIP = 2,
        HC_NOREMOVE = 3,
        HC_NOREM = 3,
        HC_SYSMODALON = 4,
        HC_SYSMODALOFF = 5
    }

    /// <summary>
    /// Virtual key code.
    /// </summary>
    public enum VK_CODE : int
    {
        VK_LBUTTON = 0x01,
        VK_RBUTTON = 0x02,
        VK_SHIFT = 0x10,
        VK_CONTROL = 0x11,
        VK_MENU = 0x12, //ALT
        VK_C = 0x43,
        VK_V = 0x56,
        VK_X = 0x58,
        VK_Y = 0x59,
        VK_Z = 0x5A,
        VK_APPS = 0x5D,
        VK_LSHIFT = 0xA0,
        VK_RSHIFT = 0xA1,
        VK_LCONTROL = 0xA2,
        VK_RCONTROL = 0xA3,
        VK_LMENU = 0xA4,
        VK_RMENU = 0xA5,
        VK_PROCESSKEY = 0xE5,
    }

    /// <summary>
    /// 键盘钩子事件结构定义
    /// </summary>
    /// <remarks>详细说明请参考MSDN中关于 KBDLLHOOKSTRUCT 的说明</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardHookStruct
    {
        /// <summary>
        /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
        /// </summary>
        public uint VKCode;

        /// <summary>
        /// Specifies a hardware scan code for the key.
        /// </summary>
        public uint ScanCode;

        /// <summary>
        /// Specifies the extended-key flag, event-injected flag, context code, 
        /// and transition-state flag. This member is specified as follows. 
        /// An application can use the following values to test the keystroke flags. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// Specifies the time stamp for this message. 
        /// </summary>
        public uint Time;

        /// <summary>
        /// Specifies extra information associated with the message. 
        /// </summary>
        public uint ExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        /// <summary>
        /// A handle to the window whose window procedure receives the message. 
        /// This member is null when the message is a thread message.
        /// </summary>
        public IntPtr hwnd;
        /// <summary>
        /// The message identifier. Applications can only use the low word; 
        /// the high word is reserved by the system.
        /// </summary>
        public uint message;
        /// <summary>
        /// Additional information about the message. 
        /// The exact meaning depends on the value of the message member.
        /// </summary>
        public int wParam;
        /// <summary>
        /// Additional information about the message. 
        /// The exact meaning depends on the value of the message member.
        /// </summary>
        public IntPtr lParam;
        /// <summary>
        /// The time at which the message was posted.
        /// </summary>
        public uint time;
        /// <summary>
        /// The cursor position, in screen coordinates, when the message was posted.
        /// </summary>
        public POINT pt;
        public uint lPrivate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        int x, y;
    }

    #endregion

    #region IME Status

    public enum IMEConversionMode
    {
        IME_CMODE_FULLSHAPE = 0x8,
        IME_CHOTKEY_SHAPE_TOGGLE = 0x11,
    }

    public enum IMECompositionValue
    {
        GCS_RESULTSTR = 0x0800,
        GCS_COMPSTR = 0x0008,
    }

    #endregion
}
