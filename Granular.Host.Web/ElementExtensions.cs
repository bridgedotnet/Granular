using System;
using System.Collections.Generic;
using static Retyped.dom;
using System.Runtime.CompilerServices;
using System.Text;

namespace Granular.Host
{
    public static class ElementExtensions
    {
        [Bridge.Template("{element}.setSelectionRange({index}, {index})")]
        public static extern void SetCaretIndex(this HTMLElement element, int index);

        [Bridge.Template("{element}.selectionStart")]
        public static extern int GetSelectionStart(this HTMLElement element);

        [Bridge.Template("{element}.selectionStart = {value}")]
        public static extern void SetSelectionStart(this HTMLElement element, int value);

        [Bridge.Template("{element}.selectionEnd")]
        public static extern int GetSelectionEnd(this HTMLElement element);

        [Bridge.Template("{element}.selectionEnd = {value}")]
        public static extern void SetSelectionEnd(this HTMLElement element, int value);

        [Bridge.Template("{element}.value")]
        public static extern string GetValue(this HTMLElement element);

        [Bridge.Template("{element}.value = {value}")]
        public static extern void SetValue(this HTMLElement element, string value);
    }
}
