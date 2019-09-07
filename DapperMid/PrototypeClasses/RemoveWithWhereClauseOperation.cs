using Dapper;
using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace DapperMid.PrototypeClasses
{
    static class GenerateWhere
    {
        static IDictionary<string, string> _expToSqlDict = new Dictionary<string, string>
        {
            ["AndAlso"] = "And",
            ["OrElse"] = "Or",
            ["=="] = "=",
            ["\""] = "\'"
        };

        public static string Generate(string paramaterName, string expressionText)
        {
            paramaterName = paramaterName + '.';
            expressionText = expressionText.Replace(paramaterName, "");
            foreach (var item in _expToSqlDict)
            {
                expressionText = expressionText.Replace(item.Key, item.Value);
            }
            return expressionText;
        }

    }
    class RemoveWithWhereClauseOperation<T> : IRemoveWithWhere<T> where T : Datatable
    {
        readonly SqlConnection _db;
        readonly IRemoveCommand<T> removeCommand;

        public RemoveWithWhereClauseOperation(SqlConnection db, IRemoveCommand<T> removeCommand)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            this.removeCommand = removeCommand ?? throw new ArgumentNullException(nameof(removeCommand));
        }
        List<Type> types = new List<Type>();
        void CheckBody(BinaryExpression biExp)
        {
            if (biExp.Left.NodeType == ExpressionType.MemberAccess)
            {
                var type = ((MemberExpression)biExp.Left).Expression.Type;
                types.Add(type);
                CheckBody((BinaryExpression)((BinaryExpression)biExp.Left).Right);
            }
            CheckBody((BinaryExpression)biExp.Left);

            if (biExp.Right.NodeType == ExpressionType.MemberAccess)
            {
                var type = ((MemberExpression)biExp.Right).Expression.Type;
                types.Add(type);
            }
            CheckBody((BinaryExpression)biExp);
        }
        public int RemeoveWithWhereClause(Expression<Predicate<T>> expression, object obj)
        {
            var command = removeCommand.GetRemoveCommand();
            var bExp = (BinaryExpression)expression.Body;
            var name = CheckBody(bExp);
            var dView = bExp.ToString();
            string whereClause = "";
            if (expression.Parameters.Count == 1)
            {
                whereClause = GenerateWhere.Generate(expression.Parameters[0].ToString(), dView);
            }
            command += " Where " + whereClause;
            return _db.Execute(command, obj);
        }
    }
}