using System;
using Microsoft.Extensions.Logging;
using StoreManagement.API.Common;
using StoreManagement.API.Entities;

namespace StoreManagement.API.Utils
{
    public class JsonValidatableFruitFactory : JsonValidatableProductFactory
    {
        #region Fields

        private ILogger<FruitValidator> logger;

        #endregion Fields

        public JsonValidatableFruitFactory(ILogger<FruitValidator> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region JsonValidatableProductFactory members

        public override IJsonProductFactory Factory => factory ??= new JsonFruitFactory();

        public override IValidator Validator => validator ??= new FruitValidator(logger);

        #endregion JsonValidatableProductFactory members
    }
}
