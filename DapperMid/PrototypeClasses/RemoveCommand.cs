using DapperMid.DataTables;
using DapperMid.Interfaces.CtorInterfaces;

namespace DapperMid.PrototypeClasses
{

    class RemoveCommand<T> : IRemoveCommand<T> where T : Datatable
    {
        public string GetRemoveCommand()
        {
            return $"Delete {typeof(T).Name} from {typeof(T).Name}";
        }
    }
}
