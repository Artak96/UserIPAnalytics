using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserIPAnalytics.Infrustructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "idx_ip_address",
                table: "UserConnections",
                column: "IpAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_ip_address",
                table: "UserConnections");
        }
    }
}
