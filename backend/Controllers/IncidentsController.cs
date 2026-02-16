using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using IncidentTracker.Api.Data;
using IncidentTracker.Api.DTOs;
using IncidentTracker.Api.Models;

namespace IncidentTracker.Api.Controllers;

[ApiController]
[Route("api/incidents")]
public class IncidentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IValidator<CreateIncidentRequest> _createValidator;
    private readonly IValidator<UpdateIncidentRequest> _updateValidator;
    private readonly ILogger<IncidentsController> _logger;

    public IncidentsController(
        ApplicationDbContext context,
        IValidator<CreateIncidentRequest> createValidator,
        IValidator<UpdateIncidentRequest> updateValidator,
        ILogger<IncidentsController> logger)
    {
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<IncidentResponse>> CreateIncident(
        [FromBody] CreateIncidentRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        var incident = new Incident
{
    Id = Guid.NewGuid(),
    Title = request.title,      
    Service = request.service,  
    Severity = request.severity, 
    Status = request.status,    
    Owner = request.owner,       
    Summary = request.summary,   
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow
};

        _context.Incidents.Add(incident);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created incident {IncidentId}: {Title}", incident.Id, incident.Title);

        return CreatedAtAction(
            nameof(GetIncident),
            new { id = incident.Id },
            MapToResponse(incident));
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<IncidentResponse>>> GetIncidents(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? service = null,
        [FromQuery] Models.Severity? severity = null,
        [FromQuery] Models.Status? status = null,
        [FromQuery] string sortBy = "createdAt",
        [FromQuery] string sortOrder = "desc")
    {
        // Validate pagination
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _context.Incidents.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(i =>
                i.Title.ToLower().Contains(searchLower) ||
                i.Service.ToLower().Contains(searchLower) ||
                (i.Owner != null && i.Owner.ToLower().Contains(searchLower)) ||
                (i.Summary != null && i.Summary.ToLower().Contains(searchLower)));
        }

        if (!string.IsNullOrWhiteSpace(service))
        {
            query = query.Where(i => i.Service == service);
        }

        if (severity.HasValue)
        {
            query = query.Where(i => i.Severity == severity.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(i => i.Status == status.Value);
        }

        // Apply sorting
        query = sortBy.ToLower() switch
        {
            "title" => sortOrder.ToLower() == "asc" 
                ? query.OrderBy(i => i.Title) 
                : query.OrderByDescending(i => i.Title),
            "service" => sortOrder.ToLower() == "asc" 
                ? query.OrderBy(i => i.Service) 
                : query.OrderByDescending(i => i.Service),
            "severity" => sortOrder.ToLower() == "asc" 
                ? query.OrderBy(i => i.Severity) 
                : query.OrderByDescending(i => i.Severity),
            "status" => sortOrder.ToLower() == "asc" 
                ? query.OrderBy(i => i.Status) 
                : query.OrderByDescending(i => i.Status),
            "updatedat" => sortOrder.ToLower() == "asc" 
                ? query.OrderBy(i => i.UpdatedAt) 
                : query.OrderByDescending(i => i.UpdatedAt),
            _ => sortOrder.ToLower() == "asc" 
                ? query.OrderBy(i => i.CreatedAt) 
                : query.OrderByDescending(i => i.CreatedAt)
        };

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var incidents = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = new PagedResponse<IncidentResponse>(
            Data: incidents.Select(MapToResponse).ToList(),
            Page: page,
            PageSize: pageSize,
            TotalCount: totalCount,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageSize)
        );

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IncidentResponse>> GetIncident(Guid id)
    {
        var incident = await _context.Incidents.FindAsync(id);

        if (incident == null)
        {
            return NotFound(new { message = "Incident not found" });
        }

        return Ok(MapToResponse(incident));
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<IncidentResponse>> UpdateIncident(
        Guid id,
        [FromBody] UpdateIncidentRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        var incident = await _context.Incidents.FindAsync(id);
        if (incident == null)
        {
            return NotFound(new { message = "Incident not found" });
        }

        // Update only provided fields
if (!string.IsNullOrEmpty(request.title))
    incident.Title = request.title;

if (!string.IsNullOrEmpty(request.service))
    incident.Service = request.service;

if (request.severity.HasValue)
    incident.Severity = request.severity.Value;

if (request.status.HasValue)
    incident.Status = request.status.Value;

if (request.owner != null)
    incident.Owner = request.owner;

if (request.summary != null)
    incident.Summary = request.summary;

        incident.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated incident {IncidentId}", incident.Id);

        return Ok(MapToResponse(incident));
    }

    private static IncidentResponse MapToResponse(Incident incident)
    {
        return new IncidentResponse(
            Id: incident.Id,
            Title: incident.Title,
            Service: incident.Service,
            Severity: incident.Severity,
            Status: incident.Status,
            Owner: incident.Owner,
            Summary: incident.Summary,
            CreatedAt: incident.CreatedAt,
            UpdatedAt: incident.UpdatedAt
        );
    }
}