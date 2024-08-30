using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

#nullable disable

namespace SeaPublicWebsite.Data.Migrations
{
    public partial class ForwardFillTimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /// Part of PC-1234, to recover the "missing" data by approximating timestamps for incomplete journeys:
            /// This will perform a forward-fill on "RecommendationsFirstRetrievedAt"
            /// starting from August 1st onward, filling in null values with the most recent non-null value.
            ///
            /// After release, we realised there was a mistake in this migration: it did not account for old users
            /// revisiting the site and retrieving recommendations for PropertyData generated before August. This was
            /// fixed manually on all environments using the following script:
            ///
            /// with older_rows as (
            ///     select *, lag("RecommendationsFirstRetrievedAt") over (order by "PropertyDataId") as prev
            ///     from "PropertyData"
            ///     where "PropertyDataId" < N
            /// ),
            /// rows_to_clear as (
            ///     select * from older_rows
            ///     where "RecommendationsFirstRetrievedAt" = prev
            /// )
            /// update "PropertyData"
            /// set "RecommendationsFirstRetrievedAt" = null
            /// where "PropertyDataId" in (select "PropertyDataId" from rows_to_clear)
            /// and extract(month from "RecommendationsFirstRetrievedAt") = 8;
            ///
            /// where N was the manually-observed start point of August (for prod this was 481800). We could not do
            /// something like:
            /// select max("PropertyDataId") from "PropertyData" where extract(month from "RecommendationsFirstRetrievedAt") = 7
            /// because there was one pathological row with a July timestamp (and no reference code) that appeared to
            /// have been insterted in August - this was raised as a separate issue (PC-1287).
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
