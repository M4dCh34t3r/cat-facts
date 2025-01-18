using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facts",
                columns: table => new
                {
                    Id = table.Column<Guid>(
                        type: "uniqueidentifier",
                        nullable: false,
                        defaultValueSql: "NEWID()"
                    ),
                    Text = table.Column<string>(
                        type: "nvarchar(900)",
                        maxLength: 900,
                        nullable: false
                    ),
                    InsertedAt = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false,
                        defaultValueSql: "GETUTCDATE()"
                    ),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccurrenceCount = table.Column<int>(
                        type: "int",
                        nullable: false,
                        defaultValue: 1
                    ),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    DislikeCount = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facts", x => x.Id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Facts_Text",
                table: "Facts",
                column: "Text",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Facts");
        }
    }
}
