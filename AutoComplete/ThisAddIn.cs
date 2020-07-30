using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using static AutoComplete.Win32API;

namespace AutoComplete
{
    public partial class ThisAddIn
    {
        private Document activeDocument;

        private KeyboardHook keyboardHook;

        // "" means ".
        private readonly Regex canCompletePairs = new Regex(@"\.|,|;|:|\s|\)|\]|}|>|""|）|”|】|》");

        // Pattern used when user presses Backspace.
        private readonly Regex canDelPairs = new Regex(@"<>|\[\]|\(\)|{}|（）|【】|《》|“”");

        private readonly List<string> languageIMEInfo = new List<string> { "zh-CN", "zh-TW" };

        // Mapping of half shape char to full shape char.
        private readonly Dictionary<string, string> FullShapeChar = new Dictionary<string, string>
        {
            [")"] = "）",
            ["]"] = "】",
            ["}"] = "}",
            [">"] = "》",
            ["\""] = "”",
        };

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
        /// When active window is changed, update active document and enable/diable auto complete.
        /// </summary>
        private void OnWindowActivated()
        {
            try
            {
                activeDocument = Application.ActiveDocument;
                keyboardHook.enabled = true;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                activeDocument = null;
                keyboardHook.enabled = false;
            }
        }

        /// <summary>
        /// The function will look for current input language and see whether it's Chinese.
        /// If so, it will return true. Otherwise, it will return false.
        /// </summary>
        private bool GetCultureType()
            => languageIMEInfo.Contains(InputLanguage.CurrentInputLanguage.Culture.Name);

        /// <summary>
        /// Retrieve the handle of active document.
        /// </summary>
        /// <param name="docName">The name of active document.</param>
        /// <returns>The handle of active document.</returns>
        private IntPtr GetHandle(string docName)
            => FindWindow(null, docName + " - Word")
                .FindWindowEx(0, "_WwF", null)
                .FindWindowEx(0, "_WwF", null)
                .FindWindowEx(0, "_WwF", null);

        /// <summary>
        /// The process method of key press event.
        /// </summary>
        private void AutoProcessPairs()
        {
            // Auto complete and auto delete.
            if (KeyboardHook.IsKeyDown(Keys.Back)) DelWithBackspace();
            else if (!KeyboardHook.IsKeyDown(Keys.Delete)) CompletePairs();
        }

        /// <summary>
        ///     The function will try to get text from a given range marked by params startFrom and endWith. 
        ///     If the the selection is an insertion point(IP) and the range is valid, it will return true and 
        ///     give the text in out param text. Otherwise, it will return false and text will be null.
        /// </summary>
        /// <param name="startFrom">The starting point using IP as reference.</param>
        /// <param name="endWith">The ending point using IP as reference.</param>
        /// <param name="text">The text wanted.</param>
        /// <returns>Whether it successfully get the text.</returns>
        private bool TryGetText(int startFrom, int endWith, out string text)
        {
            text = null;
            var currentSelection = Application.Selection;

            if (activeDocument == null) return false;

            else if (currentSelection.Type == WdSelectionType.wdSelectionIP)
            {
                var endPoint = currentSelection.Range.End;

                text = activeDocument
                        .Range(Math.Max(endPoint + startFrom, 0), endPoint + endWith)
                        .Text;

                return true;
            }

            else return false;
        }

        /// <summary>
        /// Delete a pair of bracket if possible.
        /// </summary>
        private void DelWithBackspace()
        {
            if (TryGetText(-1, 1, out string pairs) && canDelPairs.IsMatch(pairs))
            {
                Application.Selection.Range.Delete();
            }
        }

        /// <summary>
        /// Complete a pair of bracket if necessary.
        /// </summary>
        private void CompletePairs()
        {
            // TODO: Retrive key press message intercepted by IME.
            if (KeyboardHook.IsKeyDown(Keys.ShiftKey) 
                && !KeyboardHook.IsKeyDown(Keys.Back) && !KeyboardHook.IsKeyDown(Keys.Delete))
            {
                // (
                if (KeyboardHook.IsKeyDown(Keys.D9)) InsertText(")");
                // {
                else if (KeyboardHook.IsKeyDown(Keys.OemOpenBrackets)) InsertText("}");
                // "
                else if (KeyboardHook.IsKeyDown(Keys.OemQuotes)) InsertText("\"");
                // <
                else if (KeyboardHook.IsKeyDown(Keys.Oemcomma)) InsertText(">");
            }
            else
            {
                // [
                if (KeyboardHook.IsKeyDown(Keys.OemOpenBrackets)) InsertText("]");
            }
        }

        private void InsertText(string anotherHalf)
        {
            Selection currentSelection = Application.Selection;

            // Test to see if selection is an insertion point(usually represented by a blinking vertical line).
            if (currentSelection.Type == WdSelectionType.wdSelectionIP
                && NeedsComplete())
            {
                bool isFullShape = GetCultureType()
                                   && ConversionStatusChecker.IsFullShape(GetHandle(activeDocument.Name));
                currentSelection.Range.InsertAfter(isFullShape ? FullShapeChar[anotherHalf] : anotherHalf);
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
        private bool NeedsComplete()
        {
            TryGetText(0, 1, out string charAfter);
            return canCompletePairs.IsMatch(charAfter) || charAfter.Length == 0;
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
            keyboardHook.UnInstallHook();
            //messageHook.UnInstallHook();
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
