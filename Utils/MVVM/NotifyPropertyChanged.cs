using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// On property changed
        /// </summary>
        /// <param name="expression">Expression</param>
        protected void OnPropertyChanged(Expression<Func<object>> expression)
        {
            OnPropertyChanged(GetPropertyName(expression));
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Private static methods

        /// <summary>
        /// Get property name
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns>Returns </returns>
        public static string GetPropertyName(Expression<Func<object>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            MemberExpression memberExpression;

            if (expression.Body is UnaryExpression)
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            else
                memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException();

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentException();

            var getMethod = property.GetGetMethod(true);
            if (getMethod.IsStatic)
                throw new ArgumentException();

            return memberExpression.Member.Name;
        }

        #endregion
    }
}
