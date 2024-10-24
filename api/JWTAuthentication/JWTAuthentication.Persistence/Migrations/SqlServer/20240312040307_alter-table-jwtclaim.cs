using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWTAuthentication.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class altertablejwtclaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimId",
                schema: "app",
                table: "RoleJwtClaim");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClaimId",
                schema: "app",
                table: "RoleJwtClaim",
                type: "int",
                nullable: true);
        }
    }
}
