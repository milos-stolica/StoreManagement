namespace StoreManagement.API.Repositories
{
    /// <summary>
    /// CRUD operation entity status
    /// </summary>
    public enum EntityStatus
    {
        UpdatedSuccessfully,
        UnprocessableUpdate,
        NotUpdated,
        NotFound,
        None
    }
}
