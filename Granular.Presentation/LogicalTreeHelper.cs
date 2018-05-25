
namespace System.Windows
{
    /// <summary>
    /// Static helper functions for dealing with the logical tree
    /// </summary>
    public static class LogicalTreeHelper
    {/// <summary>
        /// Get the logical parent of the given DependencyObject.
        /// The given DependencyObject must be either a FrameworkElement or FrameworkContentElement
        /// to have a logical parent.
        /// </summary>
        public static DependencyObject GetParent(DependencyObject current)
        {
            if (current == null)
            {
                throw new ArgumentNullException("current");
            }

            FrameworkElement fe = current as FrameworkElement;
            if (fe != null)
            {
                return fe.LogicalParent;
            }

            //FrameworkContentElement fce = current as FrameworkContentElement;
            //if (fce != null)
            //{
            //    return fce.LogicalParent;
            //}

            return null;
        }

    }
}
