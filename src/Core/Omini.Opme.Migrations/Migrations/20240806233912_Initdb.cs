using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Omini.Opme.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Initdb : Migration
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
                name: "Hospitals",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "nextval ('hospitalcode_sequence')"),
                    LegalName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TradeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Hospitals_OpmeUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hospitals_OpmeUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InsuranceCompanies",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "nextval ('insurancecompanycode_sequence')"),
                    LegalName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TradeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceCompanies", x => x.Code);
                    table.ForeignKey(
                        name: "FK_InsuranceCompanies_OpmeUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceCompanies_OpmeUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id");
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
                    table.ForeignKey(
                        name: "FK_InternalSpecialists_OpmeUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InternalSpecialists_OpmeUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id");
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
                    table.ForeignKey(
                        name: "FK_Items_OpmeUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_OpmeUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id");
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
                    Comments = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Patients_OpmeUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patients_OpmeUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id");
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
                    Comments = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Physicians", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Physicians_OpmeUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Physicians_OpmeUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id");
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
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Total = table.Column<double>(type: "double precision", nullable: false),
                    Comments = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotations_InsuranceCompanies_InsuranceCompanyCode",
                        column: x => x.InsuranceCompanyCode,
                        principalTable: "InsuranceCompanies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotations_OpmeUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quotations_OpmeUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "OpmeUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Quotations_Patients_PatientCode",
                        column: x => x.PatientCode,
                        principalTable: "Patients",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotations_Physicians_PhysicianCode",
                        column: x => x.PhysicianCode,
                        principalTable: "Physicians",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
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
                    UnitPrice = table.Column<double>(type: "double precision", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    LineTotal = table.Column<double>(type: "double precision", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationItems_Quotations_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OpmeUsers",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Email", "IsDeleted", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { new Guid("77e48701-6371-4e3e-8d92-9db4a2bc1e5f"), new Guid("c8c5ce24-820f-41ba-8560-d7a282d80d29"), new DateTime(2024, 8, 6, 23, 39, 11, 881, DateTimeKind.Utc).AddTicks(2700), null, null, "guilherme_or@outlook.com", false, null, null },
                    { new Guid("c8c5ce24-820f-41ba-8560-d7a282d80d29"), new Guid("c8c5ce24-820f-41ba-8560-d7a282d80d29"), new DateTime(2024, 8, 6, 23, 39, 11, 881, DateTimeKind.Utc).AddTicks(2700), null, null, "test@invalid.com", false, null, null },
                    { new Guid("e6211f68-cfcd-40e9-a31a-bd0dcf4b4052"), new Guid("c8c5ce24-820f-41ba-8560-d7a282d80d29"), new DateTime(2024, 8, 6, 23, 39, 11, 881, DateTimeKind.Utc).AddTicks(2700), null, null, "dacceto@gmail.com", false, null, null }
                });

            migrationBuilder.InsertData(
                table: "InternalSpecialists",
                columns: new[] { "Code", "CreatedBy", "CreatedOn", "Email", "Telefone", "UpdatedBy", "UpdatedOn", "FirstName", "LastName", "MiddleName" },
                values: new object[] { "1", new Guid("c8c5ce24-820f-41ba-8560-d7a282d80d29"), new DateTime(2024, 8, 6, 23, 39, 11, 881, DateTimeKind.Utc).AddTicks(2800), "comercial@fratermedical.com.br", "(11) 3829-9400", null, null, "Nathália", "Camelo", null });

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_CreatedBy",
                table: "Hospitals",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_UpdatedBy",
                table: "Hospitals",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCompanies_CreatedBy",
                table: "InsuranceCompanies",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCompanies_UpdatedBy",
                table: "InsuranceCompanies",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InternalSpecialists_CreatedBy",
                table: "InternalSpecialists",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InternalSpecialists_UpdatedBy",
                table: "InternalSpecialists",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CreatedBy",
                table: "Items",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Items_UpdatedBy",
                table: "Items",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CreatedBy",
                table: "Patients",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UpdatedBy",
                table: "Patients",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Physicians_CreatedBy",
                table: "Physicians",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Physicians_UpdatedBy",
                table: "Physicians",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationItems_ItemCode",
                table: "QuotationItems",
                column: "ItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_CreatedBy",
                table: "Quotations",
                column: "CreatedBy");

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

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_UpdatedBy",
                table: "Quotations",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternalSpecialists");

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

            migrationBuilder.DropTable(
                name: "OpmeUsers");

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
