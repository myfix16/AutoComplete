using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    #region 定义结构

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
        /// 底层键盘钩子
        /// </summary>
        WH_KEYBOARD_LL = 13,

        /// <summary>
        /// 底层鼠标钩子
        /// </summary>
        WH_MOUSE_LL = 14
    }

    public enum WM_MOUSE : int
    {
        /// <summary>
        /// 鼠标开始
        /// </summary>
        WM_MOUSEFIRST = 0x200,

        /// <summary>
        /// 鼠标移动
        /// </summary>
        WM_MOUSEMOVE = 0x200,

        /// <summary>
        /// 左键按下
        /// </summary>
        WM_LBUTTONDOWN = 0x201,

        /// <summary>
        /// 左键释放
        /// </summary>
        WM_LBUTTONUP = 0x202,

        /// <summary>
        /// 左键双击
        /// </summary>
        WM_LBUTTONDBLCLK = 0x203,

        /// <summary>
        /// 右键按下
        /// </summary>
        WM_RBUTTONDOWN = 0x204,

        /// <summary>
        /// 右键释放
        /// </summary>
        WM_RBUTTONUP = 0x205,

        /// <summary>
        /// 右键双击
        /// </summary>
        WM_RBUTTONDBLCLK = 0x206,

        /// <summary>
        /// 中键按下
        /// </summary>
        WM_MBUTTONDOWN = 0x207,

        /// <summary>
        /// 中键释放
        /// </summary>
        WM_MBUTTONUP = 0x208,

        /// <summary>
        /// 中键双击
        /// </summary>
        WM_MBUTTONDBLCLK = 0x209,

        /// <summary>
        /// 滚轮滚动
        /// </summary>
        /// <remarks>WINNT4.0以上才支持此消息</remarks>
        WM_MOUSEWHEEL = 0x020A
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

    public enum VK_CODE : int
    {
        VK_LBUTTON = 0x01,
        VK_RBUTTON = 0x02,
        VK_SHIFT = 0x10,
        VK_CONTROL = 0x11,
        VK_MENU = 0x12,//ALT
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
        VK_RMENU = 0xA5
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
    public struct POINT
    {
        public int X;
        public int Y;
    }

    /// <summary>
    /// 鼠标钩子事件结构定义
    /// </summary>
    /// <remarks>详细说明请参考MSDN中关于 MSLLHOOKSTRUCT 的说明</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseHookStruct
    {
        public POINT Point;
        public uint hwnd;
        public uint wHitTestCode;
        public uint dwExtraInfo;
    }

    #endregion
}
