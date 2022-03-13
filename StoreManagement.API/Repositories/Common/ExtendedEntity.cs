namespace StoreManagement.API.Repositories
{
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
