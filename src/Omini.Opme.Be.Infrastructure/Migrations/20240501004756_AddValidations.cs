using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omini.Opme.Be.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddValidations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Patients",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "InsuranceCompanies",
                type: "character varying(18)",
                maxLength: 18,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "Hospitals",
                type: "character varying(18)",
                maxLength: 18,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Patients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "InsuranceCompanies",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(18)",
                oldMaxLength: 18);

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "Hospitals",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(18)",
                oldMaxLength: 18);
        }
    }
}
