using System.Collections.Generic;
using System.Reflection;

namespace DapperMid.Interfaces.CtorInterfaces
{

    public interface IGetSelectCommand
    {
        string GetSelectWithJoin(string joinDirection, bool isDistict, int top, HashSet<PropertyInfo> props);
    }
}
