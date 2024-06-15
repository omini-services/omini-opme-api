using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Omini.Opme.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "hospitalcode_sequence");

            migrationBuilder.CreateSequence(
                name: "insurancecompanycode_sequence");

            migrationBuilder.CreateSequence(
                name: "internalspecialistcode_sequence");

            migrationBuilder.CreateSequence(
                name: "itemcode_sequence");

            migrationBuilder.CreateSequence(
                name: "patientcode_sequence");

            migrationBuilder.CreateSequence(
                name: "physiciancode_sequence");

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "nextval ('hospitalcode_sequence')"),
                    LegalName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TradeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceCompanies",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "nextval ('insurancecompanycode_sequence')"),
                    LegalName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TradeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceCompanies", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "InternalSpecialists",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "nextval ('internalspecialistcode_sequence')"),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Telefone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalSpecialists", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "nextval ('itemcode_sequence')"),
                    SalesName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Uom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AnvisaCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AnvisaDueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SupplierCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Cst = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SusCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NcmCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "OpmeUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpmeUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "nextval ('patientcode_sequence')"),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Cpf = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Physicians",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "nextval ('physiciancode_sequence')"),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Cro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Crm = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Physicians", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientCode = table.Column<string>(type: "character varying(50)", nullable: false),
                    PatientFirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PatientLastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PatientMiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PhysicianCode = table.Column<string>(type: "character varying(50)", nullable: false),
                    PhysicianFirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PhysicianLastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PhysicianMiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HospitalCode = table.Column<string>(type: "character varying(50)", nullable: false),
                    HospitalName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    InsuranceCompanyCode = table.Column<string>(type: "character varying(50)", nullable: false),
                    InsuranceCompanyName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    InternalSpecialistCode = table.Column<string>(type: "text", nullable: false),
                    PayingSourceType = table.Column<string>(type: "text", nullable: false),
                    PayingSourceCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PayingSourceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotations_Hospitals_HospitalCode",
                        column: x => x.HospitalCode,
                        principalTable: "Hospitals",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quotations_InsuranceCompanies_InsuranceCompanyCode",
                        column: x => x.InsuranceCompanyCode,
                        principalTable: "InsuranceCompanies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quotations_Patients_PatientCode",
                        column: x => x.PatientCode,
                        principalTable: "Patients",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quotations_Physicians_PhysicianCode",
                        column: x => x.PhysicianCode,
                        principalTable: "Physicians",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuotationItems",
                columns: table => new
                {
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    LineId = table.Column<int>(type: "integer", nullable: false),
                    ItemCode = table.Column<string>(type: "character varying(50)", nullable: false),
                    ItemName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ReferenceCode = table.Column<string>(type: "text", nullable: false),
                    AnvisaCode = table.Column<string>(type: "text", nullable: false),
                    AnvisaDueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    LineTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    LineOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationItems", x => new { x.DocumentId, x.LineId });
                    table.ForeignKey(
                        name: "FK_QuotationItems_Items_ItemCode",
                        column: x => x.ItemCode,
                        principalTable: "Items",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuotationItems_Quotations_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "InternalSpecialists",
                columns: new[] { "Code", "CreatedBy", "CreatedOn", "Email", "Telefone", "UpdatedBy", "UpdatedOn", "FirstName", "LastName", "MiddleName" },
                values: new object[] { "1", new Guid("93191413-db51-4cc8-bc58-cc80e180a551"), new DateTime(2024, 6, 15, 1, 18, 24, 277, DateTimeKind.Utc).AddTicks(4820), "comercial@fratermedical.com.br", "(11) 3829-9400", null, null, "Nathália", "Camelo", null });

            migrationBuilder.CreateIndex(
                name: "IX_QuotationItems_ItemCode",
                table: "QuotationItems",
                column: "ItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_HospitalCode",
                table: "Quotations",
                column: "HospitalCode");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_InsuranceCompanyCode",
                table: "Quotations",
                column: "InsuranceCompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_PatientCode",
                table: "Quotations",
                column: "PatientCode");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_PhysicianCode",
                table: "Quotations",
                column: "PhysicianCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternalSpecialists");

            migrationBuilder.DropTable(
                name: "OpmeUsers");

            migrationBuilder.DropTable(
                name: "QuotationItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Quotations");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "InsuranceCompanies");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Physicians");

            migrationBuilder.DropSequence(
                name: "hospitalcode_sequence");

            migrationBuilder.DropSequence(
                name: "insurancecompanycode_sequence");

            migrationBuilder.DropSequence(
                name: "internalspecialistcode_sequence");

            migrationBuilder.DropSequence(
                name: "itemcode_sequence");

            migrationBuilder.DropSequence(
                name: "patientcode_sequence");

            migrationBuilder.DropSequence(
                name: "physiciancode_sequence");
        }
    }
}
