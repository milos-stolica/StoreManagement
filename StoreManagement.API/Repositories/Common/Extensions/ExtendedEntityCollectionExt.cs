using System.Collections.Generic;
using System.Linq;

namespace StoreManagement.API.Repositories.Extensions
{
    public static class ExtendedEntityCollectionExt
    {
        public static IEnumerable<ExtendedEntity<T>> FilterBy<T>(this IEnumerable<ExtendedEntity<T>> extendedEntities, EntityStatus status)
        {
            return extendedEntities.Where(extendedEntity => extendedEntity.StatusCode == status);
        }

        public static IEnumerable<T> ToEntities<T>(this IEnumerable<ExtendedEntity<T>> extendedEntities)
        {
            return extendedEntities.Select(extendedEntity => extendedEntity.Entity);
        }
    }
}
