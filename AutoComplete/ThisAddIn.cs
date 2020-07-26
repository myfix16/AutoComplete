using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

namespace AutoComplete
{
    public partial class ThisAddIn
    {
        private Hook hook;

        // "" means ".
        private readonly Regex canCompletePairs = new Regex(@"\.|,|;|:|\s|\)|\]|}|>|""|）|”|】|》");

        // Pattern used when user presses Backspace.
        private readonly Regex canDelPairs = new Regex(@"<>|\[\]|\(\)|{}|（）|【】|《》|“”");

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            // Hook config.
            hook = new Hook
            {
                processAction = AutoProcessPairs
            };

            hook.InstallHook();
        }

        private void AutoProcessPairs()
        {
            // Auto complete and auto delete.
            if (Hook.IsKeyDown(Keys.Back)) DelWithBackspace();
            else if (!Hook.IsKeyDown(Keys.Delete)) CompletePairs();
        }

        private void DelWithBackspace()
        {
            Selection currentSelection = Application.Selection;
            if (currentSelection.Type == WdSelectionType.wdSelectionIP)
            {
#if DEBUG
                try
                {
                    var endPoint = currentSelection.Range.End;
                    // If the selection is not on the place of first character,
                    // continue smart delete procedure.
                    if (endPoint >= 1)
                    {
                        var pairs = Application.ActiveDocument
                            .Range(endPoint - 1, endPoint + 1)
                            .Text;
                        if (canDelPairs.IsMatch(pairs)) currentSelection.Range.Delete();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    throw;
                }
#else
                    var endPoint = currentSelection.Range.End;
                    // If the selection is not on the place of first character,
                    // continue smart delete procedure.
                    if (endPoint >= 1)
                    {
                        var pairs = Application.ActiveDocument
                            .Range(endPoint - 1, endPoint + 1)
                            .Text;
                        if (canDelPairs.IsMatch(pairs)) currentSelection.Range.Delete();
                    }
#endif
            }
        }

        private void CompletePairs()
        {
            if (Hook.IsKeyDown(Keys.ShiftKey) && !Hook.IsKeyDown(Keys.Back) && !Hook.IsKeyDown(Keys.Delete))
            {
                // (
                if (Hook.IsKeyDown(Keys.D9)) InsertText(")");
                // {
                else if (Hook.IsKeyDown(Keys.OemOpenBrackets)) InsertText("}");
                // "
                else if (Hook.IsKeyDown(Keys.OemQuotes)) InsertText("\"");
                // <
                else if (Hook.IsKeyDown(Keys.Oemcomma)) InsertText(">");
            }
            else
            {
                // [
                if (Hook.IsKeyDown(Keys.OemOpenBrackets)) InsertText("]");
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
            return canCompletePairs.IsMatch(charAfter) || charAfter.Length == 0;
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
