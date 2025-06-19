using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyHospialoo.Migrations
{
    /// <inheritdoc />
    public partial class addSenderNameToMassage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "Massages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "Massages");
        }
    }
}
