using DapperMid.Crud;
using DapperMid.DataTables;
using Dapper;
using System.Linq.Expressions;
using System;
using System.Data.SqlClient;

namespace DapperMid.Extensions
{

    public static class DbExtensions
    {

        /// <summary>
        /// Uses Dapper. Creates a select statement with count  
        /// </summary>
        /// <typeparam name="T">Database Table</typeparam>
        /// <param name="self">Crud we want count on</param>
        /// <returns></returns>
        public static int Count<T>(this Crud<T> self) where T : Datatable
        {
            SqlConnection con = self.GetCurrentSqlConnection();
            string sqlCommand = $"Select Count(Id) from {typeof(T).Name}";
            return (int)con.ExecuteScalar(sqlCommand);
        }
        /// <summary>
        /// Uses Dapper, Creates a select statetment which adds given column together and returns result  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P">Must be numerical but all i can do is struct and IConvertible</typeparam>
        /// <param name="self"></param>
        /// <param name="expression"></param>
        /// <returns>Summation of given column at expression</returns>
        public static P Sum<T, P>(this Crud<T> self, Expression<Func<T, P>> expression)
              where T : Datatable
              where P : struct, IConvertible
        {
            string name = "Id";
            if (expression.Body is UnaryExpression unaryExp && unaryExp.Operand is MemberExpression memberExp)
            {
                name = memberExp.Member.Name;
            }
            else if (expression.Body is MemberExpression pMemberExp)
            {
                name = pMemberExp.Member.Name;
            }
            else
            {
                throw new Exception("Not In Correct Format");
            }
            SqlConnection con = self.GetCurrentSqlConnection();
            string sqlCommand = $"Select Sum({name}) from {typeof(T).Name}";
            return (P)con.ExecuteScalar(sqlCommand);
        }
        /// <summary>
        /// Uses Dapper, Creates a select statetment to find minimum value of given column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P">Must be numerical but all i can do is struct and IConvertible</typeparam>
        /// <param name="self"></param>
        /// <param name="expression"></param>
        /// <returns>Minimum value  of given column</returns>
        public static P Min<T, P>(this Crud<T> self, Expression<Func<T, P>> expression)
            where T : Datatable
            where P : struct, IConvertible
        {
            string name = "Id";
            if (expression.Body is UnaryExpression unaryExp && unaryExp.Operand is MemberExpression memberExp)
            {
                name = memberExp.Member.Name;
            }
            else if (expression.Body is MemberExpression pMemberExp)
            {
                name = pMemberExp.Member.Name;
            }
            else
            {
                throw new Exception("Not In Correct Format");
            }
            SqlConnection con = self.GetCurrentSqlConnection();
            string sqlCommand = $"Select Min({name}) from {typeof(T).Name}";
            return (P)con.ExecuteScalar(sqlCommand);
        }
        /// <summary>
        /// Uses Dapper, Creates a select statetment to find maximum value of given column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P">Must be numerical but all i can do is struct and IConvertible</typeparam>
        /// <param name="self"></param>
        /// <param name="expression"></param>
        /// <returns>Max value  of given column</returns>
        public static P Max<T, P>(this Crud<T> self, Expression<Func<T, P>> expression)
            where T : Datatable
            where P : struct, IConvertible
        {
            string name = "Id";
            if (expression.Body is UnaryExpression unaryExp && unaryExp.Operand is MemberExpression memberExp)
            {
                name = memberExp.Member.Name;
            }
            else if (expression.Body is MemberExpression pMemberExp)
            {
                name = pMemberExp.Member.Name;
            }
            else
            {
                throw new Exception("Not In Correct Format");
            }
            SqlConnection con = self.GetCurrentSqlConnection();
            string sqlCommand = $"Select Max({name}) from {typeof(T).Name}";
            return (P)con.ExecuteScalar(sqlCommand);
        }
        /// <summary>
        /// Uses Dapper, Creates a select statetment to find avarage value of given column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P">Must be numerical but all i can do is struct and IConvertible</typeparam>
        /// <param name="self"></param>
        /// <param name="expression"></param>
        /// <returns>Avarage value  of given column</returns>
        public static double Avg<T, P>(this Crud<T> self, Expression<Func<T, P>> expression)
            where T : Datatable
            where P : struct, IConvertible
        {
            string name = "Id";
            if (expression.Body is UnaryExpression unaryExp && unaryExp.Operand is MemberExpression memberExp)
            {
                name = memberExp.Member.Name;
            }
            else if (expression.Body is MemberExpression pMemberExp)
            {
                name = pMemberExp.Member.Name;
            }
            else
            {
                throw new Exception("Not In Correct Format");
            }
            SqlConnection con = self.GetCurrentSqlConnection();
            string sqlCommand = $"Select Avg({name}) from {typeof(T).Name}";
            return (double)con.ExecuteScalar(sqlCommand);
        }
    }
}
