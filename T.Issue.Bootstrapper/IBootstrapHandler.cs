using System.Collections.Generic;
using System.IO;
using System.Linq;
using T.Issue.Bootstrapper.Model;

namespace T.Issue.Bootstrapper
{
    public interface IBootstrapHandler<TApplied, TClasspath> where TApplied : AppliedItem where TClasspath : ClasspathItem
    {
        /// <summary>
        /// Should collect already applied items.
        /// </summary>
        /// <returns>Already applied items</returns>
        IList<TApplied> GetAppliedItems();

        /// <summary>
        /// Should provide classpath item locations.
        /// </summary>
        /// <returns>Classpath item locations</returns>
        IList<ItemLocation> GetItemLocations();

        /// <summary>
        /// Should build classpath item.
        /// </summary>
        /// <param name="location">Item location.</param>
        /// <param name="itemName">Item name.</param>
        /// <param name="content">Content stream.</param>
        /// <returns>Classpath item</returns>
        TClasspath BuildClasspathItem(ItemLocation location, string itemName, Stream content);

        /// <summary>
        /// Should preprocess items, if required.
        /// </summary>
        /// <param name="appliedItems">Applied item list.</param>
        /// <param name="classpathItems">Classpath item list.</param>
        void PreprocessItems(IList<TApplied> appliedItems, IList<TClasspath> classpathItems);

        /// <summary>
        /// Should perform additional checks if pending calasspath item should be bootstrapped.
        /// </summary>
        /// <param name="appliedItem">Applied item, if exists.</param>
        /// <param name="classpathItem">Classpath item.</param>
        /// <returns>True if pending calasspath item should be applied, false otherwise.</returns>
        bool IsItemPending(TApplied appliedItem, TClasspath classpathItem);

        /// <summary>
        /// Should perform actual pending items bootstrapping.
        /// </summary>
        /// <param name="pendingItems">Pending classpath items.</param>
        void BootstrapPendingItems(IOrderedEnumerable<TClasspath> pendingItems);

        /// <summary>
        /// Should handle validation errors, if required.
        /// </summary>
        /// <param name="error">Validation error.</param>
        /// <param name="appliedItem">Already applied item.</param>
        /// <param name="classpathItem">Classpath item.</param>
        void OnValidationError(ErrorType error, TApplied appliedItem, TClasspath classpathItem);

        /// <summary>
        /// Should deside if bootstrapping should be terminated in case of validation errors.
        /// </summary>
        /// <returns>False if bootstrapping should be terminated, true otherwise.</returns>
        bool OnValidationErrors();
    }
}
