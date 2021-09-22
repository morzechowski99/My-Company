using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attribute = My_Company.Models.Attribute;

namespace My_Company.Interfaces
{
    public interface ICategoryAttributesRepository : IRepositoryBase<Attribute>
    {
        Task<Attribute> GetAttributeById(int id);
        Task AddAttributeValue(int attributeId, List<string> values);
        Task<Attribute> GetAttributeWithCategoryAndValuesById(int id);
    }
}
