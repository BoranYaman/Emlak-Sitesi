﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Migrations
{
    /// <inheritdoc />
    public partial class AdGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdNumber",
                table: "Plot",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AdNumber",
                table: "House",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdNumber",
                table: "Plot");

            migrationBuilder.DropColumn(
                name: "AdNumber",
                table: "House");
        }
    }
}
