namespace StoreManagement.API.Common
{
    public interface IErrorMessageFactory<T>
    {
        string CreateErrorMessage(T errorModel);
    }
}
