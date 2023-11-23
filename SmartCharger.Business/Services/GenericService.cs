using Microsoft.EntityFrameworkCore;
using SmartCharger.Business.Interfaces;
using SmartCharger.Data;
using SmartCharger.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharger.Business.Services
{
    public class GenericService<TEntity> : IService where TEntity : BaseEntity
    {
        protected readonly SmartChargerContext _context;

        public GenericService(SmartChargerContext context)
        {
            _context = context;
        }

        public virtual async Task DeleteAsync(int id)
        {
            await _context.Set<TEntity>().Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}
