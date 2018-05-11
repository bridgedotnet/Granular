using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Granular.Extensions;
using System.Windows.Input;
using static Retyped.dom;

namespace Granular.Host.Render
{
    public class HtmlTextBoxRenderElement : HtmlRenderElement, ITextBoxRenderElement
    {
        private HtmlRenderElement contentElement;
        private HtmlRenderElement ContentElement
        {
            get { return contentElement; }
            set
            {
                if (contentElement == value)
                {
                    return;
                }

                if (contentElement != null)
                {
                    HtmlElement.removeChild(contentElement.HtmlElement);
                }

                contentElement = value;

                if (contentElement != null)
                {
                    HtmlElement.appendChild(contentElement.HtmlElement);
                }
            }
        }

        public event EventHandler TextChanged;
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (text == value)
                {
                    return;
                }

                text = value;

                renderQueue.InvokeAsync(() =>
                {
                    SetContentElementText();
                    GetContentElementSelection();
                });

                TextChanged.Raise(this);
            }
        }

        private int maxLength;
        public int MaxLength
        {
            get { return maxLength; }
            set
            {
                if (maxLength == value)
                {
                    return;
                }

                maxLength = value;
                renderQueue.InvokeAsync(SetContentElementMaxLength);
            }
        }

        public event EventHandler CaretIndexChanged;
        private int caretIndex;
        public int CaretIndex
        {
            get { return caretIndex; }
            set
            {
                if (caretIndex == value)
                {
                    return;
                }

                caretIndex = value;
                renderQueue.InvokeAsync(SetContentElementCaretIndex);
                CaretIndexChanged.Raise(this);
            }
        }

        public event EventHandler SelectionStartChanged;
        private int selectionStart;
        public int SelectionStart
        {
            get { return selectionStart; }
            set
            {
                if (selectionStart == value)
                {
                    return;
                }

                selectionStart = value;
                renderQueue.InvokeAsync(SetContentElementSelectionStart);
                SelectionStartChanged.Raise(this);
            }
        }

        public event EventHandler SelectionLengthChanged;
        private int selectionLength;
        public int SelectionLength
        {
            get { return selectionLength; }
            set
            {
                if (selectionLength == value)
                {
                    return;
                }

                selectionLength = value;
                renderQueue.InvokeAsync(SetContentElementSelectionLength);
                SelectionLengthChanged.Raise(this);
            }
        }

        private bool isPassword;
        public bool IsPassword
        {
            get { return isPassword; }
            set
            {
                if (isPassword == value)
                {
                    return;
                }

                isPassword = value;
                renderQueue.InvokeAsync(SetContentElement);
            }
        }

        private bool isReadOnly;
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                if (isReadOnly == value)
                {
                    return;
                }

                isReadOnly = value;
                renderQueue.InvokeAsync(SetContentElementIsReadOnly);
            }
        }

        private bool spellCheck;
        public bool SpellCheck
        {
            get { return spellCheck; }
            set
            {
                if (spellCheck == value)
                {
                    return;
                }

                spellCheck = value;
                renderQueue.InvokeAsync(SetContentElementSpellCheck);
            }
        }

        private System.Windows.Rect bounds;
        public System.Windows.Rect Bounds
        {
            get { return bounds; }
            set
            {
                if (bounds == value)
                {
                    return;
                }

                bounds = value;
                renderQueue.InvokeAsync(() =>
                {
                    HtmlElement.SetHtmlBounds(bounds, converter);
                    ContentElement.HtmlElement.SetHtmlSize(bounds.Size, converter);
                });
            }
        }

        private Brush foreground;
        public Brush Foreground
        {
            get { return foreground; }
            set
            {
                if (foreground == value)
                {
                    return;
                }

                if (IsLoaded && foreground != null)
                {
                    foreground.Changed -= OnForegroundChanged;
                }

                foreground = value;
                renderQueue.InvokeAsync(() => ContentElement.HtmlElement.SetHtmlForeground(Foreground, converter));

                if (IsLoaded && foreground != null)
                {
                    foreground.Changed += OnForegroundChanged;
                }
            }
        }

        private FontFamily fontFamily;
        public FontFamily FontFamily
        {
            get { return fontFamily; }
            set
            {
                if (fontFamily == value)
                {
                    return;
                }

                fontFamily = value;
                renderQueue.InvokeAsync(() => ContentElement.HtmlElement.SetHtmlFontFamily(fontFamily, converter));
            }
        }

        private double fontSize;
        public double FontSize
        {
            get { return fontSize; }
            set
            {
                if (fontSize == value)
                {
                    return;
                }

                fontSize = value;
                renderQueue.InvokeAsync(() => ContentElement.HtmlElement.SetHtmlFontSize(fontSize, converter));
            }
        }

        private FontStyle fontStyle;
        public FontStyle FontStyle
        {
            get { return fontStyle; }
            set
            {
                if (fontStyle == value)
                {
                    return;
                }

                fontStyle = value;
                renderQueue.InvokeAsync(() => ContentElement.HtmlElement.SetHtmlFontStyle(fontStyle, converter));
            }
        }

        private FontWeight fontWeight;
        public FontWeight FontWeight
        {
            get { return fontWeight; }
            set
            {
                if (fontWeight == value)
                {
                    return;
                }

                fontWeight = value;
                renderQueue.InvokeAsync(() => ContentElement.HtmlElement.SetHtmlFontWeight(fontWeight, converter));
            }
        }

        private FontStretch fontStretch;
        public FontStretch FontStretch
        {
            get { return fontStretch; }
            set
            {
                if (fontStretch == value)
                {
                    return;
                }

                fontStretch = value;
                renderQueue.InvokeAsync(() => ContentElement.HtmlElement.SetHtmlFontStretch(fontStretch, converter));
            }
        }

        private TextAlignment textAlignment;
        public TextAlignment TextAlignment
        {
            get { return textAlignment; }
            set
            {
                if (textAlignment == value)
                {
                    return;
                }

                textAlignment = value;
                renderQueue.InvokeAsync(() => ContentElement.HtmlElement.SetHtmlTextAlignment(textAlignment, converter));
            }
        }

        private TextTrimming textTrimming;
        public TextTrimming TextTrimming
        {
            get { return textTrimming; }
            set
            {
                if (textTrimming == value)
                {
                    return;
                }

                textTrimming = value;
                renderQueue.InvokeAsync(() => ContentElement.HtmlElement.SetHtmlTextTrimming(textTrimming));
            }
        }

        private TextWrapping textWrapping;
        public TextWrapping TextWrapping
        {
            get { return textWrapping; }
            set
            {
                if (textWrapping == value)
                {
                    return;
                }

                textWrapping = value;
                renderQueue.InvokeAsync(SetContentElementTextWrapping);
            }
        }

        private bool acceptsReturn;
        public bool AcceptsReturn
        {
            get { return acceptsReturn; }
            set
            {
                if (acceptsReturn == value)
                {
                    return;
                }

                acceptsReturn = value;
                renderQueue.InvokeAsync(SetContentElement);
            }
        }

        private ScrollBarVisibility horizontalScrollBarVisibility;
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return horizontalScrollBarVisibility; }
            set
            {
                if (horizontalScrollBarVisibility == value)
                {
                    return;
                }

                horizontalScrollBarVisibility = value;
                renderQueue.InvokeAsync(() => ContentElement.HtmlElement.SetHtmlHorizontalScrollBarVisibility(horizontalScrollBarVisibility, converter));
            }
        }

        private ScrollBarVisibility verticalScrollBarVisibility;
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return verticalScrollBarVisibility; }
            set
            {
                if (verticalScrollBarVisibility == value)
                {
                    return;
                }

                verticalScrollBarVisibility = value;
                renderQueue.InvokeAsync(() => ContentElement.HtmlElement.SetHtmlVerticalScrollBarVisibility(verticalScrollBarVisibility, converter));
            }
        }

        private bool isHitTestVisible;
        public bool IsHitTestVisible
        {
            get { return isHitTestVisible; }
            set
            {
                if (isHitTestVisible == value)
                {
                    return;
                }

                isHitTestVisible = value;
                renderQueue.InvokeAsync(() => ContentElement.HtmlElement.SetHtmlIsHitTestVisible(IsHitTestVisible));
            }
        }

        public bool AcceptsTab { get; set; }

        private IRenderElementFactory factory;
        private RenderQueue renderQueue;
        private HtmlValueConverter converter;
        private bool isFocused;

        public HtmlTextBoxRenderElement(IRenderElementFactory factory, RenderQueue renderQueue, HtmlValueConverter converter)
        {
            this.factory = factory;
            this.renderQueue = renderQueue;
            this.converter = converter;

            this.bounds = System.Windows.Rect.Empty;
            this.fontFamily = FontFamily.Default;

            HTMLElement styleElement = document.createElement("style");
            styleElement.textContent = "::-ms-clear { width: 0px; height: 0px; }";

            HtmlElement.appendChild(styleElement);

            SetContentElement();
        }

        private void SetContentElement()
        {
            HTMLElement htmlElement;

            if (IsPassword || !AcceptsReturn)
            {
                htmlElement = document.createElement("input");
                htmlElement.setAttribute("type", IsPassword ? "password" : "text");
            }
            else
            {
                htmlElement = document.createElement("textArea");
            }

            ContentElement = new HtmlRenderElement(htmlElement);

            SetContentElementText();
            SetContentElementMaxLength();
            SetContentElementSelectionStart();
            SetContentElementSelectionLength();
            SetContentElementIsReadOnly();
            SetContentElementSpellCheck();
            SetContentElementTextWrapping();

            ContentElement.HtmlElement.SetHtmlStyleProperty("resize", "none");
            ContentElement.HtmlElement.SetHtmlStyleProperty("margin", "0px");
            ContentElement.HtmlElement.SetHtmlStyleProperty("padding", "0px");
            ContentElement.HtmlElement.SetHtmlStyleProperty("border", "0px solid transparent");
            ContentElement.HtmlElement.SetHtmlStyleProperty("outline", "1px solid transparent");
            ContentElement.HtmlElement.SetHtmlStyleProperty("cursor", "inherit");
            ContentElement.HtmlElement.SetHtmlBackground(Brushes.Transparent, System.Windows.Rect.Zero, factory, converter);
            ContentElement.HtmlElement.SetHtmlLocation(Point.Zero, converter);

            ContentElement.HtmlElement.SetHtmlSize(Bounds.Size, converter);
            ContentElement.HtmlElement.SetHtmlForeground(Foreground, converter);
            ContentElement.HtmlElement.SetHtmlFontFamily(FontFamily, converter);
            ContentElement.HtmlElement.SetHtmlFontSize(FontSize, converter);
            ContentElement.HtmlElement.SetHtmlFontStyle(FontStyle, converter);
            ContentElement.HtmlElement.SetHtmlFontWeight(FontWeight, converter);
            ContentElement.HtmlElement.SetHtmlFontStretch(FontStretch, converter);
            ContentElement.HtmlElement.SetHtmlIsHitTestVisible(IsHitTestVisible);
            ContentElement.HtmlElement.SetHtmlTextAlignment(TextAlignment, converter);
            ContentElement.HtmlElement.SetHtmlTextTrimming(TextTrimming);
            ContentElement.HtmlElement.SetHtmlHorizontalScrollBarVisibility(HorizontalScrollBarVisibility, converter);
            ContentElement.HtmlElement.SetHtmlVerticalScrollBarVisibility(VerticalScrollBarVisibility, converter);

            ContentElement.HtmlElement.oninput += e => { this.Text = ContentElement.HtmlElement.GetValue(); return e; };
            ContentElement.HtmlElement.onkeydown += OnContentElementKeyDown;
            ContentElement.HtmlElement.onselect += e => GetContentElementSelection();
            ContentElement.HtmlElement.onkeyup += e => GetContentElementSelection();
            ContentElement.HtmlElement.onmouseup += e => GetContentElementSelection();
        }

        public void Focus()
        {
            isFocused = true;
            ContentElement.HtmlElement.focus();
        }

        public void ClearFocus()
        {
            isFocused = false;
            ContentElement.HtmlElement.blur();
        }

        private void OnForegroundChanged(object sender, EventArgs e)
        {
            renderQueue.InvokeAsync(() => ContentElement.HtmlElement.SetHtmlForeground(Foreground, converter));
        }

        private object OnContentElementKeyDown(Event e)
        {
            if (!IsReadOnly && AcceptsTab && ((KeyboardEvent)e).keyCode == 9)
            {
                int selectionStart = SelectionStart;

                string contentElementText = ContentElement.HtmlElement.GetValue();
                this.Text = String.Format("{0}\t{1}", contentElementText.Substring(0, SelectionStart), contentElementText.Substring(SelectionStart + SelectionLength));

                ContentElement.HtmlElement.SetSelectionStart(selectionStart + 1);
                ContentElement.HtmlElement.SetSelectionEnd(selectionStart + 1);
                GetContentElementSelection();

                e.preventDefault();
            }

            return e;
        }

        private object GetContentElementSelection()
        {
            int selectionStart = ContentElement.HtmlElement.GetSelectionStart();
            int selectionEnd = ContentElement.HtmlElement.GetSelectionEnd();

            if (this.SelectionStart != selectionStart || this.SelectionLength != selectionEnd - selectionStart)
            {
                int changeIndex = this.SelectionStart + this.SelectionLength != selectionEnd ? selectionEnd : selectionStart;

                this.SelectionStart = selectionStart;
                this.SelectionLength = selectionEnd - selectionStart;
                this.CaretIndex = changeIndex;
            }

            return null;
        }

        private void SetContentElementCaretIndex()
        {
            if (isFocused && CaretIndex != SelectionStart && CaretIndex != SelectionStart + SelectionLength)
            {
                ContentElement.HtmlElement.focus();
                ContentElement.HtmlElement.SetCaretIndex(CaretIndex);
            }
        }

        private void SetContentElementSelectionStart()
        {
            if (ContentElement.HtmlElement.GetSelectionStart() != SelectionStart)
            {
                ContentElement.HtmlElement.SetSelectionStart(SelectionStart);
            }
        }

        private void SetContentElementSelectionLength()
        {
            if (ContentElement.HtmlElement.GetSelectionEnd() != SelectionStart + SelectionLength)
            {
                ContentElement.HtmlElement.SetSelectionEnd(SelectionStart + SelectionLength);
            }
        }

        private void SetContentElementText()
        {
            if (ContentElement.HtmlElement.GetValue() != Text.DefaultIfNullOrEmpty())
            {
                ContentElement.HtmlElement.SetValue(Text);
            }
        }

        private void SetContentElementMaxLength()
        {
            if (maxLength > 0)
            {
                ContentElement.HtmlElement.setAttribute("maxLength", maxLength.ToString());
            }
            else
            {
                ContentElement.HtmlElement.removeAttribute("maxLength");
            }
        }

        private void SetContentElementIsReadOnly()
        {
            if (IsReadOnly)
            {
                ContentElement.HtmlElement.setAttribute("readonly", String.Empty);
            }
            else
            {
                ContentElement.HtmlElement.removeAttribute("readonly");
            }
        }

        private void SetContentElementSpellCheck()
        {
            ContentElement.HtmlElement.setAttribute("spellcheck", converter.ToBooleanString(SpellCheck));
        }

        private void SetContentElementTextWrapping()
        {
            ContentElement.HtmlElement.setAttribute("wrap", converter.ToWrapString(TextWrapping));
        }

        public void ProcessKeyEvent(KeyEventArgs e)
        {
            e.ForceHostHandling = true;
            e.Handled = true;
        }

        protected override void OnLoad()
        {
            if (Foreground != null)
            {
                Foreground.Changed += OnForegroundChanged;
            }

            renderQueue.InvokeAsync(() => ContentElement.HtmlElement.SetHtmlForeground(Foreground, converter));
        }

        protected override void OnUnload()
        {
            if (Foreground != null)
            {
                Foreground.Changed -= OnForegroundChanged;
            }
        }
    }
}
