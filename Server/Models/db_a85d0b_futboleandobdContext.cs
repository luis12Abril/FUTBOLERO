using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FUTBOLERO.Server.Models
{
    public partial class db_a85d0b_futboleandobdContext : DbContext
    {
        public db_a85d0b_futboleandobdContext()
        {
        }

        public db_a85d0b_futboleandobdContext(DbContextOptions<db_a85d0b_futboleandobdContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Arbitro> Arbitro { get; set; }
        public virtual DbSet<Arbitrocolegio> Arbitrocolegio { get; set; }
        public virtual DbSet<Avisofutboleando> Avisofutboleando { get; set; }
        public virtual DbSet<Boton> Boton { get; set; }
        public virtual DbSet<Campo> Campo { get; set; }
        public virtual DbSet<Campocolegio> Campocolegio { get; set; }
        public virtual DbSet<Colegioarbitro> Colegioarbitro { get; set; }
        public virtual DbSet<Comentario> Comentario { get; set; }
        public virtual DbSet<Comunicado> Comunicado { get; set; }
        public virtual DbSet<Configuracion> Configuracion { get; set; }
        public virtual DbSet<Equipo> Equipo { get; set; }
        public virtual DbSet<Equipocolegio> Equipocolegio { get; set; }
        public virtual DbSet<Estado> Estado { get; set; }
        public virtual DbSet<Estatusjuego> Estatusjuego { get; set; }
        public virtual DbSet<Gol> Gol { get; set; }
        public virtual DbSet<Jornada> Jornada { get; set; }
        public virtual DbSet<Juego> Juego { get; set; }
        public virtual DbSet<Jugador> Jugador { get; set; }
        public virtual DbSet<Liga> Liga { get; set; }
        public virtual DbSet<Municipio> Municipio { get; set; }
        public virtual DbSet<Pagina> Pagina { get; set; }
        public virtual DbSet<Paginatipousuario> Paginatipousuario { get; set; }
        public virtual DbSet<Paginatipousuarioboton> Paginatipousuarioboton { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<Programacioncolegio> Programacioncolegio { get; set; }
        public virtual DbSet<Publicidad> Publicidad { get; set; }
        public virtual DbSet<Tipousuario> Tipousuario { get; set; }
        public virtual DbSet<Torneo> Torneo { get; set; }
        public virtual DbSet<Ultimoscinco> Ultimoscinco { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Usuariotorneo> Usuariotorneo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=sql5052.site4now.net;database= db_a85d0b_futboleandobd;User Id=db_a85d0b_futboleandobd_admin; password = Labt1970");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arbitro>(entity =>
            {
                entity.HasKey(e => e.Idarbitro);

                entity.ToTable("ARBITRO");

                entity.Property(e => e.Idarbitro).HasColumnName("IDARBITRO");

                entity.Property(e => e.Apmaterno)
                    .HasColumnName("APMATERNO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Appaterno)
                    .HasColumnName("APPATERNO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Torneo)
                    .HasColumnName("TORNEO")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Arbitrocolegio>(entity =>
            {
                entity.HasKey(e => e.Idarbitrocolegio);

                entity.ToTable("ARBITROCOLEGIO");

                entity.Property(e => e.Idarbitrocolegio).HasColumnName("IDARBITROCOLEGIO");

                entity.Property(e => e.Apmaterno)
                    .HasColumnName("APMATERNO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Appaterno)
                    .HasColumnName("APPATERNO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Codigoactual)
                    .HasColumnName("CODIGOACTUAL")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fnacimiento)
                    .HasColumnName("FNACIMIENTO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fotoarbitro)
                    .HasColumnName("FOTOARBITRO")
                    .IsUnicode(false);

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idcolegioarbitro).HasColumnName("IDCOLEGIOARBITRO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nomusuario)
                    .HasColumnName("NOMUSUARIO")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Peso).HasColumnName("PESO");
            });

            modelBuilder.Entity<Avisofutboleando>(entity =>
            {
                entity.HasKey(e => e.Idavisofutboleando);

                entity.ToTable("AVISOFUTBOLEANDO");

                entity.Property(e => e.Idavisofutboleando).HasColumnName("IDAVISOFUTBOLEANDO");

                entity.Property(e => e.Fechamensaje)
                    .HasColumnName("FECHAMENSAJE")
                    .HasColumnType("date");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Mensaje)
                    .HasColumnName("MENSAJE")
                    .IsUnicode(false);

                entity.Property(e => e.Titulomensaje)
                    .HasColumnName("TITULOMENSAJE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Boton>(entity =>
            {
                entity.HasKey(e => e.Idboton);

                entity.ToTable("BOTON");

                entity.Property(e => e.Idboton).HasColumnName("IDBOTON");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Campo>(entity =>
            {
                entity.HasKey(e => e.Idcampo);

                entity.ToTable("CAMPO");

                entity.Property(e => e.Idcampo).HasColumnName("IDCAMPO");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Torneo)
                    .HasColumnName("TORNEO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ubicacion)
                    .HasColumnName("UBICACION")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Campocolegio>(entity =>
            {
                entity.HasKey(e => e.Idcampocolegio);

                entity.ToTable("CAMPOCOLEGIO");

                entity.Property(e => e.Idcampocolegio).HasColumnName("IDCAMPOCOLEGIO");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idcolegioarbitro).HasColumnName("IDCOLEGIOARBITRO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Ubicacion)
                    .HasColumnName("UBICACION")
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Colegioarbitro>(entity =>
            {
                entity.HasKey(e => e.Idcolegioarbitro);

                entity.ToTable("COLEGIOARBITRO");

                entity.Property(e => e.Idcolegioarbitro).HasColumnName("IDCOLEGIOARBITRO");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idpresidente).HasColumnName("IDPRESIDENTE");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasKey(e => e.Idcomentario);

                entity.ToTable("COMENTARIO");

                entity.Property(e => e.Idcomentario).HasColumnName("IDCOMENTARIO");

                entity.Property(e => e.Comentario1)
                    .HasColumnName("COMENTARIO")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Fechacomentario)
                    .HasColumnName("FECHACOMENTARIO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idjuego).HasColumnName("IDJUEGO");

                entity.Property(e => e.Idusuario).HasColumnName("IDUSUARIO");
            });

            modelBuilder.Entity<Comunicado>(entity =>
            {
                entity.HasKey(e => e.Idcomunicado);

                entity.ToTable("COMUNICADO");

                entity.Property(e => e.Idcomunicado).HasColumnName("IDCOMUNICADO");

                entity.Property(e => e.Comunicadocorto)
                    .HasColumnName("COMUNICADOCORTO")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Comunicadolargo)
                    .HasColumnName("COMUNICADOLARGO")
                    .IsUnicode(false);

                entity.Property(e => e.Fechacomunicado)
                    .HasColumnName("FECHACOMUNICADO")
                    .HasColumnType("date");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");
            });

            modelBuilder.Entity<Configuracion>(entity =>
            {
                entity.HasKey(e => e.Idconfiguracion);

                entity.ToTable("CONFIGURACION");

                entity.Property(e => e.Idconfiguracion)
                    .HasColumnName("IDCONFIGURACION")
                    .ValueGeneratedNever();

                entity.Property(e => e.Switch1).HasColumnName("SWITCH1");

                entity.Property(e => e.Switch2).HasColumnName("SWITCH2");

                entity.Property(e => e.Switch3).HasColumnName("SWITCH3");
            });

            modelBuilder.Entity<Equipo>(entity =>
            {
                entity.HasKey(e => e.Idequipo);

                entity.ToTable("EQUIPO");

                entity.Property(e => e.Idequipo).HasColumnName("IDEQUIPO");

                entity.Property(e => e.Claequipo)
                    .HasColumnName("CLAEQUIPO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Difgoles).HasColumnName("DIFGOLES");

                entity.Property(e => e.Empatados).HasColumnName("EMPATADOS");

                entity.Property(e => e.Empatadosganados).HasColumnName("EMPATADOSGANADOS");

                entity.Property(e => e.Empatadosperdidos).HasColumnName("EMPATADOSPERDIDOS");

                entity.Property(e => e.Fotoequipo)
                    .HasColumnName("FOTOEQUIPO")
                    .IsUnicode(false);

                entity.Property(e => e.Ganados).HasColumnName("GANADOS");

                entity.Property(e => e.Ganadosadmo).HasColumnName("GANADOSADMO");

                entity.Property(e => e.Golesafavor).HasColumnName("GOLESAFAVOR");

                entity.Property(e => e.Golesencontra).HasColumnName("GOLESENCONTRA");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");

                entity.Property(e => e.Jugados).HasColumnName("JUGADOS");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Perdidos).HasColumnName("PERDIDOS");

                entity.Property(e => e.Perdidosadmo).HasColumnName("PERDIDOSADMO");

                entity.Property(e => e.Puntos).HasColumnName("PUNTOS");

                entity.Property(e => e.Puntosextras).HasColumnName("PUNTOSEXTRAS");

                entity.Property(e => e.Representante)
                    .HasColumnName("REPRESENTANTE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Torneo)
                    .HasColumnName("TORNEO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Usuequipo)
                    .HasColumnName("USUEQUIPO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Vigencia)
                    .HasColumnName("VIGENCIA")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Equipocolegio>(entity =>
            {
                entity.HasKey(e => e.Idequipocolegio);

                entity.ToTable("EQUIPOCOLEGIO");

                entity.Property(e => e.Idequipocolegio).HasColumnName("IDEQUIPOCOLEGIO");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idcolegioarbitro).HasColumnName("IDCOLEGIOARBITRO");

                entity.Property(e => e.Idtorneocolegio).HasColumnName("IDTORNEOCOLEGIO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.HasKey(e => e.Idestado);

                entity.ToTable("ESTADO");

                entity.Property(e => e.Idestado).HasColumnName("IDESTADO");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(509)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Estatusjuego>(entity =>
            {
                entity.HasKey(e => e.Idestatusjuego);

                entity.ToTable("ESTATUSJUEGO");

                entity.Property(e => e.Idestatusjuego).HasColumnName("IDESTATUSJUEGO");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Torneo)
                    .HasColumnName("TORNEO")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Gol>(entity =>
            {
                entity.HasKey(e => e.Idgol);

                entity.ToTable("GOL");

                entity.Property(e => e.Idgol).HasColumnName("IDGOL");

                entity.Property(e => e.Goles).HasColumnName("GOLES");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idequipo).HasColumnName("IDEQUIPO");

                entity.Property(e => e.Idjuego).HasColumnName("IDJUEGO");

                entity.Property(e => e.Idjugador).HasColumnName("IDJUGADOR");
            });

            modelBuilder.Entity<Jornada>(entity =>
            {
                entity.HasKey(e => e.Idjornada);

                entity.ToTable("JORNADA");

                entity.Property(e => e.Idjornada).HasColumnName("IDJORNADA");

                entity.Property(e => e.Finiciojornada)
                    .HasColumnName("FINICIOJORNADA")
                    .HasColumnType("date");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Torneo)
                    .HasColumnName("TORNEO")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Juego>(entity =>
            {
                entity.HasKey(e => e.Idjuego);

                entity.ToTable("JUEGO");

                entity.Property(e => e.Idjuego).HasColumnName("IDJUEGO");

                entity.Property(e => e.Cuentaparagoles).HasColumnName("CUENTAPARAGOLES");

                entity.Property(e => e.Cuentaparapuntos).HasColumnName("CUENTAPARAPUNTOS");

                entity.Property(e => e.Fhorario)
                    .HasColumnName("FHORARIO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Golesequipo01).HasColumnName("GOLESEQUIPO01");

                entity.Property(e => e.Golesequipo02).HasColumnName("GOLESEQUIPO02");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idarbitro).HasColumnName("IDARBITRO");

                entity.Property(e => e.Idcampo).HasColumnName("IDCAMPO");

                entity.Property(e => e.Idequipo01).HasColumnName("IDEQUIPO01");

                entity.Property(e => e.Idequipo02).HasColumnName("IDEQUIPO02");

                entity.Property(e => e.Idestatusjuego).HasColumnName("IDESTATUSJUEGO");

                entity.Property(e => e.Idjornada).HasColumnName("IDJORNADA");

                entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");

                entity.Property(e => e.Peequipo01).HasColumnName("PEEQUIPO01");

                entity.Property(e => e.Peequipo02).HasColumnName("PEEQUIPO02");

                entity.Property(e => e.Puntosequipo01).HasColumnName("PUNTOSEQUIPO01");

                entity.Property(e => e.Puntosequipo02).HasColumnName("PUNTOSEQUIPO02");

                entity.Property(e => e.Resequipo01)
                    .HasColumnName("RESEQUIPO01")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Resequipo02)
                    .HasColumnName("RESEQUIPO02")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Torneo)
                    .HasColumnName("TORNEO")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Jugador>(entity =>
            {
                entity.HasKey(e => e.Idjugador);

                entity.ToTable("JUGADOR");

                entity.Property(e => e.Idjugador).HasColumnName("IDJUGADOR");

                entity.Property(e => e.Apmaterno)
                    .HasColumnName("APMATERNO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Appaterno)
                    .HasColumnName("APPATERNO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fnacimiento)
                    .HasColumnName("FNACIMIENTO")
                    .HasColumnType("date");

                entity.Property(e => e.Goles).HasColumnName("GOLES");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idequipo).HasColumnName("IDEQUIPO");

                entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Torneo)
                    .HasColumnName("TORNEO")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Liga>(entity =>
            {
                entity.HasKey(e => e.Idliga);

                entity.ToTable("LIGA");

                entity.Property(e => e.Idliga).HasColumnName("IDLIGA");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idmunicipio).HasColumnName("IDMUNICIPIO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Municipio>(entity =>
            {
                entity.HasKey(e => e.Idmunicipio);

                entity.ToTable("MUNICIPIO");

                entity.Property(e => e.Idmunicipio).HasColumnName("IDMUNICIPIO");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idestado).HasColumnName("IDESTADO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pagina>(entity =>
            {
                entity.HasKey(e => e.Idpagina);

                entity.ToTable("PAGINA");

                entity.Property(e => e.Idpagina).HasColumnName("IDPAGINA");

                entity.Property(e => e.Accion)
                    .HasColumnName("ACCION")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Mensaje)
                    .HasColumnName("MENSAJE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ordenmenu).HasColumnName("ORDENMENU");

                entity.Property(e => e.Visible).HasColumnName("VISIBLE");
            });

            modelBuilder.Entity<Paginatipousuario>(entity =>
            {
                entity.HasKey(e => e.Idpaginatipousuario);

                entity.ToTable("PAGINATIPOUSUARIO");

                entity.Property(e => e.Idpaginatipousuario).HasColumnName("IDPAGINATIPOUSUARIO");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idpagina).HasColumnName("IDPAGINA");

                entity.Property(e => e.Idtipousuario).HasColumnName("IDTIPOUSUARIO");
            });

            modelBuilder.Entity<Paginatipousuarioboton>(entity =>
            {
                entity.HasKey(e => e.Idpaginatipousuarioboton);

                entity.ToTable("PAGINATIPOUSUARIOBOTON");

                entity.Property(e => e.Idpaginatipousuarioboton).HasColumnName("IDPAGINATIPOUSUARIOBOTON");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idboton).HasColumnName("IDBOTON");

                entity.Property(e => e.Idpaginatipousuario).HasColumnName("IDPAGINATIPOUSUARIO");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.Idpersona);

                entity.ToTable("PERSONA");

                entity.Property(e => e.Idpersona).HasColumnName("IDPERSONA");

                entity.Property(e => e.Apmaterno)
                    .HasColumnName("APMATERNO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Appaterno)
                    .HasColumnName("APPATERNO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Correo)
                    .HasColumnName("CORREO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fechanacimiento)
                    .HasColumnName("FECHANACIMIENTO")
                    .HasColumnType("date");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasColumnName("TELEFONO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tieneusuario).HasColumnName("TIENEUSUARIO");
            });

            modelBuilder.Entity<Programacioncolegio>(entity =>
            {
                entity.HasKey(e => e.Idprogramacioncolegio);

                entity.ToTable("PROGRAMACIONCOLEGIO");

                entity.Property(e => e.Idprogramacioncolegio).HasColumnName("IDPROGRAMACIONCOLEGIO");

                entity.Property(e => e.Comentario)
                    .HasColumnName("COMENTARIO")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Fjuegocolegio)
                    .HasColumnName("FJUEGOCOLEGIO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idarbitrocolegio).HasColumnName("IDARBITROCOLEGIO");

                entity.Property(e => e.Idcampocolegio).HasColumnName("IDCAMPOCOLEGIO");

                entity.Property(e => e.Idcolegioarbitro).HasColumnName("IDCOLEGIOARBITRO");

                entity.Property(e => e.Idequipocolegio01).HasColumnName("IDEQUIPOCOLEGIO01");

                entity.Property(e => e.Idequipocolegio02).HasColumnName("IDEQUIPOCOLEGIO02");
            });

            modelBuilder.Entity<Publicidad>(entity =>
            {
                entity.HasKey(e => e.Idpublicidad);

                entity.ToTable("PUBLICIDAD");

                entity.Property(e => e.Idpublicidad).HasColumnName("IDPUBLICIDAD");

                entity.Property(e => e.Foto)
                    .HasColumnName("FOTO")
                    .IsUnicode(false);

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");

                entity.Property(e => e.Orden).HasColumnName("ORDEN");
            });

            modelBuilder.Entity<Tipousuario>(entity =>
            {
                entity.HasKey(e => e.Idtipousuario);

                entity.ToTable("TIPOUSUARIO");

                entity.Property(e => e.Idtipousuario).HasColumnName("IDTIPOUSUARIO");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("DESCRIPCION")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Torneo>(entity =>
            {
                entity.HasKey(e => e.Idtorneo);

                entity.ToTable("TORNEO");

                entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");

                entity.Property(e => e.Clavetorneo)
                    .HasColumnName("CLAVETORNEO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idliga).HasColumnName("IDLIGA");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ordentorneo).HasColumnName("ORDENTORNEO");

                entity.Property(e => e.Visible).HasColumnName("VISIBLE");

                entity.Property(e => e.Visitas).HasColumnName("VISITAS");

                entity.Property(e => e.Visitascel).HasColumnName("VISITASCEL");
            });

            modelBuilder.Entity<Ultimoscinco>(entity =>
            {
                entity.HasKey(e => e.Idultimoscinco);

                entity.ToTable("ULTIMOSCINCO");

                entity.Property(e => e.Idultimoscinco).HasColumnName("IDULTIMOSCINCO");

                entity.Property(e => e.C2)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.C3)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.C4)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.C5)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Idequipo).HasColumnName("IDEQUIPO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Puntos).HasColumnName("PUNTOS");

                entity.Property(e => e.Ultimo)
                    .HasColumnName("ULTIMO")
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Idusuario);

                entity.ToTable("USUARIO");

                entity.Property(e => e.Idusuario).HasColumnName("IDUSUARIO");

                entity.Property(e => e.Contraseña)
                    .HasColumnName("CONTRASEÑA")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fechaalta)
                    .HasColumnName("FECHAALTA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idarbitrocolegio).HasColumnName("IDARBITROCOLEGIO");

                entity.Property(e => e.Idpersona).HasColumnName("IDPERSONA");

                entity.Property(e => e.Idtipousuario).HasColumnName("IDTIPOUSUARIO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Origenalta)
                    .HasColumnName("ORIGENALTA")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .HasColumnName("TOKEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Visitas).HasColumnName("VISITAS");

                entity.Property(e => e.Visitascel).HasColumnName("VISITASCEL");
            });

            modelBuilder.Entity<Usuariotorneo>(entity =>
            {
                entity.HasKey(e => e.Idusuariotorneo);

                entity.ToTable("USUARIOTORNEO");

                entity.Property(e => e.Idusuariotorneo).HasColumnName("IDUSUARIOTORNEO");

                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");

                entity.Property(e => e.Idusuario).HasColumnName("IDUSUARIO");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
