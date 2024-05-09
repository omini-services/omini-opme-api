using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omini.Opme.Be.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addIdentityMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "IdentityOpmeUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "IdentityOpmeUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "IdentityOpmeUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "IdentityOpmeUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "IdentityOpmeUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "IdentityOpmeUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedBy",
                table: "IdentityOpmeUsers",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "IdentityOpmeUsers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "IdentityOpmeUsers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "IdentityOpmeUsers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "IdentityOpmeUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "IdentityOpmeUsers");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "IdentityOpmeUsers");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "IdentityOpmeUsers");
        }
    }
}
