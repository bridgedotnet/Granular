namespace System.Windows.Controls
{
    /// <summary>
    ///     Represents a node in a linked list used to track active containers.
    ///     Containers should instantiate and references these.
    ///     Parents hold onto the linked list.
    ///     
    ///     The list is iterated in order to call a variety of methods on containers
    ///     in response to changes on the parent.
    /// </summary>
    internal class ContainerTracking<T>
    {
        internal ContainerTracking(T container)
        {
            _container = container;
        }

        /// <summary>
        ///     The row container that this object represents.
        /// </summary>
        internal T Container
        {
            get { return _container; }
        }

        /// <summary>
        ///     The next node in the list.
        /// </summary>
        internal ContainerTracking<T> Next
        {
            get { return _next; }
        }

        /// <summary>
        ///     The previous node in the list.
        /// </summary>
        internal ContainerTracking<T> Previous
        {
            get { return _previous; }
        }

        /// <summary>
        ///     Adds this tracker to the list of active containers.
        /// </summary>
        /// <param name="root">The root of the list.</param>
        internal void StartTracking(ref ContainerTracking<T> root)
        {
            // Add the node to the root
            if (root != null)
            {
                root._previous = this;
            }

            _next = root;
            root = this;
        }

        /// <summary>
        ///     Removes this tracker from the list of active containers.
        /// </summary>
        /// <param name="root">The root of the list.</param>
        internal void StopTracking(ref ContainerTracking<T> root)
        {
            // Unhook the node from the list
            if (_previous != null)
            {
                _previous._next = _next;
            }

            if (_next != null)
            {
                _next._previous = _previous;
            }

            // Update the root reference
            if (root == this)
            {
                root = _next;
            }

            // Clear the node's references
            _previous = null;
            _next = null;
        }

        private T _container;
        private ContainerTracking<T> _next;
        private ContainerTracking<T> _previous;
    }
}
