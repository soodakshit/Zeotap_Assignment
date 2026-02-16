using IncidentTracker.Api.Models;

namespace IncidentTracker.Api.DTOs;

public record CreateIncidentRequest(
    string title,
    string service,
    Severity severity,
    Status status,
    string? owner,
    string? summary
);

public record UpdateIncidentRequest(
    string? title,
    string? service,
    Severity? severity,
    Status? status,
    string? owner,
    string? summary
);

public record IncidentResponse(
    Guid Id,
    string Title,
    string Service,
    Severity Severity,
    Status Status,
    string? Owner,
    string? Summary,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record PagedResponse<T>(
    List<T> Data,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);

public record IncidentQueryParams(
    int Page = 1,
    int PageSize = 10,
    string? Search = null,
    string? Service = null,
    Severity? Severity = null,
    Status? Status = null,
    string? SortBy = "createdAt",
    string? SortOrder = "desc"
);