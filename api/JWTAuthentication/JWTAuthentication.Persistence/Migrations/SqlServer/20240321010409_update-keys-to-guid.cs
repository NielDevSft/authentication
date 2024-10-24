using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWTAuthentication.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatekeystoguid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleJwtClaim_JwtClaim_JwtClaimId",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleJwtClaim_Role_RoleId",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_JwtClaim_JwtClaimId",
                schema: "app",
                table: "Usuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                schema: "app",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_JwtClaimId",
                schema: "app",
                table: "Usuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleJwtClaim",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropIndex(
                name: "IX_RoleJwtClaim_JwtClaimId",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropIndex(
                name: "IX_RoleJwtClaim_RoleId",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                schema: "app",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JwtClaim",
                schema: "app",
                table: "JwtClaim");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "app",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "JwtClaimId",
                schema: "app",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropColumn(
                name: "JwtClaimId",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropColumn(
                name: "RoleId",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "app",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "app",
                table: "JwtClaim");

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                schema: "app",
                table: "Usuario",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "JwtClaimUuid",
                schema: "app",
                table: "Usuario",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                schema: "app",
                table: "RoleJwtClaim",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "JwtClaimUuid",
                schema: "app",
                table: "RoleJwtClaim",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleUuid",
                schema: "app",
                table: "RoleJwtClaim",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                schema: "app",
                table: "Role",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                schema: "app",
                table: "JwtClaim",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                schema: "app",
                table: "Usuario",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleJwtClaim",
                schema: "app",
                table: "RoleJwtClaim",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                schema: "app",
                table: "Role",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JwtClaim",
                schema: "app",
                table: "JwtClaim",
                column: "Uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_JwtClaimUuid",
                schema: "app",
                table: "Usuario",
                column: "JwtClaimUuid");

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

            migrationBuilder.AddForeignKey(
                name: "FK_RoleJwtClaim_JwtClaim_JwtClaimUuid",
                schema: "app",
                table: "RoleJwtClaim",
                column: "JwtClaimUuid",
                principalSchema: "app",
                principalTable: "JwtClaim",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleJwtClaim_Role_RoleUuid",
                schema: "app",
                table: "RoleJwtClaim",
                column: "RoleUuid",
                principalSchema: "app",
                principalTable: "Role",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_JwtClaim_JwtClaimUuid",
                schema: "app",
                table: "Usuario",
                column: "JwtClaimUuid",
                principalSchema: "app",
                principalTable: "JwtClaim",
                principalColumn: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleJwtClaim_JwtClaim_JwtClaimUuid",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleJwtClaim_Role_RoleUuid",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_JwtClaim_JwtClaimUuid",
                schema: "app",
                table: "Usuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                schema: "app",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_JwtClaimUuid",
                schema: "app",
                table: "Usuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleJwtClaim",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropIndex(
                name: "IX_RoleJwtClaim_JwtClaimUuid",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropIndex(
                name: "IX_RoleJwtClaim_RoleUuid",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                schema: "app",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JwtClaim",
                schema: "app",
                table: "JwtClaim");

            migrationBuilder.DropColumn(
                name: "Uuid",
                schema: "app",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "JwtClaimUuid",
                schema: "app",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Uuid",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropColumn(
                name: "JwtClaimUuid",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropColumn(
                name: "RoleUuid",
                schema: "app",
                table: "RoleJwtClaim");

            migrationBuilder.DropColumn(
                name: "Uuid",
                schema: "app",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Uuid",
                schema: "app",
                table: "JwtClaim");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "app",
                table: "Usuario",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "JwtClaimId",
                schema: "app",
                table: "Usuario",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "app",
                table: "RoleJwtClaim",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "JwtClaimId",
                schema: "app",
                table: "RoleJwtClaim",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                schema: "app",
                table: "RoleJwtClaim",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "app",
                table: "Role",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "app",
                table: "JwtClaim",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                schema: "app",
                table: "Usuario",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleJwtClaim",
                schema: "app",
                table: "RoleJwtClaim",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                schema: "app",
                table: "Role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JwtClaim",
                schema: "app",
                table: "JwtClaim",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_JwtClaimId",
                schema: "app",
                table: "Usuario",
                column: "JwtClaimId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_RoleJwtClaim_JwtClaim_JwtClaimId",
                schema: "app",
                table: "RoleJwtClaim",
                column: "JwtClaimId",
                principalSchema: "app",
                principalTable: "JwtClaim",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleJwtClaim_Role_RoleId",
                schema: "app",
                table: "RoleJwtClaim",
                column: "RoleId",
                principalSchema: "app",
                principalTable: "Role",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_JwtClaim_JwtClaimId",
                schema: "app",
                table: "Usuario",
                column: "JwtClaimId",
                principalSchema: "app",
                principalTable: "JwtClaim",
                principalColumn: "Id");
        }
    }
}
