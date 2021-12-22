using System.Collections.Generic;
using System.Threading.Tasks;
using Attribute = My_Company.Models.Attribute;

namespace My_Company.Interfaces
{
    public interface ICategoryAttributesRepository : IRepositoryBase<Attribute>
    {
        Task<Attribute> GetAttributeById(int id);
        Task AddAttributeValue(int attributeId, List<string> values);
        Task<Attribute> GetAttributeWithCategoryAndValuesById(int id);
        Task<Attribute> GetAttributeWithCategoryAndValuesTrackedById(int id);
        Task<List<Attribute>> GetAttributesByCategoryId(int categoryId);
    }
}
