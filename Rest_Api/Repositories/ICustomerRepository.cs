﻿using System.Collections;
using Rest_Api.Models;

namespace Rest_Api.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(int id);
        Task<(IEnumerable<Customer> customers, int totalRecords)> GetAllAsync(string? search, string? sortBy, int page, int pageSize);
    }
}
