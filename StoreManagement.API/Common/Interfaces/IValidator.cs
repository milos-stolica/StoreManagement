namespace StoreManagement.API.Common
{
    public interface IValidator
    {
        bool IsValid(object validatableObject);
    }
}
