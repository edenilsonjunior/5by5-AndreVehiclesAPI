using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndreVehicles.CarAPI.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "_id",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FinancialPending",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerDocument = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinancialPendingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialPending", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialPending_Customer_CustomerDocument",
                        column: x => x.CustomerDocument,
                        principalTable: "Customer",
                        principalColumn: "Document",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialPending_CustomerDocument",
                table: "FinancialPending",
                column: "CustomerDocument");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialPending");

            migrationBuilder.DropColumn(
                name: "_id",
                table: "Address");
        }
    }
}
