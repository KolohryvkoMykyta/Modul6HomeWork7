namespace Catalog.Host.Models.Response
{
    public class GetByIdResponse<T>
        where T : class
    {
        public T Value { get; set; } = null!;
    }
}
