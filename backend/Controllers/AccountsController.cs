using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSoftwareWorkshop.Contracts;
using ProjectSoftwareWorkshop.Data;
using ProjectSoftwareWorkshop.Models.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectSoftwareWorkshop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAccountsRepository _accountsRepository;

    public AccountsController(IMapper mapper, IAccountsRepository accountsRepository)
    {
        _mapper = mapper;
        _accountsRepository = accountsRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
    {
        var accounts = await _accountsRepository.GetAllAsync();
        var records = _mapper.Map<List<AccountDto>>(accounts);
        return Ok(records);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetAccount(int id)
    {
        var account = await _accountsRepository.GetAsync(id);
        if (account == null) return NotFound();

        return Ok(_mapper.Map<AccountDto>(account));
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> PostAccount(AccountDto accountDto)
    {
        var account = _mapper.Map<Account>(accountDto);
        await _accountsRepository.AddAsync(account);
        return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, _mapper.Map<AccountDto>(account));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AccountDto>> PutAccount(int id, AccountDto accountDto)
    {
        if (id != accountDto.Id) return BadRequest();

        var account = await _accountsRepository.GetAsync(id);
        if (account == null) return NotFound();

        _mapper.Map(accountDto, account);

        try
        {
            await _accountsRepository.UpdateAsync(account);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _accountsRepository.Exists(id)) return NotFound();
            throw;
        }

        var updatedDto = _mapper.Map<AccountDto>(account);
        return Ok(updatedDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var account = await _accountsRepository.GetAsync(id);
        if (account == null) return NotFound();

        await _accountsRepository.DeleteAsync(id);
        return NoContent();
    }
}
