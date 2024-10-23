using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoinDesk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "幣別Id"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "幣別名稱"),
                    CurrencyCode = table.Column<int>(type: "int", nullable: false, comment: "幣別代碼"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "建立時間"),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "修改時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
