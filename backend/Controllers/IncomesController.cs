using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSoftwareWorkshop.Contracts;
using ProjectSoftwareWorkshop.Data;
using ProjectSoftwareWorkshop.Models.Income;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectSoftwareWorkshop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IncomesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IIncomesRepository _incomesRepository;

    public IncomesController(IMapper mapper, IIncomesRepository incomesRepository)
    {
        _mapper = mapper;
        _incomesRepository = incomesRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncomeDto>>> GetIncomes()
    {
        var incomes = await _incomesRepository.GetAllAsync();
        var records = _mapper.Map<List<IncomeDto>>(incomes);
        return Ok(records);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IncomeDto>> GetIncome(int id)
    {
        var income = await _incomesRepository.GetAsync(id);
        if (income == null) return NotFound();

        return Ok(_mapper.Map<IncomeDto>(income));
    }

    [HttpPost]
    public async Task<ActionResult<IncomeDto>> PostIncome(IncomeDto incomeDto)
    {
        var income = _mapper.Map<Income>(incomeDto);

        // pobieranie kontekstu rêcznie (dodaj jako zale¿noœæ jeœli trzeba)
        var context = (ProjectSoftwareWorkshopDbContext?)HttpContext.RequestServices.GetService(typeof(ProjectSoftwareWorkshopDbContext));
        if (context == null) return StatusCode(500, "Brak kontekstu bazy danych.");

        var account = await context.Accounts.FirstOrDefaultAsync(a => a.Id == incomeDto.AccountId);
        if (account == null)
        {
            return NotFound("Nie znaleziono konta o podanym ID.");
        }

        // dodaj wp³yw i zaktualizuj saldo konta
        await _incomesRepository.AddAsync(income);
        account.Balance += income.Amount;

        await context.SaveChangesAsync(); // zapisujemy zmiany w koncie

        return CreatedAtAction(nameof(GetIncome), new { id = income.Id }, _mapper.Map<IncomeDto>(income));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<IncomeDto>> PutIncome(int id, IncomeDto incomeDto)
    {
        if (id != incomeDto.Id) return BadRequest();

        var income = await _incomesRepository.GetAsync(id);
        if (income == null) return NotFound();

        var context = (ProjectSoftwareWorkshopDbContext?)HttpContext.RequestServices.GetService(typeof(ProjectSoftwareWorkshopDbContext));
        if (context == null) return StatusCode(500, "Brak kontekstu bazy danych.");

        var account = await context.Accounts.FirstOrDefaultAsync(a => a.Id == incomeDto.AccountId);
        if (account == null) return NotFound("Nie znaleziono konta o podanym ID.");

        _mapper.Map(incomeDto, income);
        income.Account = account;

        try
        {
            await _incomesRepository.UpdateAsync(income);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _incomesRepository.Exists(id)) return NotFound();
            throw;
        }

        var updatedDto = _mapper.Map<IncomeDto>(income);
        return Ok(updatedDto);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIncome(int id)
    {
        var income = await _incomesRepository.GetAsync(id);
        if (income == null) return NotFound();

        await _incomesRepository.DeleteAsync(id);
        return NoContent();
    }
}
