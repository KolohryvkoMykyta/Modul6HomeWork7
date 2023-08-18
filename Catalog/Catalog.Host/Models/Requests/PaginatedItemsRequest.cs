using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Catalog.Host.Models.Enums;
using Newtonsoft.Json.Converters;

namespace Catalog.Host.Models.Requests;

public class PaginatedItemsRequest<T>
    where T : notnull
{
    [Required]
    [Range(1, 100)]
    public int PageIndex { get; set; }

    [Required]
    [Range(1, 20)]
    public int PageSize { get; set; }

    public Dictionary<CatalogTypeFilter, int>? Filters { get; set; }
}