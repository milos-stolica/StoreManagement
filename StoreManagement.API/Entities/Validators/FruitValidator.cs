using System;
using Microsoft.Extensions.Logging;
using StoreManagement.API.Utils;

namespace StoreManagement.API.Entities
{
    public class FruitValidator : ProductValidator
    {
        #region Constructor

        public FruitValidator(ILogger<FruitValidator> logger) : base(logger)
        {

        }

        #endregion Constructor

        #region ProductValidator members

        protected override bool ValidateProduct(object validatableObject)
        {
            ArgumentValidator.ThrowOnUnexpectedType(validatableObject, typeof(Fruit));

            Fruit fruit = validatableObject as Fruit;

            ArgumentValidator.ThrowIfNullOrEmpty(fruit.Subtype, nameof(fruit.Subtype));

            return base.ValidateProduct(validatableObject) &&
                   !LongerThanDefault(fruit.Subtype);
        }

        #endregion ProductValidator
    }
}
