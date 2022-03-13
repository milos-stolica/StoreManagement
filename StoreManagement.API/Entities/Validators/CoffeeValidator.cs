using Microsoft.Extensions.Logging;
using StoreManagement.API.Utils;

namespace StoreManagement.API.Entities
{
    public class CoffeeValidator : ProductValidator
    {
        #region Constructor

        public CoffeeValidator(ILogger<CoffeeValidator> logger) : base(logger)
        {

        }

        #endregion Constructor

        #region ProductValidator members

        protected override bool ValidateProduct(object validatableObject)
        {
            ArgumentValidator.ThrowOnUnexpectedType(validatableObject, typeof(Coffee));

            Coffee coffee = validatableObject as Coffee;

            ArgumentValidator.ThrowIfNullOrEmpty(coffee.Origin, nameof(coffee.Origin));
            ArgumentValidator.ThrowIfNullOrEmpty(coffee.Roast, nameof(coffee.Roast));

            return base.ValidateProduct(validatableObject) &&
                   !LongerThanDefault(coffee.Origin) &&
                   !LongerThanDefault(coffee.Roast);
        }

        #endregion ProductValidator members
    }
}
