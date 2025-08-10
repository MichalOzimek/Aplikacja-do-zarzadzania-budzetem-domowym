using ProjectSoftwareWorkshop.Models.Category;

namespace ProjectSoftwareWorkshop.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSoftwareWorkshop.Contracts;
using ProjectSoftwareWorkshop.Data;
using ProjectSoftwareWorkshop.Models.Category;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICategoriesRepository _categoriesRepository;
    
    public CategoriesController(IMapper mapper, ICategoriesRepository categoriesRepository)
    {
        _mapper = mapper;
        _categoriesRepository = categoriesRepository;
    }
    
    // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _categoriesRepository.GetAllAsync();
            var records = _mapper.Map<List<CategoryDto>>(categories);
            return Ok(records);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoriesRepository.GetAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CategoryDto>(category));
        }

    // PUT: api/Categories/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> PutCategory(int id, CategoryDto categoryDto)
    {
        if (id != categoryDto.Id)
        {
            return BadRequest();
        }

        var category = await _categoriesRepository.GetAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _mapper.Map(categoryDto, category);

        try
        {
            await _categoriesRepository.UpdateAsync(category);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CategoryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return Ok(_mapper.Map<CategoryDto>(category));
    }


    // POST: api/Categories
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoriesRepository.AddAsync(category);

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoriesRepository.GetAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoriesRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CategoryExists(int id)
        {
            return await _categoriesRepository.Exists(id);
        }
}