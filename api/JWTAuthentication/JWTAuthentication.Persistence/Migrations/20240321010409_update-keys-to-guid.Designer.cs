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
    [DbContext(typeof(AuthenticationOrganizationContext))]
    [Migration("20240321010409_update-keys-to-guid")]
    partial class updatekeystoguid
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
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

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

                    b.HasKey("Uuid");

                    b.ToTable("JwtClaim", "app");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.Roles.Role", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

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

                    b.HasKey("Uuid");

                    b.ToTable("Role", "app");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims.RoleJwtClaim", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("JwtClaimUuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<Guid?>("RoleUuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Uuid");

                    b.HasIndex("JwtClaimUuid");

                    b.HasIndex("RoleUuid");

                    b.ToTable("RoleJwtClaim", "app");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.Usuario", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("JwtClaimUuid")
                        .HasColumnType("uniqueidentifier");

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

                    b.HasKey("Uuid");

                    b.HasIndex("JwtClaimUuid");

                    b.ToTable("Usuario", "app");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.Roles.RoleJwtClaims.RoleJwtClaim", b =>
                {
                    b.HasOne("JWTAuthentication.Domain.Usuarios.JwsClaims.JwtClaim", "JwtClaim")
                        .WithMany("RoleJwtClaims")
                        .HasForeignKey("JwtClaimUuid");

                    b.HasOne("JWTAuthentication.Domain.Usuarios.Roles.Role", "Role")
                        .WithMany("RoleJwtClaims")
                        .HasForeignKey("RoleUuid");

                    b.Navigation("JwtClaim");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.Usuario", b =>
                {
                    b.HasOne("JWTAuthentication.Domain.Usuarios.JwsClaims.JwtClaim", "JwtClaims")
                        .WithMany()
                        .HasForeignKey("JwtClaimUuid");

                    b.Navigation("JwtClaims");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.JwsClaims.JwtClaim", b =>
                {
                    b.Navigation("RoleJwtClaims");
                });

            modelBuilder.Entity("JWTAuthentication.Domain.Usuarios.Roles.Role", b =>
                {
                    b.Navigation("RoleJwtClaims");
                });
#pragma warning restore 612, 618
        }
    }
}
