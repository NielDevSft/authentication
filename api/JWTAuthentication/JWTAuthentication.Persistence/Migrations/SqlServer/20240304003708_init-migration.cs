using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWTAuthentication.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "JwtClaim",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwtClaim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwtClaimId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_JwtClaim_JwtClaimId",
                        column: x => x.JwtClaimId,
                        principalSchema: "app",
                        principalTable: "JwtClaim",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleJwtClaim",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    JwtClaimId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleJwtClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleJwtClaim_JwtClaim_JwtClaimId",
                        column: x => x.JwtClaimId,
                        principalSchema: "app",
                        principalTable: "JwtClaim",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleJwtClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "app",
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleJwtClaim_JwtClaimId",
                schema: "app",
                table: "RoleJwtClaim",
                column: "JwtClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleJwtClaim_RoleId",
                schema: "app",
                table: "RoleJwtClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_JwtClaimId",
                schema: "app",
                table: "Usuario",
                column: "JwtClaimId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleJwtClaim",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Usuario",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "app");

            migrationBuilder.DropTable(
                name: "JwtClaim",
                schema: "app");
        }
    }
}
