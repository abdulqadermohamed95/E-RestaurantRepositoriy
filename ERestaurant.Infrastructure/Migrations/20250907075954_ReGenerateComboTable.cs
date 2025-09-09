using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERestaurant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReGenerateComboTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComboMaterials_Materials_MaterialId",
                table: "ComboMaterials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComboMaterials",
                table: "ComboMaterials");

            migrationBuilder.AlterColumn<bool>(
                name: "IsOptional",
                table: "ComboMaterials",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ComboMaterials",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ComboMaterials",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComboMaterials",
                table: "ComboMaterials",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ComboMaterials_ComboId",
                table: "ComboMaterials",
                column: "ComboId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComboMaterials_Materials_MaterialId",
                table: "ComboMaterials",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComboMaterials_Materials_MaterialId",
                table: "ComboMaterials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComboMaterials",
                table: "ComboMaterials");

            migrationBuilder.DropIndex(
                name: "IX_ComboMaterials_ComboId",
                table: "ComboMaterials");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ComboMaterials");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ComboMaterials");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
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

            migrationBuilder.AlterColumn<bool>(
                name: "IsOptional",
                table: "ComboMaterials",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ComboMaterials",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComboMaterials",
                table: "ComboMaterials",
                columns: new[] { "ComboId", "MaterialId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ComboMaterials_Materials_MaterialId",
                table: "ComboMaterials",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
