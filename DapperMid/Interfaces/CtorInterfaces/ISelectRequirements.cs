using System;
using System.Reflection;
namespace DapperMid.Interfaces.CtorInterfaces
{
    public interface ISelectRequirements
    {
        void AddToForeignKeyList(Type type);
        void AddToProperties(PropertyInfo property);
    }
}
