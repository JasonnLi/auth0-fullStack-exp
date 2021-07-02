using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthApp.Infra.Exp.Migrations
{
    public partial class update_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: 1,
                column: "PublicId",
                value: new Guid("bbdee09c-089b-4d30-bece-44df5923716c"));

            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: 2,
                column: "PublicId",
                value: new Guid("6fb600c1-9011-4fd7-9234-881379716440"));

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Auth0Id", "CustomerId", "Email", "FirstName", "LastName", "Source", "Type" },
                values: new object[,]
                {
                    { 1, "auth0|507f1f77bcf86cd799439020", 1, "authappadmin@admin.com", "Jason", "Lee", 2, 2 },
                    { 2, "auth0|5f36291f307dac006791986c", 2, "authappadmin1@admin1.com", "admin", "admin", 2, 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: 1,
                column: "PublicId",
                value: new Guid("bbdee09c-089b-4d30-bece-44df5923716c"));

            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: 2,
                column: "PublicId",
                value: new Guid("6fb600c1-9011-4fd7-9234-881379716440"));
        }
    }
}
