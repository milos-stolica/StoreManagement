using StoreManagement.API.Common.Extensions;
using StoreManagement.API.Utils;

namespace StoreManagement.API.Entities
{
    public class ProductValidator : ProductValidatorBase
    {
        #region Fields

        protected const float minDefault = 0;
        protected const float maxDefault = float.MaxValue;
        //assume max length
        protected const int maxStringLengthDefault = 255;
        //assume pattern based on https://www.iban.com/currency-codes
        private const string currencyPattern = @"^[A-Z]{3}$";

        #endregion Fields

        #region ProductValidatorBase members

        protected override bool ValidateProduct(object validatableObject)
        {
            ArgumentValidator.ThrowOnUnexpectedType(validatableObject, typeof(Product));

            Product product = validatableObject as Product;

            ArgumentValidator.ThrowIfNullOrEmpty(product.Sku, nameof(product.Sku));
            ArgumentValidator.ThrowIfNullOrEmpty(product.Name, nameof(product.Name));
            ArgumentValidator.ThrowIfNullOrEmpty(product.Currency, nameof(product.Currency));
            ArgumentValidator.ThrowIfNullOrEmpty(product.Type, nameof(product.Type));

            return !LongerThanDefault(product.Sku) &&
                   !LongerThanDefault(product.Name) &&
                   !LongerThanDefault(product.Type) &&
                   MatchPattern(product.Currency, currencyPattern) &&
                   IsBetweenDefault(product.UnitWeight) &&
                   IsBetweenDefault(product.Quantity) &&
                   IsBetweenDefault(product.Price);
        }

        #endregion ProductValidatorBase members

        #region Protected methods

        protected bool LongerThanDefault(string str)
        {
            return str.LongerThan(maxStringLengthDefault);
        }

        protected bool IsBetweenDefault(float num)
        {
            return num.IsBetween(minDefault, maxDefault);
        }

        protected bool MatchPattern(string str, string pattern)
        {
            return str.Match(pattern);
        }

        #endregion Protected methods
    }
}
