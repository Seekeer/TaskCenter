
using System;
using System.Windows;
using System.Windows.Media;

namespace Utils.Helpers
{
    class WPFHelper
    {
        public static object GetParent(FrameworkElement descendant, Type parentType)
        {
            DependencyObject parent = descendant;
            do
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            while (parent != null && (parent.GetType().BaseType != parentType && 
                parent.GetType() != parentType));

            return parent;
        }
    }
}
