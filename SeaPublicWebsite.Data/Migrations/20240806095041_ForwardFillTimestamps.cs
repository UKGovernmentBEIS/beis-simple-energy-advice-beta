using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    public partial class ForwardFillTimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Operations.Add(new SqlOperation
            {
                Sql = @"
                    with grouped_table as (
                        SELECT ""PropertyDataId"", ""RecommendationsFirstRetrievedAt"", count(""RecommendationsFirstRetrievedAt"") 
                        OVER (ORDER BY ""PropertyDataId"") AS _grp
                        FROM ""PropertyData""
                    ), 
                    final_table AS(
                        SELECT ""PropertyDataId"", _grp, FIRST_VALUE(""RecommendationsFirstRetrievedAt"") 
                            OVER (partition by _grp order by ""PropertyDataId"") AS smeared_timestamps FROM grouped_table
                    )
                    UPDATE ""PropertyData"" p 
                        SET ""RecommendationsFirstRetrievedAt"" = f.smeared_timestamps from final_table f 
                        WHERE p.""PropertyDataId"" = f.""PropertyDataId"" 
                            AND f.""smeared_timestamps"" >= to_timestamp('2024/08/01 00:00:00', 'YYYY/MM/DD HH24:MI:SS') AT TIME ZONE 'UTC';"
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
