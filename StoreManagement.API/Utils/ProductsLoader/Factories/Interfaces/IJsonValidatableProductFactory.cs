using StoreManagement.API.Common;

namespace StoreManagement.API.Utils
{
    public interface IJsonValidatableProductFactory
    {
        IJsonProductFactory Factory { get; }

        IValidator Validator { get; }
    }
}
