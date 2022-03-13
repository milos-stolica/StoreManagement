using System;
using Microsoft.Extensions.Logging;
using StoreManagement.API.Common;

namespace StoreManagement.API.Entities
{
    public abstract class ProductValidatorBase : IValidator
    {
        #region Fields

        private readonly ILogger logger;

        #endregion Fields

        #region Constructor

        public ProductValidatorBase(ILogger<ProductValidatorBase> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Constructor

        #region IValidator members

        public bool IsValid(object validatableObject)
        {
            try
            {
                return ValidateProduct(validatableObject);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning($"{ex}");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Unexpected exception occured while validating product: {ex}");
                return false;
            }
        }

        #endregion IValidator members

        #region Abstract methods

        protected abstract bool ValidateProduct(object validatableObject);

        #endregion Abstract methods
    }
}
