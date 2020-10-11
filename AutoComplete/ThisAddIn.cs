using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Office.Interop.Word;

namespace AutoComplete
{
    public partial class ThisAddIn
    {
        private Document activeDocument;

        private KeyboardHook keyboardHook;

        private IntPtr docHandle;

        private IntPtr hIMC;

        // "" means ".
        private readonly Regex canCompletePairs = new Regex(@"\.|,|;|:|\s|\)|\]|}|>|""|）|”|】|》");

        // Pattern used when user presses Backspace.
        private readonly Regex canDelPairs = new Regex(@"<>|\[\]|\(\)|{}|（）|【】|《》|“”");

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            // Hook config.
            keyboardHook = new KeyboardHook
            {
                processAction = AutoProcessPairs
            };

            keyboardHook.InstallHook();

            // Subscribe DocumentChange event to update active document in time.
            Application.DocumentChange += OnWindowActivated;
        }

        /// <summary>
        /// When active window is changed, update active document and enable/disable auto complete.
        /// </summary>
        private void OnWindowActivated()
        {
            try
            {
                activeDocument = Application.ActiveDocument;
                keyboardHook.enabled = true;
                docHandle= (IntPtr)Application.ActiveWindow.Hwnd;
                hIMC = Win32API.ImmGetContext(docHandle);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                activeDocument = null;
                keyboardHook.enabled = false;
            }
        }

        private void AutoProcessPairs()
        {
            // Auto complete and auto delete.
            if (KeyboardHook.IsKeyDown(Keys.Back)) DelWithBackspace();
            else if (!KeyboardHook.IsKeyDown(Keys.Delete)) CompletePairs();
        }

        private void DelWithBackspace()
        {
            Selection currentSelection = Application.Selection;
            if (currentSelection.Type == WdSelectionType.wdSelectionIP)
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
        }

        private void CompletePairs()
        {
            if (KeyboardHook.IsKeyDown(Keys.ShiftKey)
                && !KeyboardHook.IsKeyDown(Keys.Back)
                && !KeyboardHook.IsKeyDown(Keys.Delete)
                && IsInProperLanguageState())
            {
                // (
                if (KeyboardHook.IsKeyDown(Keys.D9)) InsertText(")");
                // {
                else if (KeyboardHook.IsKeyDown(Keys.OemOpenBrackets)) InsertText("}");
                // <
                else if (KeyboardHook.IsKeyDown(Keys.Oemcomma)) InsertText(">");
            }
            else
            {
                // [
                if (KeyboardHook.IsKeyDown(Keys.OemOpenBrackets)) InsertText("]");
            }
        }

        private bool IsInProperLanguageState()
        {
            if (InputLanguage.CurrentInputLanguage.Culture.Parent.Name == "en")
            {
                return true;
            }

            int iMode = 0;
            int iSentence = 0;
            if (Win32API.ImmGetConversionStatus(hIMC, ref iMode, ref iSentence))
            {
                if (iMode == 0)
                {
                    return true;
                }
            }
            
            return false;
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
            keyboardHook.UnInstallHook();
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
