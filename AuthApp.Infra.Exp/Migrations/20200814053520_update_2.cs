using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthApp.Infra.Exp.Migrations
{
    public partial class update_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "ApplicationId", "EnvironmentType", "Name", "OrgId", "PublicId" },
                values: new object[,]
                {
                    { 1, "AuthApp", "Dev", "Experiment1", "EXP1", new Guid("bbdee09c-089b-4d30-bece-44df5923716c") },
                    { 2, "AuthApp", "Dev", "Experiment2", "EXP2", new Guid("6fb600c1-9011-4fd7-9234-881379716440") }
                });

            migrationBuilder.InsertData(
                table: "Language",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Originated from China", "Chinese" },
                    { 2, "Popular in multiples countries", "English" },
                    { 3, "Originated from French", "French" }
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Action" },
                values: new object[,]
                {
                    { 1, "Create" },
                    { 2, "Read" },
                    { 3, "Update" },
                    { 4, "Delete" }
                });

            migrationBuilder.InsertData(
                table: "AuthConnection",
                columns: new[] { "Id", "ApplicationId", "ConnectionName", "CustomerId", "EnvironmentType" },
                values: new object[,]
                {
                    { 1, "AuthApp", "Username-Password-Authentication", 1, "Dev" },
                    { 2, "AuthApp", "google-oauth2", 1, "Dev" },
                    { 3, "AuthApp", "Username-Password-Authentication", 2, "Dev" },
                    { 4, "AuthApp", "google-oauth2", 2, "Dev" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CustomerId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Admin" },
                    { 2, 1, "Standard" },
                    { 3, 2, "Admin" },
                    { 4, 2, "Standard" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AuthConnection",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AuthConnection",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AuthConnection",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AuthConnection",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
