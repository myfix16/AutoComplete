using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoComplete
{
    public class BindingFunctions
    {
        [DllImport("user32.dll")]
        static extern short GetKeyState(int nVirtKey);

        public static bool IsKeyDown(Keys keys)
        {
            return (GetKeyState((int)keys) & 0x8000) == 0x8000;
        }

    }
}
