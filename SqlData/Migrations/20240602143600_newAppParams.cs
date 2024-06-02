using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchEngine.Migrations
{
    /// <inheritdoc />
    public partial class newAppParams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF NOT EXISTS(Select Id From [AppParameters] where Name = 'SolrUrl')
BEGIN
    Insert into [AppParameters]  (Name, Value) values 
    ('SolrLogin', 'solr'),
    ('SolrPassword', 'AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAJi2obdt9MUqr+55LXlvpkwAAAAACAAAAAAAQZgAAAAEAACAAAACeNn1KzYRBJT3jtZENpBDZw7RZKx4sZeqMFXx+PygUzgAAAAAOgAAAAAIAACAAAAB9YOBtNqIfakRrdpNBM5D7ID5yydQKRcPSBr2q6F3y/xAAAADpw5BirwFyUJNyccAqUixWQAAAANTvNq+BjGRbzWWahLqBaAhtiwvbBSWc4cFvh/O9ex/w770RRLpAClKBZqBgLuKtBX+RdLAlMEXVcyLRVRWQLXE=')
END
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
