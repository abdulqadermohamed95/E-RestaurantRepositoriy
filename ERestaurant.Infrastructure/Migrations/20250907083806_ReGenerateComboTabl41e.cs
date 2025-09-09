using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERestaurant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReGenerateComboTabl41e : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ComboMaterials");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ComboMaterials");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ComboMaterials");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ComboMaterials");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ComboMaterials");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "ComboMaterials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ComboMaterials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ComboMaterials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ComboMaterials",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "ComboMaterials",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ComboMaterials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "ComboMaterials",
                type: "datetime2",
                nullable: true);
        }
    }
}
