using Microsoft.EntityFrameworkCore;
using Proyecto_Order2Go.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Order2Go.DataContext
{
    public class CodeStackCTX : DbContext
    {
        public CodeStackCTX(DbContextOptions<CodeStackCTX> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UsuarioRol>().HasKey(x => new { x.IdUsuario, x.IdRol });
        }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UsuarioRol> UsuarioRol { get; set; }
        public DbSet<Comercio> Comercio { get; set; }
        public DbSet<Producto> Producto { get; set; }
    }
}
