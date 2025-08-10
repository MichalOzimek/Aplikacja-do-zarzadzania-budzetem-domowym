using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSoftwareWorkshop.Contracts;
using ProjectSoftwareWorkshop.Data;
using ProjectSoftwareWorkshop.Models.Purchase;

namespace ProjectSoftwareWorkshop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PurchasesController : ControllerBase
{
        private readonly IMapper _mapper;
        private readonly IPurchasesRepository _purchasesRepository;

        public PurchasesController(IMapper mapper, IPurchasesRepository purchasesRepository)
        {
            _mapper = mapper;
            _purchasesRepository = purchasesRepository;
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> GetPurchases()
        {
            var purchases = await _purchasesRepository.GetAllAsync();
            var records = _mapper.Map<List<PurchaseDto>>(purchases);
            return Ok(records);
        }

        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseDto>> GetPurchase(int id)
        {
            var purchase = await _purchasesRepository.GetAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PurchaseDto>(purchase));
        }

    // PUT: api/Purchases/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPurchase(int id, PurchaseUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var context = (ProjectSoftwareWorkshopDbContext?)HttpContext.RequestServices.GetService(typeof(ProjectSoftwareWorkshopDbContext));
        if (context == null)
            return StatusCode(500, "Brak kontekstu bazy danych.");

        var purchase = await _purchasesRepository.GetAsync(id);
        if (purchase == null)
            return NotFound();

        // Pobierz stare i nowe konto
        var oldAccount = await context.Accounts.FirstOrDefaultAsync(a => a.Id == purchase.AccountId);
        var newAccount = await context.Accounts.FirstOrDefaultAsync(a => a.Id == dto.AccountId);
        if (newAccount == null)
            return NotFound("Nie znaleziono nowego konta.");

        // Zwróæ star¹ kwotê na stare konto
        if (oldAccount != null)
            oldAccount.Balance += purchase.BillCost;

        // Odejmij now¹ kwotê z nowego konta
        newAccount.Balance -= dto.BillCost;

        // Zaktualizuj dane zakupu
        purchase.Note = dto.Note;
        purchase.BillCost = dto.BillCost;
        purchase.Date = dto.Date;
        purchase.AccountId = dto.AccountId;
        purchase.CategoryId = dto.CategoryId;
        if (dto.ShopId.HasValue)
            purchase.ShopId = dto.ShopId.Value;

        try
        {
            await _purchasesRepository.UpdateAsync(purchase);
            await context.SaveChangesAsync(); // zapisz zmiany sald
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _purchasesRepository.Exists(id))
                return NotFound();
            throw;
        }

        return Ok(_mapper.Map<PurchaseDto>(purchase));
    }




    // POST: api/Purchases
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Purchase>> PostPurchase(PurchaseCreateDto purchaseCreateDto)
    {
        var context = (ProjectSoftwareWorkshopDbContext?)HttpContext.RequestServices.GetService(typeof(ProjectSoftwareWorkshopDbContext));
        if (context == null)
            return StatusCode(500, "Brak kontekstu bazy danych.");

        var purchase = _mapper.Map<Purchase>(purchaseCreateDto);

        // znajdŸ konto
        var account = await context.Accounts.FirstOrDefaultAsync(a => a.Id == purchaseCreateDto.AccountId);
        if (account == null)
            return NotFound("Nie znaleziono konta o podanym ID.");

        // odejmij kwotê z konta
        account.Balance -= purchase.BillCost;

        await _purchasesRepository.AddAsync(purchase);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchase);
    }

    // DELETE: api/Purchases/5
    [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            var purchase = await _purchasesRepository.GetAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            await _purchasesRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> PurchaseExists(int id)
        {
            return await _purchasesRepository.Exists(id);
        }
}