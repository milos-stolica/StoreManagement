namespace StoreManagement.API.Repositories
{
    /// <summary>
    /// Models any entity with appropriate status (created, updated, not updated,...)
    /// Used to notify client classes about CRUD success
    /// </summary>
    public class ExtendedEntity<T>
    {
        public ExtendedEntity(EntityStatus statusCode, T entity)
        {
            StatusCode = statusCode;
            Entity = entity;
        }

        public EntityStatus StatusCode { get; private set; }

        public T Entity { get; private set; }
    }
}
