﻿using Microsoft.EntityFrameworkCore;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class PhotosRepository : RepositoryBase<Photo>, IPhotosRepository
    {
        public PhotosRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Photo>> GetPhotosByProduct(int productId)
        {
            return await FindByCondition(p => p.ProductId == productId).ToListAsync();
        }
    }
}
