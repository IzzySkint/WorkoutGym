using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutGym.Attributes;
using WorkoutGym.Data;
using WorkoutGym.Models;

namespace WorkoutGym.Controllers.Api;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AdminController : Controller
{
    private readonly IMapper _mapper;
    private readonly IAdminRepository _repository;
    private readonly ILogger _logger;
    
    public AdminController(IMapper mapper, IAdminRepository repository, ILogger<AdminController> logger)
    {
        this._mapper = mapper;
        this._repository = repository;
        this._logger = logger;
    }

    [HttpGet]
    [AjaxOnly]
    [Route("getMemberSessionsByDate")]
    public async Task<IActionResult> GetMemberSessionsByDate(DateTime date)
    {
        try
        {
            var result = await _repository.GetMemberSessionsByDateAsync(date);

            var model = _mapper.Map<IEnumerable<MemberSessionModel>>(result);

            return Ok(model);
        }
        catch (RepositoryException e)
        {
            _logger.LogError(e, $"Repository error {nameof(GetMemberSessionsByDate)}");
            return StatusCode(500, "Server Error");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error {nameof(GetMemberSessionsByDate)}");
            return StatusCode(500, "Server Error");
        }
    }

    [HttpGet]
    [AjaxOnly]
    [Route("getMemberSessionsByDateRange")]
    public async Task<IActionResult> GetMemberSessionsByDateRange(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("Invalid start date");
        }

        try
        {
            var result = await _repository.GetMemberSessionsByDateRangeAsync(startDate, endDate);

            var model = _mapper.Map<IEnumerable<MemberSessionModel>>(result);

            return Ok(model);
        }
        catch (RepositoryException e)
        {
            _logger.LogError(e, $"Repository error {nameof(GetMemberSessionsByDateRange)}");
            return StatusCode(500, "Server Error");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error {nameof(GetMemberSessionsByDateRange)}");
            return StatusCode(500, "Server Error");
        }
    }
}