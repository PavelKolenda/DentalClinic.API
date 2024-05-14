using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DentalClinic.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWorkingDaysTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingSchedules_WorkingDays_WorkingDayId",
                table: "WorkingSchedules");

            migrationBuilder.DropTable(
                name: "WorkingDays");

            migrationBuilder.DropIndex(
                name: "IX_WorkingSchedules_WorkingDayId",
                table: "WorkingSchedules");

            migrationBuilder.DropColumn(
                name: "WorkingDayId",
                table: "WorkingSchedules");

            migrationBuilder.AddColumn<string>(
                name: "WorkingDay",
                table: "WorkingSchedules",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingDay",
                table: "WorkingSchedules");

            migrationBuilder.AddColumn<int>(
                name: "WorkingDayId",
                table: "WorkingSchedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkingDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Day = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDays", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingSchedules_WorkingDayId",
                table: "WorkingSchedules",
                column: "WorkingDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingSchedules_WorkingDays_WorkingDayId",
                table: "WorkingSchedules",
                column: "WorkingDayId",
                principalTable: "WorkingDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
