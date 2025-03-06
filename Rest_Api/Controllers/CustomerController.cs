using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rest_Api.Models;
using Rest_Api.Repositories;

namespace Rest_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;

        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
     [FromQuery] string? search,
     [FromQuery] string? sortBy,
     [FromQuery] int page = 1,
     [FromQuery] int pageSize = 10)
        {
            var (customers, totalRecords) = await _repository.GetAllAsync(search, sortBy, page, pageSize);
            return Ok(new { customers, totalRecords, totalPages = (int)Math.Ceiling((double)totalRecords / pageSize) });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            await _repository.AddAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Customer customer)
        {
            if (id != customer.Id) return BadRequest();

            await _repository.UpdateAsync(customer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
