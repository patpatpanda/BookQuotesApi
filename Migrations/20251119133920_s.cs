using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookQuotesApi.Migrations
{
    /// <inheritdoc />
    public partial class s : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TemplateQuotes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Author", "Text" },
                values: new object[] { "Per Morberg", "Livet är en jobbig period." });

            migrationBuilder.UpdateData(
                table: "TemplateQuotes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Author", "Text" },
                values: new object[] { "Emil Arrenius", "Det gäller att ha tungan rätt i mun." });

            migrationBuilder.UpdateData(
                table: "TemplateQuotes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Author", "Text" },
                values: new object[] { "Krister Pettersson", "Som man bäddar får man ligga." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TemplateQuotes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Author", "Text" },
                values: new object[] { "Karin Boye", "Det är vägen, inte målet, som är mödan värd." });

            migrationBuilder.UpdateData(
                table: "TemplateQuotes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Author", "Text" },
                values: new object[] { "H.C. Andersen", "Att resa är att leva." });

            migrationBuilder.UpdateData(
                table: "TemplateQuotes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Author", "Text" },
                values: new object[] { "Greta Thunberg", "Vi måste våga ta ansvar för vår framtid." });
        }
    }
}
