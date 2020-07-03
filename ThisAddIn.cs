using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using System.Windows.Forms;

namespace AutoComplete
{
    public partial class ThisAddIn
    {
        Hook hook;

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            hook = new Hook();
            hook.InstallHook();
            hook.OnKeyDown += Hook_OnKeyDown;
        }

        private void Hook_OnKeyDown(object sender, KeyEventArgs e)
        {
            Word.Range rng = Application.ActiveDocument.Range(0, 0);
            rng.Text = "New Text";
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
            hook.UnInstallHook();
        }

        #region VSTO 生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InternalStartup()
        {
            Startup += new System.EventHandler(ThisAddIn_Startup);
            Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
