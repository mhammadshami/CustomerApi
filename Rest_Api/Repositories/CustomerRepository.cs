using Microsoft.EntityFrameworkCore;
using Rest_Api.Data;
using Rest_Api.Models;

namespace Rest_Api.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                customer.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Customer>> GetAllAsync() => await _context.Customers.ToListAsync();

        public async Task<(IEnumerable<Customer> customers, int totalRecords)> GetAllAsync(string? search, string? sortBy, int page, int pageSize)
        {
            var query = _context.Customers
                .Where(c => !c.IsDeleted)  // استبعاد العملاء المحذوفين
                .AsNoTracking();

            // 1️⃣ الفلترة (Filtering)
            if (!string.IsNullOrEmpty(search))
                query = query.Where(c => c.Name.Contains(search) || c.Email.Contains(search));

            // 2️⃣ الترتيب (Sorting)
            query = sortBy?.ToLower() switch
            {
                "name" => query.OrderBy(c => c.Name),
                "email" => query.OrderBy(c => c.Email),
                _ => query.OrderBy(c => c.Id)
            };

            // 3️⃣ التقسيم إلى صفحات (Pagination)
            int totalRecords = await query.CountAsync();
            var customers = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (customers, totalRecords);
        }

        public async Task<Customer> GetByIdAsync(int id) => await _context.Customers.FindAsync(id);

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
