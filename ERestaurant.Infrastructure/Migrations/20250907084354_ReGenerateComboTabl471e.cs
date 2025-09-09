using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERestaurant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReGenerateComboTabl471e : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComboMaterials_Combos_ComboId",
                table: "ComboMaterials");

            migrationBuilder.AddForeignKey(
                name: "FK_ComboMaterials_Combos_ComboId",
                table: "ComboMaterials",
                column: "ComboId",
                principalTable: "Combos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComboMaterials_Combos_ComboId",
                table: "ComboMaterials");

            migrationBuilder.AddForeignKey(
                name: "FK_ComboMaterials_Combos_ComboId",
                table: "ComboMaterials",
                column: "ComboId",
                principalTable: "Combos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
