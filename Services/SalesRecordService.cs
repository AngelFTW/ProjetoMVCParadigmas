using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;
using System.Globalization;

namespace SalesWebMVC.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMVCContext _context;

        public SalesRecordService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecords select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date.ToUniversalTime() >= minDate.Value.ToUniversalTime());
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date.ToUniversalTime() <= maxDate.Value.ToUniversalTime());
            }

            return await result
            .Include(x => x.Seller)
            .Include(x => x.Seller.Department)
            .OrderByDescending(x => x.Date.ToUniversalTime())
            .ToListAsync();
        }

        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecords select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date.ToUniversalTime() >= minDate.Value.ToUniversalTime());
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date.ToUniversalTime() <= maxDate.Value.ToUniversalTime());
            }

            return await result
            .Include(x => x.Seller)
            .Include(x => x.Seller.Department)
            .OrderByDescending(x => x.Date.ToUniversalTime())
            .GroupBy(x => x.Seller.Department)
            .ToListAsync();
        }
    }
}
