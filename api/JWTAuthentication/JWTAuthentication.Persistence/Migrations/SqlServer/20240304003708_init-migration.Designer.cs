﻿// <auto-generated />
using System;
using JWTAuthentication.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JWTAuthentication.Persistence.Migrations
{
    [DbContext(typeof(AuthenticationOrganizationContextSqlServer))]
    [Migration("20240304003708_init-migration")]
    partial class initmigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("app")
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.JwsClaims.JwtClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("JwtClaim", "app");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Role", "app");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole.RoleJwtClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int?>("ClaimId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("JwtClaimId")
                        .HasColumnType("int");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("JwtClaimId");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleJwtClaim", "app");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("JwtClaimId")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("JwtClaimId");

                    b.ToTable("Usuario", "app");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.UsuariosRole.RoleJwtClaim", b =>
                {
                    b.HasOne("JWTAuthentication.Domain.Usuarios.JwsClaims.JwtClaim", "JwtClaim")
                        .WithMany("RoleJwtClaims")
                        .HasForeignKey("JwtClaimId");

                    b.HasOne("JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.Role", "Role")
                        .WithMany("RoleJwtClaims")
                        .HasForeignKey("RoleId");

                    b.Navigation("JwtClaim");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.Usuario", b =>
                {
                    b.HasOne("JWTAuthentication.Domain.Usuarios.JwsClaims.JwtClaim", "JwtClaims")
                        .WithMany()
                        .HasForeignKey("JwtClaimId");

                    b.Navigation("JwtClaims");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.JwsClaims.JwtClaim", b =>
                {
                    b.Navigation("RoleJwtClaims");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.JwsClaims.Roles.Role", b =>
                {
                    b.Navigation("RoleJwtClaims");
                });
#pragma warning restore 612, 618
        }
    }
}