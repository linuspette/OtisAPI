﻿using Microsoft.EntityFrameworkCore;
using OtisAPI.Model.DataEntities;

namespace OtisAPI.DataAccess;

public class SqlContext : DbContext
{
    public SqlContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ErrandEntity> Errands { get; set; } = null!;
}