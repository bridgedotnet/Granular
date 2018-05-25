namespace System.Windows.Controls
{
    /// <summary>
    /// VirtualizationCacheLengthUnit enum is used to indicate what kind of value the 
    /// VirtualizationCacheLength is holding.
    /// </summary>
    // Note: Keep the VirtualizationCacheLengthUnit enum in sync with the string representation 
    //       of units (VirtualizationCacheLengthConverter._unitString). 
    public enum VirtualizationCacheLengthUnit
    {
        /// <summary>
        /// The value is expressed as a pixel.
        /// </summary>
        Pixel,
        /// <summary>
        /// The value is expressed as an item.
        /// </summary>
        Item,
        /// <summary>
        /// The value is expressed as a page full of item.
        /// </summary>
        Page,
    }
}