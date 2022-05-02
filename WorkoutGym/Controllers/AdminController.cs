using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutGym.Data;
using WorkoutGym.Models;
using WorkoutGym.Reports;

namespace WorkoutGym.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IReportsService _reportsService;
    private readonly IAdminRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    
    public AdminController(
        IReportsService reportsService, 
        IAdminRepository repository, 
        IMapper mapper,
        ILogger<AdminController> logger)
    {
        this._reportsService = reportsService;
        this._repository = repository;
        this._mapper = mapper;
        this._logger = logger;
    }
    
    [HttpGet]
    public IActionResult Dashboard()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ViewSessions()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DownloadDailyTimeTable()
    {
        string fileName = $"TimeTable_{DateTime.Now.ToString("yyyy-MM-dd")}.pdf";
        string contentType = "application/pdf";

        try
        {
            var result = await _repository.GetMemberSessionsByDateAsync(DateTime.Now);
            var data = this._mapper.Map<IEnumerable<MemberSessionModel>>(result);
            var htmlTemplate = new DailyTimeTableTemplate(data);
            byte[] report = await this._reportsService.GenerateReportAsync(htmlTemplate);

            return File(report, contentType, fileName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error generating daily time table report");
            return StatusCode(500, "Server Error");
        }
    }
}