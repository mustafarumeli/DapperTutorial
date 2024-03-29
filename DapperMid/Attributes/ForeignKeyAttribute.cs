﻿using System;
namespace DapperMid.Attributes
{
    /// <summary>
    /// Identifies property as ForeignKey
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class ForeignKeyAttribute : Attribute
    {
        public string Name { get; }
        /// <summary>
        /// Name should be equal to column name in sql
        /// </summary>
        /// <param name="name">Name of Column in Sql</param>
        public ForeignKeyAttribute(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
