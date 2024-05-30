using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchEngine.Migrations
{
    /// <inheritdoc />
    public partial class SolrParam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF NOT EXISTS(Select Id From [AppParameters] where Name = 'SolrUrl')
BEGIN
    Insert into [AppParameters]  (Name, Value) values ('SolrUrl', 'localhost:8983/solr')
END
GO");
        }
       
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF EXISTS(Select Id From [AppParameters] where Name = 'SolrUrl')
BEGIN
      DELETE FROM [AppParameters]  WHERE Name ='SolrUrl'
END
GO");
        }
    }
}
