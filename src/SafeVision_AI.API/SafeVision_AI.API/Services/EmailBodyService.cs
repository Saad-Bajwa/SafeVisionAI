using SafeVisionAI.Core.Entities;

namespace SafeVision_AI.API.Services
{
    public class EmailBodyService
    {
        public static string GenerateAlertEmailBody(Incident incident, IncidentAlert alert)
        {
            return $@"
    <html>
    <head>
        <style>
            body {{ font-family: Arial, sans-serif; color: #333; }}
            .header {{ background: #e53935; color: #fff; padding: 16px; font-size: 20px; }}
            .content {{ margin: 20px 0; }}
            .footer {{ margin-top: 30px; font-size: 12px; color: #888; }}
            .label {{ font-weight: bold; }}
        </style>
    </head>
    <body>
        <div class='header'>🚨 SafeVision AI Alert Notification</div>
        <div class='content'>
            <p><span class='label'>Incident Type:</span> {incident.Type}</p>
            <p><span class='label'>Severity:</span> {incident.Severity}</p>
            <p><span class='label'>Location:</span> {incident.Camera?.Location ?? "N/A"}</p>
            <p><span class='label'>Camera:</span> {incident.Camera?.Name ?? "N/A"}</p>
            <p><span class='label'>Detected At:</span> {incident.DetectedAt:yyyy-MM-dd HH:mm:ss} UTC</p>
            <p><span class='label'>Description:</span> {incident.Description}</p>
            {(string.IsNullOrEmpty(incident.ImageUrl) ? "" : $"<p><img src='{incident.ImageUrl}' alt='Incident Image' style='max-width:400px;'/></p>")}
        </div>
        <div class='footer'>
            This is an automated alert from SafeVision AI. Please do not reply to this email.
        </div>
    </body>
    </html>
    ";
        }
    }
}
