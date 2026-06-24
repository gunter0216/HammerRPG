using System;
using UnityEngine.UIElements;

namespace Game.Core.Utils.Common
{
    public static class UIToolkitUtils
    {
        /// <summary>
        /// Adds element to parent and returns it for chaining
        /// </summary>
        public static T Append<T>(this VisualElement root, T element) where T : VisualElement
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (element == null) throw new ArgumentNullException(nameof(element));
            
            root.Add(element);
            return element;
        }
        
        /// <summary>
        /// Adds element to parent and returns it for chaining
        /// </summary>
        public static T Append<T>(this VisualElement root, Func<T> builder) where T : VisualElement
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var element = builder.Invoke();
            root.Add(element);
            return element;
        }
    }
}