namespace DapperMid.Interfaces
{
    interface IDatatable<T> where T : class
    {
        T Id { get; }
    }
}
