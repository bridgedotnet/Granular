using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace System.Windows.Controls
{
    public class DataGridTextColumn : DataGridBoundColumn
    {
        static DataGridTextColumn()
        {
            ElementStyleProperty.OverrideMetadata(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(DefaultElementStyle));
            EditingElementStyleProperty.OverrideMetadata(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(DefaultEditingElementStyle));
        }

        #region Styles

        /// <summary>
        ///     The default value of the ElementStyle property.
        ///     This value can be used as the BasedOn for new styles.
        /// </summary>
        public static Style DefaultElementStyle
        {
            get
            {
                if (_defaultElementStyle == null)
                {
                    Style style = new Style { TargetType = typeof(TextBlock) };

                    // Use the same margin used on the TextBox to provide space for the caret
                    //style.Setters.Add(new Setter(TextBlock.MarginProperty, new Thickness(2.0, 0.0, 2.0, 0.0)));

                    _defaultElementStyle = style;
                }

                return _defaultElementStyle;
            }
        }

        /// <summary>
        ///     The default value of the EditingElementStyle property.
        ///     This value can be used as the BasedOn for new styles.
        /// </summary>
        public static Style DefaultEditingElementStyle
        {
            get
            {
                if (_defaultEditingElementStyle == null)
                {
                    Style style = new Style { TargetType = typeof(TextBox) };

                    //style.Setters.Add(new Setter(TextBox.BorderThicknessProperty, new Thickness(0.0)));
                    //style.Setters.Add(new Setter(TextBox.PaddingProperty, new Thickness(0.0)));

                    _defaultEditingElementStyle = style;
                }

                return _defaultEditingElementStyle;
            }
        }

        #endregion

        #region Element Generation

        /// <summary>
        ///     Creates the visual tree for text based cells.
        /// </summary>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            TextBlock textBlock = new TextBlock();

            SyncProperties(textBlock);

            ApplyStyle(/* isEditing = */ false, /* defaultToElementStyle = */ false, textBlock);
            ApplyBinding(textBlock, TextBlock.TextProperty);

            // DataGridHelper.RestoreFlowDirection(textBlock, cell);

            return textBlock;
        }

        /// <summary>
        ///     Creates the visual tree for text based cells.
        /// </summary>
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            TextBox textBox = new TextBox();

            SyncProperties(textBox);

            ApplyStyle(/* isEditing = */ true, /* defaultToElementStyle = */ false, textBox);
            ApplyBinding(textBox, TextBox.TextProperty);

            // DataGridHelper.RestoreFlowDirection(textBox, cell);

            return textBox;
        }

        private void SyncProperties(FrameworkElement e)
        {
            //DataGridHelper.SyncColumnProperty(this, e, TextElement.FontFamilyProperty, FontFamilyProperty);
            //DataGridHelper.SyncColumnProperty(this, e, TextElement.FontSizeProperty, FontSizeProperty);
            //DataGridHelper.SyncColumnProperty(this, e, TextElement.FontStyleProperty, FontStyleProperty);
            //DataGridHelper.SyncColumnProperty(this, e, TextElement.FontWeightProperty, FontWeightProperty);
            //DataGridHelper.SyncColumnProperty(this, e, TextElement.ForegroundProperty, ForegroundProperty);
        }

        protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
        {
            DataGridCell cell = element as DataGridCell;

            if (cell != null)
            {
                FrameworkElement textElement = cell.Content as FrameworkElement;

                if (textElement != null)
                {
                    //switch (propertyName)
                    //{
                    //    case "FontFamily":
                    //        DataGridHelper.SyncColumnProperty(this, textElement, TextElement.FontFamilyProperty, FontFamilyProperty);
                    //        break;
                    //    case "FontSize":
                    //        DataGridHelper.SyncColumnProperty(this, textElement, TextElement.FontSizeProperty, FontSizeProperty);
                    //        break;
                    //    case "FontStyle":
                    //        DataGridHelper.SyncColumnProperty(this, textElement, TextElement.FontStyleProperty, FontStyleProperty);
                    //        break;
                    //    case "FontWeight":
                    //        DataGridHelper.SyncColumnProperty(this, textElement, TextElement.FontWeightProperty, FontWeightProperty);
                    //        break;
                    //    case "Foreground":
                    //        DataGridHelper.SyncColumnProperty(this, textElement, TextElement.ForegroundProperty, ForegroundProperty);
                    //        break;
                    //}
                }
            }

            base.RefreshCellContent(element, propertyName);
        }

        #endregion

        #region Editing

        /// <summary>
        ///     Called when a cell has just switched to edit mode.
        /// </summary>
        /// <param name="editingElement">A reference to element returned by GenerateEditingElement.</param>
        /// <param name="editingEventArgs">The event args of the input event that caused the cell to go into edit mode. May be null.</param>
        /// <returns>The unedited value of the cell.</returns>
        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            //TextBox textBox = editingElement as TextBox;
            //if (textBox != null)
            //{
            //    textBox.Focus();

            //    string originalValue = textBox.Text;

            //    TextCompositionEventArgs textArgs = editingEventArgs as TextCompositionEventArgs;
            //    if (textArgs != null)
            //    {
            //        // If text input started the edit, then replace the text with what was typed.
            //        string inputText = ConvertTextForEdit(textArgs.Text);
            //        textBox.Text = inputText;

            //        // Place the caret after the end of the text.
            //        textBox.Select(inputText.Length, 0);
            //    }
            //    else
            //    {
            //        // If a mouse click started the edit, then place the caret under the mouse.
            //        MouseButtonEventArgs mouseArgs = editingEventArgs as MouseButtonEventArgs;
            //        if ((mouseArgs == null) || !PlaceCaretOnTextBox(textBox, Mouse.GetPosition(textBox)))
            //        {
            //            // If the mouse isn't over the textbox or something else started the edit, then select the text.
            //            textBox.SelectAll();
            //        }
            //    }

            //    return originalValue;
            //}

            return null;
        }

        // convert text the user has typed into the appropriate string to enter into the editable TextBox
        string ConvertTextForEdit(string s)
        {
            // Backspace becomes the empty string
            if (s == "\b")
            {
                s = String.Empty;
            }

            return s;
        }

        /// <summary>
        ///     Called when a cell's value is to be restored to its original value,
        ///     just before it exits edit mode.
        /// </summary>
        /// <param name="editingElement">A reference to element returned by GenerateEditingElement.</param>
        /// <param name="uneditedValue">The original, unedited value of the cell.</param>
        protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
        {
            // DataGridHelper.CacheFlowDirection(editingElement, editingElement != null ? editingElement.Parent as DataGridCell : null);

            base.CancelCellEdit(editingElement, uneditedValue);
        }

        /// <summary>
        ///     Called when a cell's value is to be committed, just before it exits edit mode.
        /// </summary>
        /// <param name="editingElement">A reference to element returned by GenerateEditingElement.</param>
        /// <returns>false if there is a validation error. true otherwise.</returns>
        protected override bool CommitCellEdit(FrameworkElement editingElement)
        {
            // DataGridHelper.CacheFlowDirection(editingElement, editingElement != null ? editingElement.Parent as DataGridCell : null);

            return base.CommitCellEdit(editingElement);
        }

        private static bool PlaceCaretOnTextBox(TextBox textBox, Point position)
        {
            //int characterIndex = textBox.GetCharacterIndexFromPoint(position, /* snapToText = */ false);
            //if (characterIndex >= 0)
            //{
            //    textBox.Select(characterIndex, 0);
            //    return true;
            //}

            return false;
        }

        #endregion

       #region Data

        private static Style _defaultElementStyle;
        private static Style _defaultEditingElementStyle;

        #endregion
    }
}
