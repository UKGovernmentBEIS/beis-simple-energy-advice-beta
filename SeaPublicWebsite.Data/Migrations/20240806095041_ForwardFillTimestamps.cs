using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    public partial class ForwardFillTimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // This operation will perform a forward-fill on "RecommendationsFirstRetrievedAt"
            // starting from August 1st onward, filling in null values with the most recent non-null value
            migrationBuilder.Operations.Add(new SqlOperation
            {
                Sql = @"
                    WITH grouped_table AS (
                        SELECT ""PropertyDataId"", ""RecommendationsFirstRetrievedAt"", COUNT(""RecommendationsFirstRetrievedAt"") 
                        OVER (ORDER BY ""PropertyDataId"") AS _grp
                        FROM ""PropertyData""
                    ), 
                    final_table AS (
                        SELECT ""PropertyDataId"", _grp, FIRST_VALUE(""RecommendationsFirstRetrievedAt"") 
                            OVER (PARTITION BY _grp ORDER BY ""PropertyDataId"") AS smeared_timestamps FROM grouped_table
                    )
                    UPDATE ""PropertyData"" p 
                        SET ""RecommendationsFirstRetrievedAt"" = f.smeared_timestamps FROM final_table f 
                        WHERE p.""PropertyDataId"" = f.""PropertyDataId"" 
                            AND f.""smeared_timestamps"" >= TO_TIMESTAMP('2024/08/01 00:00:00', 'YYYY/MM/DD HH24:MI:SS') AT TIME ZONE 'UTC';"
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
