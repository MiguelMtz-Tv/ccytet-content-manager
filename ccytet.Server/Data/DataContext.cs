using ccytet.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ccytet.Server.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //RELACIONES DE USUARO CON NOTICIAS
            builder.Entity<Noticia>().HasOne(n => n.UserCreator).WithMany(uc => uc.NoticiasCreadas).HasForeignKey(n => n.IdUserCreator).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Noticia>().HasOne(n => n.UserUpdater).WithMany(uu => uu.NoticiasActualizadas).HasForeignKey(n => n.IdUserUpdater).OnDelete(DeleteBehavior.Restrict);

            //RELACIONES DE USUARIOS CON CONVOCATORIAS
            builder.Entity<Convocatoria>().HasOne(c => c.UserCreator).WithMany(uc => uc.ConvocatoriasCreadas).HasForeignKey(c => c.IdUserCreator).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Convocatoria>().HasOne(c => c.UserUpdater).WithMany(uu => uu.ConvocatoriasActualizadas).HasForeignKey(c => c.IdUserUpdater).OnDelete(DeleteBehavior.Restrict);

            //RELACION DE USUARIO CON ESF
            builder.Entity<EstadoSituacionFinanciera>().HasOne(esf => esf.UserCreator).WithMany(uc => uc.ESFCreados).HasForeignKey(esf => esf.IdUserCreator).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<EstadoSituacionFinanciera>().HasOne(esf => esf.UserUpdater).WithMany(uu => uu.ESFActualizados).HasForeignKey(esf => esf.IdUserUpdater).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EstadoSituacionFinancieraArchivo>().HasOne(x => x.EstadoSituacionFinanciera).WithMany(x => x.EstadoSituacionFinancieraArchivos).HasForeignKey(x => x.IdEstadoSituacionFinanciera).OnDelete(DeleteBehavior.Restrict); 
        }

        //A
        public DbSet<AspNetUser> AspNetUsers                                                { get; set; }
        //C
        public DbSet<Convocatoria> Convocatorias                                            { get; set; }
        //E
        public DbSet<EstadoSituacionFinanciera> EstadosSituacionFinanciera                  { get; set; }
        public DbSet<EstadoSituacionFinancieraArchivo> EstadoSituacionFinancieraArchivos    { get; set; }
        //N
        public DbSet<Noticia> Noticias                                                      { get; set; }
    }
}