namespace StoreManagement.API.Repositories
{

    //todo make limitatations for T
    //todo cannot use init in 8.0 version
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
