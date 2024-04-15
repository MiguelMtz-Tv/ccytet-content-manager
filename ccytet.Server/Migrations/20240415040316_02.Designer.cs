﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ccytet.Server.Data;

#nullable disable

namespace ccytet.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240415040316_02")]
    partial class _02
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ccytet.Server.Models.Convocatoria", b =>
                {
                    b.Property<string>("IdConvocatoria")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Abierto")
                        .HasColumnType("bit");

                    b.Property<string>("Autor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdUserCreator")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdUserUpdater")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PortadaPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Texto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdConvocatoria");

                    b.HasIndex("IdUserCreator");

                    b.HasIndex("IdUserUpdater");

                    b.ToTable("Convocatorias");
                });

            modelBuilder.Entity("ccytet.Server.Models.EstadoSituacionFinanciera", b =>
                {
                    b.Property<string>("IdEstadoSituacionFinanciera")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Desde")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Hasta")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdUserCreator")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdUserUpdater")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Titulo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdEstadoSituacionFinanciera");

                    b.HasIndex("IdUserCreator");

                    b.HasIndex("IdUserUpdater");

                    b.ToTable("EstadosSituacionFinanciera");
                });

            modelBuilder.Entity("ccytet.Server.Models.Noticia", b =>
                {
                    b.Property<string>("IdNoticia")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Autor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("bit");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdUserCreator")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdUserUpdater")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImagesArray")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Portada")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Texto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserCreatorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserUpdaterName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdNoticia");

                    b.HasIndex("IdUserCreator");

                    b.HasIndex("IdUserUpdater");

                    b.ToTable("Noticias");
                });

            modelBuilder.Entity("ccytet.Server.Models.AspNetUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("Apellidos")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Matricula")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("AspNetUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ccytet.Server.Models.Convocatoria", b =>
                {
                    b.HasOne("ccytet.Server.Models.AspNetUser", "UserCreator")
                        .WithMany("ConvocatoriasCreadas")
                        .HasForeignKey("IdUserCreator")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ccytet.Server.Models.AspNetUser", "UserUpdater")
                        .WithMany("ConvocatoriasActualizadas")
                        .HasForeignKey("IdUserUpdater")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("UserCreator");

                    b.Navigation("UserUpdater");
                });

            modelBuilder.Entity("ccytet.Server.Models.EstadoSituacionFinanciera", b =>
                {
                    b.HasOne("ccytet.Server.Models.AspNetUser", "UserCreator")
                        .WithMany("ESFCreados")
                        .HasForeignKey("IdUserCreator")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ccytet.Server.Models.AspNetUser", "UserUpdater")
                        .WithMany("ESFActualizados")
                        .HasForeignKey("IdUserUpdater")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("UserCreator");

                    b.Navigation("UserUpdater");
                });

            modelBuilder.Entity("ccytet.Server.Models.Noticia", b =>
                {
                    b.HasOne("ccytet.Server.Models.AspNetUser", "UserCreator")
                        .WithMany("NoticiasCreadas")
                        .HasForeignKey("IdUserCreator")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ccytet.Server.Models.AspNetUser", "UserUpdater")
                        .WithMany("NoticiasActualizadas")
                        .HasForeignKey("IdUserUpdater")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("UserCreator");

                    b.Navigation("UserUpdater");
                });

            modelBuilder.Entity("ccytet.Server.Models.AspNetUser", b =>
                {
                    b.Navigation("ConvocatoriasActualizadas");

                    b.Navigation("ConvocatoriasCreadas");

                    b.Navigation("ESFActualizados");

                    b.Navigation("ESFCreados");

                    b.Navigation("NoticiasActualizadas");

                    b.Navigation("NoticiasCreadas");
                });
#pragma warning restore 612, 618
        }
    }
}