using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Utils.Common;
using NotificationService.InternalModels.DTOs;
using NotificationService.InternalModels.Entities;
using NotificationService.Services;

namespace NotificationService.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<IEnumerable<NotificationDto>>> GetAll([FromQuery] string? status)
    {
        var query = NotificationStore.Notifications.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(x => x.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(ApiResponse<IEnumerable<NotificationDto>>.Ok(query.Select(NotificationDto.FromEntity).ToList()));
    }

    [HttpGet("{id:int}")]
    public ActionResult<ApiResponse<NotificationDto>> GetById(int id)
    {
        var notification = NotificationStore.Notifications.FirstOrDefault(x => x.Id == id);
        if (notification is null)
        {
            return NotFound(ApiResponse<NotificationDto>.Fail("Notification not found"));
        }

        return Ok(ApiResponse<NotificationDto>.Ok(NotificationDto.FromEntity(notification)));
    }

    [HttpPost]
    public ActionResult<ApiResponse<NotificationDto>> Create([FromBody] CreateNotificationDto dto)
    {
        var id = Interlocked.Increment(ref NotificationStore.NotificationSeed);
        var notification = new NotificationEntity
        {
            Id = id,
            Recipient = dto.Recipient,
            Channel = dto.Channel,
            Subject = dto.Subject,
            Message = dto.Message,
            Status = "Queued",
            CreatedAt = DateTime.UtcNow,
            SentAt = null
        };

        NotificationStore.Notifications.Add(notification);
        return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<NotificationDto>.Ok(NotificationDto.FromEntity(notification), "Notification queued"));
    }

    [HttpPost("{id:int}/send")]
    public ActionResult<ApiResponse<NotificationDto>> Send(int id)
    {
        var notification = NotificationStore.Notifications.FirstOrDefault(x => x.Id == id);
        if (notification is null)
        {
            return NotFound(ApiResponse<NotificationDto>.Fail("Notification not found"));
        }

        notification.Status = "Sent";
        notification.SentAt = DateTime.UtcNow;
        return Ok(ApiResponse<NotificationDto>.Ok(NotificationDto.FromEntity(notification), "Notification sent"));
    }

    [HttpPost("{id:int}/fail")]
    public ActionResult<ApiResponse<NotificationDto>> MarkFailed(int id, [FromBody] FailureReasonDto dto)
    {
        var notification = NotificationStore.Notifications.FirstOrDefault(x => x.Id == id);
        if (notification is null)
        {
            return NotFound(ApiResponse<NotificationDto>.Fail("Notification not found"));
        }

        notification.Status = "Failed";
        notification.FailureReason = dto.Reason;
        return Ok(ApiResponse<NotificationDto>.Ok(NotificationDto.FromEntity(notification), "Notification marked as failed"));
    }
}


