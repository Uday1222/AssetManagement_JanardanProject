using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagement.Migrations
{
    /// <inheritdoc />
    public partial class AssetChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SerialNo",
                table: "Assets",
                newName: "VendorEmail");

            migrationBuilder.RenameColumn(
                name: "PurchasedBy",
                table: "Assets",
                newName: "VendorContact");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "Assets",
                newName: "Vendor");

            migrationBuilder.RenameColumn(
                name: "ItemType",
                table: "Assets",
                newName: "StockLocation");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CostPerItem",
                table: "Assets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "DaysPerReorder",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ItemDiscontinued",
                table: "Assets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ItemReorderQuantity",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastOrderDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ReorderLevel",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "TotalValue",
                table: "Assets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPerItem",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "DaysPerReorder",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ItemDiscontinued",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ItemReorderQuantity",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "LastOrderDate",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ReorderLevel",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "TotalValue",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "VendorEmail",
                table: "Assets",
                newName: "SerialNo");

            migrationBuilder.RenameColumn(
                name: "VendorContact",
                table: "Assets",
                newName: "PurchasedBy");

            migrationBuilder.RenameColumn(
                name: "Vendor",
                table: "Assets",
                newName: "Model");

            migrationBuilder.RenameColumn(
                name: "StockLocation",
                table: "Assets",
                newName: "ItemType");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
