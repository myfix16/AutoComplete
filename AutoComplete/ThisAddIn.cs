using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

        private MessageHook messageHook;

        // "" means ".
        private readonly Regex canCompletePairs = new Regex(@"\.|,|;|:|\s|\)|\]|}|>|）|】|》");

        // Pattern used when user presses Backspace.
        private readonly Regex canDelPairs = new Regex(@"<>|\[\]|\(\)|{}|（）|【】|《》|“”");

        private readonly Dictionary<string, string> insertMapping = new Dictionary<string, string>
        {
            ["("] = ")",
            ["["] = "]",
            ["["] = "]",
            ["<"] = ">",
            ["（"] = "）",
            ["【"] = "】",
            ["《"] = "》",
        };

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            // Hook config.
            keyboardHook = new KeyboardHook
            {
                processAction = DelWithBackspace
            };

            keyboardHook.InstallHook();

            messageHook = new MessageHook()
            {
                processAction = CompletePairs
            };

            messageHook.InstallHook();

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
                messageHook.enabled = true;

                messageHook.handle = GetHandle(activeDocument.Name);
                messageHook.hIMC = ImmGetContext(messageHook.handle);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                activeDocument = null;
                keyboardHook.enabled = false;
                messageHook.enabled = false;
            }
        }

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
            if (KeyboardHook.IsKeyDown(Keys.Back)
                && TryGetText(-1, 1, out string pairs)
                && canDelPairs.IsMatch(pairs))
            {
                Application.Selection.Range.Delete();
            }
        }

        private string GetChar(IntPtr lParam)
        {
            var pmsg = Marshal.PtrToStructure<MSG>(lParam);
            if (pmsg.message == (int)WM_IMM.WM_IME_CHAR || pmsg.message == (int)WM_IMM.WM_CHAR)
            {
                MessageBox.Show(pmsg.wParam.ToString());
                var list = new List<string> { "(", "[", "{" };
                var character = ((char)pmsg.wParam).ToString();
                if (list.Contains(character)) MessageBox.Show(character);
                return ((char)pmsg.wParam).ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Complete a pair of bracket if necessary.
        /// </summary>
        private void CompletePairs(IntPtr lParam)
        {
            if (!KeyboardHook.IsKeyDown(Keys.Back) && !KeyboardHook.IsKeyDown(Keys.Delete))
            {
                // TODO: The method needs a in parameter.
                bool isOneOfPairs = insertMapping.TryGetValue(GetChar(lParam), out string value);
                if (isOneOfPairs) InsertText(value);
            }
        }

        private void InsertText(string anotherHalf)
        {
            Selection currentSelection = Application.Selection;

            // Test to see if selection is an insertion point(usually represented by a blinking vertical line).
            if (currentSelection.Type == WdSelectionType.wdSelectionIP && NeedsComplete())
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
        private bool NeedsComplete()
        {
            TryGetText(0, 1, out string charAfter);
            return canCompletePairs.IsMatch(charAfter) || charAfter.Length == 0;
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
            messageHook.UnInstallHook();
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
