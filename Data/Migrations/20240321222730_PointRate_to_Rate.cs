using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class PointRate_to_Rate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Currencies_CurrencyId",
                table: "Points");

            migrationBuilder.DropTable(
                name: "PointRates");

            migrationBuilder.DropIndex(
                name: "IX_Points_CurrencyId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Points");

            migrationBuilder.RenameColumn(
                name: "PointRateId",
                table: "Points",
                newName: "RateId");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Stores",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    SpentAmount = table.Column<decimal>(type: "Decimal(18,2)", nullable: false),
                    Points = table.Column<double>(type: "float", nullable: false),
                    InsertDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rates_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_OwnerId",
                table: "Stores",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_RateId",
                table: "Points",
                column: "RateId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_CurrencyId",
                table: "Rates",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Rates_RateId",
                table: "Points",
                column: "RateId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Users_OwnerId",
                table: "Stores",
                column: "OwnerId",
                principalSchema: "Security",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Rates_RateId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Users_OwnerId",
                table: "Stores");

            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Stores_OwnerId",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Points_RateId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Stores");

            migrationBuilder.RenameColumn(
                name: "RateId",
                table: "Points",
                newName: "PointRateId");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Points",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PointRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    InsertDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Points = table.Column<double>(type: "float", nullable: false),
                    SpentAmount = table.Column<decimal>(type: "Decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointRates_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Points_CurrencyId",
                table: "Points",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PointRates_CurrencyId",
                table: "PointRates",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Currencies_CurrencyId",
                table: "Points",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
