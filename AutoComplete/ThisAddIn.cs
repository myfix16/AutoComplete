﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;

namespace AutoComplete
{
    public partial class ThisAddIn
    {
        Hook hook;
        readonly Regex endCharjudge = new Regex(@"\.|,|;|:|\s|\)|\]|\}|\>");

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            // Hook config.
            hook = new Hook
            {
                processAction = AutoCompleteBracket
            };

            hook.InstallHook();
        }

        private void AutoCompleteBracket(Keys keyData)
        {
            if (Hook.IsKeyDown(Keys.ShiftKey) && Hook.IsKeyDown(keyData))
            {
                switch (keyData)
                {
                    // ( 
                    case Keys.D9:
                        InsertText(")");
                        break;
                    // {
                    case Keys.OemOpenBrackets:
                        InsertText("}");
                        break;
                    // "
                    case Keys.OemQuotes:
                        InsertText("\"");
                        break;
                    // <
                    case Keys.Oemcomma:
                        InsertText(">");
                        break;
                    default:
                        break;
                }
            }
            else if (Hook.IsKeyDown(keyData))
            {
                /*// '
                if (keyData == Keys.OemQuotes) InsertText("'");*/

                // [
                if (keyData == Keys.OemOpenBrackets) InsertText("]");
            }
        }

        private void InsertText(string anotherHalf)
        {
            Selection currentSelection = Application.Selection;
            // Test to see if selection is an insertion point(usually represented by a blinking vertical line).
            if (currentSelection.Type == WdSelectionType.wdSelectionIP
                && NeedsComplete(ref currentSelection))
            {
                currentSelection.Range.InsertAfter(anotherHalf);
            }
            #region Selection Normal
            //else if (currentSelection.Type == Word.WdSelectionType.wdSelectionNormal)
            //{
            //    // Move to start of selection.
            //    if (Application.Options.ReplaceSelection)
            //    {
            //        object direction = Word.WdCollapseDirection.wdCollapseEnd;
            //        currentSelection.Collapse(ref direction);
            //    }
            //    currentSelection.TypeText("Inserting before a text block. ");
            //}
            #endregion
        }

        /// <summary>
        /// It will check the character after selectionIP and decide whether to invoke AutoComplete.
        /// </summary>
        /// <param name="currentSelection"></param>
        private bool NeedsComplete(ref Selection currentSelection)
        {
            var charAfter = Application.ActiveDocument
                .Range(currentSelection.Range.End, currentSelection.Range.End + 1)
                .Text;
            return endCharjudge.IsMatch(charAfter) || charAfter.Length == 0;
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
