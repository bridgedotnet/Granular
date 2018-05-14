namespace System.Windows.Controls
{
    /// <summary>
    ///     Panel that lays out both cells and column headers. This stacks cells in the horizontal direction and communicates with the
    ///     relevant DataGridColumn to ensure all rows give cells in a given column the same size.
    ///     It is hardcoded against DataGridCell and DataGridColumnHeader.
    /// </summary>
    public class DataGridCellsPanel : StackPanel
    {
        public DataGridCellsPanel()
        {
            Orientation = Orientation.Horizontal;
        }
    }
}
