using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonReview.Migrations
{
    public partial class EditingTableCountriesName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Contries_CountryId",
                table: "Owners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contries",
                table: "Contries");

            migrationBuilder.RenameTable(
                name: "Contries",
                newName: "Countries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Countries_CountryId",
                table: "Owners",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Countries_CountryId",
                table: "Owners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "Contries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contries",
                table: "Contries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Contries_CountryId",
                table: "Owners",
                column: "CountryId",
                principalTable: "Contries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
