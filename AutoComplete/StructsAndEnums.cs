using System.Runtime.InteropServices;

namespace AutoComplete
{
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

}
