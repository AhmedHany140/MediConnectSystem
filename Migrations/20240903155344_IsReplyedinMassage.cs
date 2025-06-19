using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyHospialoo.Migrations
{
    /// <inheritdoc />
    public partial class IsReplyedinMassage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReplyed",
                table: "Massages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReplyed",
                table: "Massages");
        }
    }
}
