using DapperCrud.Interfaces.SqlCommadInterfaces;

namespace DapperCrud.AbstractClasses.SqlCommad
{
    public class SelectCommand<T> : IGetSelectSql<T> where T : DbTable
    {
        public string GetSelectCommand()
        {
            return "Select * from " + typeof(T).Name;
        }
    }

}
