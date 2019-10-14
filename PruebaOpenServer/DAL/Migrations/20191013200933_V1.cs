using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FinalStateId",
                table: "RankResults",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InitialStateId",
                table: "RankResults",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BattleRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PokemonRankResultId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    ChallengerPokemonId = table.Column<int>(nullable: false),
                    ChallengedPokemonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleRecords_RankResults_PokemonRankResultId",
                        column: x => x.PokemonRankResultId,
                        principalTable: "RankResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BattleRecords_PokemonRankResultId",
                table: "BattleRecords",
                column: "PokemonRankResultId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattleRecords");

            migrationBuilder.DropColumn(
                name: "FinalStateId",
                table: "RankResults");

            migrationBuilder.DropColumn(
                name: "InitialStateId",
                table: "RankResults");
        }
    }
}
