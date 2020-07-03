using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoComplete
{
    /// <summary>
    /// 钩子委托声明
    /// </summary>
    public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

    public class Hook
    {
        #region 私有变量

        private byte[] mKeyState = new byte[256];
        private Keys mKeyData = Keys.None; //专门用于判断按键的状态

        /// <summary>
        /// 键盘钩子句柄
        /// </summary>
        private IntPtr mKeyboardHook = IntPtr.Zero;
        /// <summary>
        /// 鼠标钩子句柄
        /// </summary>
        private IntPtr mMouseHook = IntPtr.Zero;
        /// <summary>
        /// 键盘钩子委托实例
        /// </summary>
        private HookProc mKeyboardHookProcedure;
        /// <summary>
        /// 鼠标钩子委托实例
        /// </summary>
        private HookProc mMouseHookProcedure;

        #endregion

        #region 鼠标事件

        public event MouseEventHandler OnMouseDown;
        public event MouseEventHandler OnMouseUp;
        public event MouseEventHandler OnMouseMove;

        #endregion

        #region 键盘事件

        public event KeyEventHandler OnKeyDown;
        public event KeyEventHandler OnKeyUp;

        #endregion

        public Hook()
        {
            Win32API.GetKeyboardState(mKeyState);
        }

        ~Hook()
        {
            UnInstallHook();
        }

        /// <summary>
        /// 键盘钩子处理函数
        /// </summary>
        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            // 定义为线程钩子时，wParam的值是击打的按键，与Keys里的对应按键相同
            if ((nCode == (int)HC_CODE.HC_ACTION) && (OnKeyDown != null || OnKeyUp != null))
            {
                //使用全局Hook才需要
                //KeyboardHookStruct KeyboardInfo = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

                mKeyData = (Keys)wParam;
                KeyEventArgs keyEvent = new KeyEventArgs(mKeyData);
                //这里简单的通过lParam的值的正负情况与按键的状态相关联
                if (lParam.ToInt32() > 0 && OnKeyDown != null)
                {
                    OnKeyDown(this, keyEvent);
                }
                else if (lParam.ToInt32() < 0 && OnKeyUp != null)
                {
                    OnKeyUp(this, keyEvent);
                }
            }

            return Win32API.CallNextHookEx(mKeyboardHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// 鼠标钩子处理函数
        /// </summary>
        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if ((nCode == (int)HC_CODE.HC_ACTION))
            {
                MouseHookStruct mouseInfo = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                MouseEventArgs mouseEvent;
                if (wParam == (int)WM_MOUSE.WM_LBUTTONDOWN)
                {
                    mouseEvent = new MouseEventArgs(MouseButtons.Left, 1, mouseInfo.Point.X, mouseInfo.Point.Y, 0);
                    if (OnMouseDown != null)
                    {
                        OnMouseDown(this, mouseEvent);
                    }
                }
                else if (wParam == (int)WM_MOUSE.WM_RBUTTONDOWN)
                {
                    mouseEvent = new MouseEventArgs(MouseButtons.Right, 1, mouseInfo.Point.X, mouseInfo.Point.Y, 0);
                    if (OnMouseDown != null)
                    {
                        OnMouseDown(this, mouseEvent);
                    }
                }
                else if (wParam == (int)WM_MOUSE.WM_LBUTTONUP)
                {
                    mouseEvent = new MouseEventArgs(MouseButtons.Left, 1, mouseInfo.Point.X, mouseInfo.Point.Y, 0);
                    if (OnMouseUp != null)
                    {
                        OnMouseUp(this, mouseEvent);
                    }
                }
                else if (wParam == (int)WM_MOUSE.WM_RBUTTONUP)
                {
                    mouseEvent = new MouseEventArgs(MouseButtons.Right, 1, mouseInfo.Point.X, mouseInfo.Point.Y, 0);
                    if (OnMouseUp != null)
                    {
                        OnMouseUp(this, mouseEvent);
                    }
                }
                else if (wParam == (int)WM_MOUSE.WM_MOUSEMOVE)
                {
                    mouseEvent = new MouseEventArgs(MouseButtons.None, 0, mouseInfo.Point.X, mouseInfo.Point.Y, 0);
                    if (OnMouseMove != null)
                    {
                        OnMouseMove(this, mouseEvent);
                    }
                }
            }

            return Win32API.CallNextHookEx(mMouseHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// 安装钩子
        /// </summary>
        /// <returns></returns>
        public bool InstallHook()
        {
            //线程钩子时一定要通过这个取得的值才是操作系统下真实的线程
            uint result = Win32API.GetCurrentThreadId();

            #region 全局Hook
            //IntPtr pInstance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().ManifestModule);
            //using (Process curProcess = Process.GetCurrentProcess())
            //{
            //    using (ProcessModule curModule = curProcess.MainModule)
            //    {
            //        pInstance = Win32API.GetModuleHandle(curModule.ModuleName);
            //    }
            //}            
            #endregion

            if (mKeyboardHook == IntPtr.Zero)
            {
                mKeyboardHookProcedure = new HookProc(KeyboardHookProc);
                //注册线程钩子时第三个参数是空
                mKeyboardHook = Win32API.SetWindowsHookEx(WH_CODE.WH_KEYBOARD, mKeyboardHookProcedure, IntPtr.Zero, result);
                if (mKeyboardHook == IntPtr.Zero)
                {
                    return false;
                }
            }
            if (mMouseHook == IntPtr.Zero)
            {
                mMouseHookProcedure = new HookProc(MouseHookProc);
                //注册线程钩子时第三个参数是空
                mMouseHook = Win32API.SetWindowsHookEx(WH_CODE.WH_MOUSE, mMouseHookProcedure, IntPtr.Zero, result);
                if (mMouseHook == IntPtr.Zero)
                {
                    UnInstallHook();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 卸载钩子
        /// </summary>
        /// <returns></returns>
        public bool UnInstallHook()
        {
            bool result = true;
            if (mKeyboardHook != IntPtr.Zero)
            {
                result = Win32API.UnhookWindowsHookEx(mKeyboardHook) && result;
                mKeyboardHook = IntPtr.Zero;
            }
            if (mMouseHook != IntPtr.Zero)
            {
                result = (Win32API.UnhookWindowsHookEx(mMouseHook) && result);
                mMouseHook = IntPtr.Zero;
            }

            return result;
        }
    }

}
