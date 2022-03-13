using Microsoft.Extensions.Logging;
using StoreManagement.API.Common;

namespace StoreManagement.API.Utils
{
    public abstract class JsonValidatableProductFactory : IJsonValidatableProductFactory
    {
        #region Fields

        protected IJsonProductFactory factory = null;
        protected IValidator validator = null;

        #endregion Fields

        #region IJsonValidatableProductFactory members

        public abstract IJsonProductFactory Factory { get; }

        public abstract IValidator Validator { get; }

        #endregion IJsonValidatableProductFactory members
    }
}
