using System;
using StoreManagement.API.Common;

namespace StoreManagement.API.Entities
{
    public abstract class ProductValidatorBase : IValidator
    {
        public bool IsValid(object validatableObject)
        {
            try
            {
                return ValidateProduct(validatableObject);
            }
            catch (ArgumentNullException ex)
            {
                //todo log
                return false;
            }
            catch (ArgumentException ex)
            {
                //todo log
                return false;
            }
        }

        #region Abstract methods

        protected abstract bool ValidateProduct(object validatableObject);

        #endregion Abstract methods
    }
}
