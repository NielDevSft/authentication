using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWTAuthentication.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Removed = table.Column<bool>(type: "boolean", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwtClaim", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "app",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Removed = table.Column<bool>(type: "boolean", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "app",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    JwtClaimUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Removed = table.Column<bool>(type: "boolean", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Usuario_JwtClaim_JwtClaimUuid",
                        column: x => x.JwtClaimUuid,
                        principalSchema: "app",
                        principalTable: "JwtClaim",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "RoleJwtClaim",
                schema: "app",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    JwtClaimUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    RoleUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Removed = table.Column<bool>(type: "boolean", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleJwtClaim", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_RoleJwtClaim_JwtClaim_JwtClaimUuid",
                        column: x => x.JwtClaimUuid,
                        principalSchema: "app",
                        principalTable: "JwtClaim",
                        principalColumn: "Uuid");
                    table.ForeignKey(
                        name: "FK_RoleJwtClaim_Role_RoleUuid",
                        column: x => x.RoleUuid,
                        principalSchema: "app",
                        principalTable: "Role",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleJwtClaim_JwtClaimUuid",
                schema: "app",
                table: "RoleJwtClaim",
                column: "JwtClaimUuid");

            migrationBuilder.CreateIndex(
                name: "IX_RoleJwtClaim_RoleUuid",
                schema: "app",
                table: "RoleJwtClaim",
                column: "RoleUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                schema: "app",
                table: "Usuario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_JwtClaimUuid",
                schema: "app",
                table: "Usuario",
                column: "JwtClaimUuid");

            // Seed: usuário admin inicial — senha Admin@123 hasheada em SHA1
            var passwordHash = Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes("Admin@123"))).ToLowerInvariant();
            migrationBuilder.InsertData(
                schema: "app",
                table: "Usuario",
                columns: new[] { "Email", "PasswordHash", "Username", "Active", "Removed", "CreateAt", "UpdateAt" },
                values: new object[] { "admin@admin.com", passwordHash, "admin", true, false, DateTime.UtcNow, DateTime.UtcNow });
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
