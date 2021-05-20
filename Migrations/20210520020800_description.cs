using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodYeah.Migrations
{
    public partial class description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_QuoteDetails_QuoteDetailsId",
                table: "Quotes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "17d55e8a-3904-45f2-b5aa-6bcb42a5dd17");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7891fa9-9797-4b2c-8994-713034ad7a7a");

            migrationBuilder.AlterColumn<int>(
                name: "QuoteDetailsId",
                table: "Quotes",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                table: "Products",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5d0419ce-08b8-4d2b-beee-8fb71deef33a", "35148f69-2956-44d2-9888-3743fe97f51d", "ADMIN", "ADMIN" },
                    { "c12995ea-92d2-4c1c-afd0-cae0e974b20d", "d8b3586b-bee4-4cda-ba22-1ceaeb0179f5", "USER", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_QuoteDetails_QuoteDetailsId",
                table: "Quotes",
                column: "QuoteDetailsId",
                principalTable: "QuoteDetails",
                principalColumn: "QuoteDetailsId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_QuoteDetails_QuoteDetailsId",
                table: "Quotes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5d0419ce-08b8-4d2b-beee-8fb71deef33a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c12995ea-92d2-4c1c-afd0-cae0e974b20d");

            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "QuoteDetailsId",
                table: "Quotes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "17d55e8a-3904-45f2-b5aa-6bcb42a5dd17", "4fe61d2a-fc6c-4465-b561-3712a2c2ba24", "ADMIN", "ADMIN" },
                    { "c7891fa9-9797-4b2c-8994-713034ad7a7a", "c79d4b04-61a0-4aea-98a8-93673ce30d16", "USER", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_QuoteDetails_QuoteDetailsId",
                table: "Quotes",
                column: "QuoteDetailsId",
                principalTable: "QuoteDetails",
                principalColumn: "QuoteDetailsId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
