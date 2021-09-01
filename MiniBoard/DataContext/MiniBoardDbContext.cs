using Microsoft.EntityFrameworkCore;
using MiniBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBoard.DataContext
{
    public class MiniBoardDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Reply> Replys { get; set; }

        // DB연결에 필요한 connectionString 설정
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer
                ("Server=localhost;Database=MiniBoardDb;User Id=sa;Password=1q2w3e4r;");
        }
    }
}
