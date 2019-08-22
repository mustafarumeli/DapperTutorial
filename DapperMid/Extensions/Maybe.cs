using DapperMid.Crud;
using DapperMid.DataTables;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DapperMid.Extensions
{
    public static class Maybe
    {
        /// <summary>
        /// Standart MaybeMonad Pattern's Do Method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="eval"></param>
        public static void Do<T>(this IEnumerable<T> self, Action<T> eval)
        {
            foreach (T item in self)
            {
                eval(item);
            }
        }

        /// <summary>
        /// This Method is for including Foreign Key's value to the end result of select statement
        /// </summary>
        /// <typeparam name="T">DataTable</typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="self">Ctor</param>
        /// <param name="property"></param>
        /// <returns>Self for Fluent Purposes</returns>
        public static Crud<T> Include<T, P>(this Crud<T> self, Expression<Func<T, P>> property) where T : DataTable
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            PropertyInfo prop;
            if (property.Body is UnaryExpression unaryExp && unaryExp.Operand is MemberExpression memberExp)
            {
                prop = (PropertyInfo)memberExp.Member;
                self.AddToForeignKeyList(prop.PropertyType);
                self.AddToProperties(prop);
            }
            else if (property.Body is MemberExpression pMemberExp)
            {
                prop = (PropertyInfo)pMemberExp.Member;
                self.AddToForeignKeyList(prop.PropertyType);
                self.AddToProperties(prop);
            }
            return self;
        }
    }
}
