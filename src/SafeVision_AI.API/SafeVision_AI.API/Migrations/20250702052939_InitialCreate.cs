using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SafeVision_AI.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cameras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StreamUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastSeen = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FrameRate = table.Column<int>(type: "int", nullable: false),
                    AnalysisInterval = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalIncidents = table.Column<int>(type: "int", nullable: false),
                    CriticalIncidents = table.Column<int>(type: "int", nullable: false),
                    HighIncidents = table.Column<int>(type: "int", nullable: false),
                    MediumIncidents = table.Column<int>(type: "int", nullable: false),
                    LowIncidents = table.Column<int>(type: "int", nullable: false),
                    ResolvedIncidents = table.Column<int>(type: "int", nullable: false),
                    GeneratedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ReportData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportFileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CameraId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ConfidenceScore = table.Column<double>(type: "float(5)", precision: 5, scale: 4, nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    DetectedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    VideoClipUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudioClipUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoundingBoxX = table.Column<int>(type: "int", nullable: true),
                    BoundingBoxY = table.Column<int>(type: "int", nullable: true),
                    BoundingBoxWidth = table.Column<int>(type: "int", nullable: true),
                    BoundingBoxHeight = table.Column<int>(type: "int", nullable: true),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    ResolvedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ModelVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incidents_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingQueue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CameraId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PayloadData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    MaxRetries = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingQueue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessingQueue_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncidentAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidentId = table.Column<int>(type: "int", nullable: false),
                    RecipientEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RecipientPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcknowledgedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentAlerts_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cameras",
                columns: new[] { "Id", "AnalysisInterval", "CreatedAt", "FrameRate", "IsActive", "LastSeen", "Location", "Name", "StreamUrl" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2024, 1, 1, 8, 0, 0, 0, DateTimeKind.Utc), 30, true, new DateTime(2024, 1, 1, 8, 0, 0, 0, DateTimeKind.Utc), "Building A - Main Entrance", "Main Entrance", "rtsp://demo:demo@ipvmdemo.dyndns.org:5541/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=unicast" },
                    { 2, 5, new DateTime(2024, 1, 1, 8, 5, 0, 0, DateTimeKind.Utc), 30, true, new DateTime(2024, 1, 1, 8, 5, 0, 0, DateTimeKind.Utc), "Building A - Parking Area", "Parking Lot", "rtsp://demo:demo@ipvmdemo.dyndns.org:5542/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=unicast" },
                    { 3, 4, new DateTime(2024, 1, 1, 8, 10, 0, 0, DateTimeKind.Utc), 25, false, new DateTime(2024, 1, 1, 6, 10, 0, 0, DateTimeKind.Utc), "Building B - Cafeteria", "Cafeteria", "rtsp://demo:demo@ipvmdemo.dyndns.org:5543/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=unicast" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_Name",
                table: "Cameras",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyReports_ReportDate",
                table: "DailyReports",
                column: "ReportDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncidentAlerts_CreatedAt",
                table: "IncidentAlerts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentAlerts_IncidentId",
                table: "IncidentAlerts",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentAlerts_Status",
                table: "IncidentAlerts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_CameraId_DetectedAt",
                table: "Incidents",
                columns: new[] { "CameraId", "DetectedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_DetectedAt",
                table: "Incidents",
                column: "DetectedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_IsResolved",
                table: "Incidents",
                column: "IsResolved");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_Severity",
                table: "Incidents",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_Type",
                table: "Incidents",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingQueue_CameraId",
                table: "ProcessingQueue",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingQueue_CreatedAt",
                table: "ProcessingQueue",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingQueue_Status",
                table: "ProcessingQueue",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingQueue_Status_CreatedAt",
                table: "ProcessingQueue",
                columns: new[] { "Status", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyReports");

            migrationBuilder.DropTable(
                name: "IncidentAlerts");

            migrationBuilder.DropTable(
                name: "ProcessingQueue");

            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropTable(
                name: "Cameras");
        }
    }
}
