using Microsoft.AspNetCore.Mvc;
using Virtual_health_api.Services;
using Virtual_health_api.Models;

namespace Virtual_health_api.Controllers;

[ApiController]
[Route("[controller]")]
public class VitalsController : ControllerBase
{
    private readonly FirestoreDBService<Vitals> _firestoreRepo;

    private readonly ILogger<VitalsController> _logger;

    public VitalsController(ILogger<VitalsController> logger, FirestoreDBService<Vitals> firestoreRepo)
    {
        _logger = logger;
        _firestoreRepo = firestoreRepo;
    }

    [HttpPost]
    public async Task<IActionResult> Add(Vitals vital)
    {
        await _firestoreRepo.AddAsync(vital.User_Id, vital);
        return Ok("vital added");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var vital = await _firestoreRepo.GetAsync(id);
        return vital is not null ? Ok(vital) : NotFound();
    }

    [HttpPut]
    public async Task<IActionResult> Update(Vitals vital)
    {
        await _firestoreRepo.UpdateAsync(vital.User_Id, vital);
        return Ok("vital updated");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _firestoreRepo.DeleteAsync(id);
        return Ok("vital deleted");
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var vitals = await _firestoreRepo.GetAllAsync();
        return Ok(vitals);
    }
}
