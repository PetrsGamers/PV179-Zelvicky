#nullable disable

namespace CapEnjoyer.DAL.Migrations;
using System;
using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class RemovedUserPassword : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
            name: "Password",
            table: "Users");

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Password",
            table: "Users",
            type: "character varying(255)",
            maxLength: 255,
            nullable: false,
            defaultValue: "");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("3cd58330-80d5-834f-6d9a-587a4d431aaa"),
            column: "Password",
            value: "password");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("55662f79-aedd-690a-18b1-8e19c3296f18"),
            column: "Password",
            value: "password");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("63ce833b-99a9-ca6f-0d1f-f18cbf724a42"),
            column: "Password",
            value: "password");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("805729bf-e89d-c71c-4de2-f348dae7b93b"),
            column: "Password",
            value: "password");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("8832134c-3771-f7e9-239b-53daeed3b23a"),
            column: "Password",
            value: "password");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("a29c7e55-9fc2-735c-3ca9-dd3f5b0c6f2a"),
            column: "Password",
            value: "password");
    }
}
