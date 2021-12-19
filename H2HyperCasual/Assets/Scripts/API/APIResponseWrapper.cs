public class APIResponseWrapper<TEntity> : IWrapper<TEntity>
    where TEntity : class
{
    public TEntity Data { get; set; }
    public bool ContainsError { get; set; }
    public string ErrorMessage { get; set; }

    public static APIResponseWrapper<TEntity> Success(TEntity data)
    {
        return new APIResponseWrapper<TEntity>
        {
            Data = data,
            ContainsError = false,
            ErrorMessage = string.Empty,
        };
    }

    public static APIResponseWrapper<TEntity> Error(string errorMessage)
    {
        return new APIResponseWrapper<TEntity>
        {
            Data = null,
            ContainsError = true,
            ErrorMessage = errorMessage
        };
    }
}