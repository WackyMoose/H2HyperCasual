public interface IWrapper<TEntity>
    where TEntity : class
{
    TEntity Data { get; set; }
    bool ContainsError { get; set; }
    string ErrorMessage { get; set; }
}