using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Utils.Common;
using NotificationService.InternalModels.DTOs;
using NotificationService.Services;

namespace NotificationService.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<NotificationDto>>>> GetAll([FromQuery] string? status)
    {
        var notifications = await _notificationService.GetAllAsync(status);
        return Ok(ApiResponse<IEnumerable<NotificationDto>>.Ok(notifications));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<NotificationDto>>> GetById(int id)
    {
        var notification = await _notificationService.GetByIdAsync(id);
        if (notification is null)
        {
            return NotFound(ApiResponse<NotificationDto>.Fail("Notification not found"));
        }

        return Ok(ApiResponse<NotificationDto>.Ok(notification));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<NotificationDto>>> Create([FromBody] CreateNotificationDto dto)
    {
        var notification = await _notificationService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = notification.Id }, ApiResponse<NotificationDto>.Ok(notification, "Notification queued"));
    }

    [HttpPost("{id:int}/send")]
    public async Task<ActionResult<ApiResponse<NotificationDto>>> Send(int id)
    {
        var notification = await _notificationService.SendAsync(id);
        if (notification is null)
        {
            return NotFound(ApiResponse<NotificationDto>.Fail("Notification not found"));
        }

        return Ok(ApiResponse<NotificationDto>.Ok(notification, "Notification sent"));
    }

    [HttpPost("{id:int}/fail")]
    public async Task<ActionResult<ApiResponse<NotificationDto>>> MarkFailed(int id, [FromBody] FailureReasonDto dto)
    {
        var notification = await _notificationService.MarkFailedAsync(id, dto.Reason);
        if (notification is null)
        {
            return NotFound(ApiResponse<NotificationDto>.Fail("Notification not found"));
        }

        return Ok(ApiResponse<NotificationDto>.Ok(notification, "Notification marked as failed"));
    }
}


