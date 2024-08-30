using IMT_Planner_DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IMT_Planner_DAL.EF_Core;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IMTPlannerDbContext>
{
    public IMTPlannerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IMTPlannerDbContext>();
        optionsBuilder.UseSqlite("Data Source=IMT_Planner.db");
        return new IMTPlannerDbContext(optionsBuilder.Options);
    }
}