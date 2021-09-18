using Microsoft.EntityFrameworkCore;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attribute = My_Company.Models.Attribute;

namespace My_Company.Repositories
{
    public class CategoryAttributesRepository : RepositoryBase<Attribute>, ICategoryAttributesRepository
    {
        public CategoryAttributesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddAttributeValue(int attributeId, List<string> values)
        {
            var attribute = await GetAttributeById(attributeId);
            attribute.AttributeDictionaryValues = new List<AttributeDictionaryValues>();
            foreach(var value in values)
            {
                attribute.AttributeDictionaryValues.Add(new AttributeDictionaryValues()
                {
                    Value= value
                });
            }

            Update(attribute);

        }

        public async Task<Attribute> GetAttributeById(int id)
        {
            return await FindByCondition(a => a.Id == id).FirstOrDefaultAsync();
        }
    }
}
