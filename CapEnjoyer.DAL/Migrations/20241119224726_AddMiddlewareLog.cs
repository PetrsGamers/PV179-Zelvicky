#nullable disable

namespace CapEnjoyer.DAL.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class AddMiddlewareLog : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.CreateTable(
            name: "MiddlewareLogs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Action = table.Column<int>(type: "integer", nullable: false),
                Log = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_MiddlewareLogs", x => x.Id));

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "MiddlewareLogs");
}
