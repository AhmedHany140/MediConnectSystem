using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyHospialoo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNurseConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Medicine",
                table: "Medicine");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medicine",
                table: "Medicine",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Medicine_NurseId",
                table: "Medicine",
                column: "NurseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Medicine",
                table: "Medicine");

            migrationBuilder.DropIndex(
                name: "IX_Medicine_NurseId",
                table: "Medicine");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medicine",
                table: "Medicine",
                columns: new[] { "NurseId", "Id" });
        }
    }
}
