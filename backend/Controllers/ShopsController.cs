using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSoftwareWorkshop.Contracts;
using ProjectSoftwareWorkshop.Data;
using ProjectSoftwareWorkshop.Models.Shop;

namespace ProjectSoftwareWorkshop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShopsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IShopsRepository _shopsRepository;
    
    public ShopsController(IMapper mapper, IShopsRepository shopsRepository)
    {
        _mapper = mapper;
        _shopsRepository = shopsRepository;
    }
    
    // GET: api/Shops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetShops()
        {
            var shops = await _shopsRepository.GetAllAsync();
            var records = _mapper.Map<List<ShopDto>>(shops);
            return Ok(records);
        }

        // GET: api/Shops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShopDto>> GetShop(int id)
        {
            var shop = await _shopsRepository.GetAsync(id);

            if (shop == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ShopDto>(shop));
        }

    // PUT: api/Shops/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<ShopDto>> PutShop(int id, ShopDto shopDto)
    {
        if (id != shopDto.Id)
            return BadRequest();

        var shop = await _shopsRepository.GetAsync(id);
        if (shop == null)
            return NotFound();

        _mapper.Map(shopDto, shop);

        try
        {
            await _shopsRepository.UpdateAsync(shop);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ShopExists(id))
                return NotFound();
            else
                throw;
        }

        return Ok(_mapper.Map<ShopDto>(shop));
    }

    // POST: api/Shops
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
        public async Task<ActionResult<Shop>> PostShop(ShopDto shopDto)
        {
            var shop = _mapper.Map<Shop>(shopDto);
            await _shopsRepository.AddAsync(shop);

            return CreatedAtAction("GetShop", new { id = shop.Id }, shop);
        }

        // DELETE: api/Shops/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            var shop = await _shopsRepository.GetAsync(id);
            if (shop == null)
            {
                return NotFound();
            }

            await _shopsRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> ShopExists(int id)
        {
            return await _shopsRepository.Exists(id);
        }
}