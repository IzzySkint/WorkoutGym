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
public class MemberController : Controller
{
    private readonly IMapper _mapper;
    private readonly IMemberRepository _repository;
    private readonly ILogger _logger;
    
    public MemberController(IMapper mapper, IMemberRepository repository, ILogger<MemberController> logger)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
    }
    
    [HttpGet]
    [AjaxOnly]
    [Route("getWorkoutAreas")]
    public async Task<IActionResult> GetWorkoutAreas()
    {
        try
        {
            var result = await _repository.GetWorkoutAreasAsync();

            var model = _mapper.Map<IEnumerable<WorkoutAreaModel>>(result);

            return Ok(model);
        }
        catch (RepositoryException e)
        {
            _logger.LogError(e, $"Repository error {nameof(GetWorkoutAreas)}");
            return StatusCode(500, "Server Error");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error {nameof(GetWorkoutAreas)}");
            return StatusCode(500, "Server Error");
        }
    }

    [HttpGet]
    [AjaxOnly]
    [Route("getWorkoutAreaSessions")]
    public async Task<IActionResult> GetWorkoutAreaSessions(int workoutAreaId, DateTime date)
    {
        try
        {
            var result = await _repository.GetWorkoutAreaSessionCountsByDateAsync(workoutAreaId, date);

            var model = _mapper.Map<IEnumerable<WorkoutAreaSessionModel>>(result);

            return Ok(model);
        }
        catch (RepositoryException e)
        {
            _logger.LogError(e, $"Repository error {nameof(GetWorkoutAreaSessions)}");
            return StatusCode(500, "Server Error");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error {nameof(GetWorkoutAreaSessions)}");
            return StatusCode(500, "Server Error");
        }
    }

    [HttpPost]
    [AjaxOnly]
    [Route("createMemberSessionBooking")]
    public async Task<IActionResult> CreateMemberSessionBooking(MemberSessionBookingModel model)
    {
        try
        {
            var result = await _repository.CreateMemberSessionBookingAsync(_mapper.Map<MemberSessionBooking>(model));

            var sessionModel = _mapper.Map<MemberSessionModel>(result);

            return Ok(sessionModel);
        }
        catch (RepositoryException e)
        {
            _logger.LogError(e,$"Repository error {nameof(CreateMemberSessionBooking)}");
            return StatusCode(500, "Server Error");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error {nameof(CreateMemberSessionBooking)}");
            return StatusCode(500, "Server Error");
        }
    }

    [HttpGet]
    [AjaxOnly]
    [Route("getMemberSessionsByDate")]
    public async Task<IActionResult> GetMemberSessionsByDate(string userId, DateTime date)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("Invalid user id");
        }
        
        try
        {
            var result = await _repository.GetMemberSessionsByDateAsync(userId, date);

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
    public async Task<IActionResult> GetMemberSessionsByDateRange(string userId, DateTime startDate, DateTime endDate)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("Invalid user id");
        }
        
        if (startDate > endDate)
        {
            return BadRequest("Invalid start date");
        }

        try
        {
            var result = await _repository.GetMemberSessionsByDateRangeAsync(userId, startDate, endDate);

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

    [HttpPost]
    [AjaxOnly]
    [Route("isMemberSessionBookingValid")]
    public async Task<IActionResult> IsMemberSessionBookingValid(MemberSessionBookingModel model)
    {
        try
        {
            var result =
                await _repository.MemberSessionBookingValidityCheckAsync(_mapper.Map<MemberSessionBooking>(model));

            var validityCheckResultModel = _mapper.Map<BookingValidityCheckResultModel>(result);

            return Ok(validityCheckResultModel);
        }
        catch (RepositoryException e)
        {
            _logger.LogError(e, $"Repository error {nameof(IsMemberSessionBookingValid)}");
            return StatusCode(500, "Server Error");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error {nameof(IsMemberSessionBookingValid)}");
            return StatusCode(500, "Server Error");
        }
    }
}