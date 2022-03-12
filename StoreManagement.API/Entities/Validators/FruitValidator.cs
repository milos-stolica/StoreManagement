using System;
using StoreManagement.API.Utils;

namespace StoreManagement.API.Entities
{
    public class FruitValidator : ProductValidator
    {
        protected override bool ValidateProduct(object validatableObject)
        {
            ArgumentValidator.ThrowOnUnexpectedType(validatableObject, typeof(Fruit));

            Fruit fruit = validatableObject as Fruit;

            ArgumentValidator.ThrowIfNullOrEmpty(fruit.Subtype, nameof(fruit.Subtype));

            return base.ValidateProduct(validatableObject) &&
                   !LongerThanDefault(fruit.Subtype);
        }
    }
}
