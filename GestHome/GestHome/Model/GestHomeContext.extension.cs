using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestHome.Model
{
    public partial class GestHomeContext : DbContext
    {
        public GestHomeContext(DbContextOptions<GestHomeContext> options)
            : base(options)
        { }

    }
}
