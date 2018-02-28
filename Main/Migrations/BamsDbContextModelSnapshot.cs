﻿// <auto-generated />
using Main.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Main.Migrations
{
    [DbContext(typeof(BamsDbContext))]
    partial class BamsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Main.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Main.Models.Asset", b =>
                {
                    b.Property<int>("AssetId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AssetName");

                    b.Property<string>("LongDescription");

                    b.Property<string>("ShortDescription");

                    b.Property<string>("assetTypeName");

                    b.Property<bool>("isDeleted");

                    b.Property<bool>("isPreferredAsset");

                    b.HasKey("AssetId");

                    b.HasIndex("assetTypeName");

                    b.ToTable("Asset");
                });

            modelBuilder.Entity("Main.Models.AssetData", b =>
                {
                    b.Property<int>("assetID");

                    b.Property<string>("fieldName");

                    b.Property<bool>("boolValue");

                    b.Property<DateTime>("dateValue");

                    b.Property<string>("fieldType");

                    b.Property<double>("floatValue");

                    b.Property<long>("intValue");

                    b.Property<bool>("isPrimaryKey");

                    b.Property<string>("strValue");

                    b.HasKey("assetID", "fieldName");

                    b.ToTable("AssetData");
                });

            modelBuilder.Entity("Main.Models.AssetModule", b =>
                {
                    b.Property<int>("assetID");

                    b.Property<int>("moduleID");

                    b.HasKey("assetID", "moduleID");

                    b.HasIndex("moduleID");

                    b.ToTable("AssetModule");
                });

            modelBuilder.Entity("Main.Models.AssetType", b =>
                {
                    b.Property<string>("Name");

                    b.HasKey("Name");

                    b.ToTable("AssetTypes");
                });

            modelBuilder.Entity("Main.Models.DataSchema", b =>
                {
                    b.Property<int>("schemaID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("assetTypeName");

                    b.Property<string>("fieldName");

                    b.Property<string>("fieldType");

                    b.Property<bool>("isPrimary");

                    b.Property<string>("schemaName");

                    b.HasKey("schemaID");

                    b.HasIndex("assetTypeName");

                    b.ToTable("DataSchema");
                });

            modelBuilder.Entity("Main.Models.Module", b =>
                {
                    b.Property<int>("moduleID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("detail1");

                    b.Property<string>("detail2");

                    b.Property<string>("detail3");

                    b.Property<string>("detail4");

                    b.Property<string>("detail5");

                    b.Property<string>("name");

                    b.HasKey("moduleID");

                    b.HasIndex("name");

                    b.ToTable("Module");
                });

            modelBuilder.Entity("Main.Models.Reporting", b =>
                {
                    b.Property<int>("ReportID");

                    b.Property<DateTime?>("DateCreate");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Name");

                    b.HasKey("ReportID");

                    b.ToTable("Reporting");
                });

            modelBuilder.Entity("Main.Models.TaskQueue", b =>
                {
                    b.Property<int>("AssetId");

                    b.Property<string>("Name");

                    b.Property<string>("alertMessage");

                    b.Property<DateTime?>("dateComplete");

                    b.Property<bool>("isComplete");

                    b.Property<string>("resolvedBy");

                    b.HasKey("AssetId");

                    b.ToTable("TaskQueue");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Main.Models.Asset", b =>
                {
                    b.HasOne("Main.Models.AssetType", "assetType")
                        .WithMany()
                        .HasForeignKey("assetTypeName");
                });

            modelBuilder.Entity("Main.Models.AssetModule", b =>
                {
                    b.HasOne("Main.Models.Asset", "asset")
                        .WithMany()
                        .HasForeignKey("assetID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Main.Models.Module", "module")
                        .WithMany()
                        .HasForeignKey("moduleID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Main.Models.DataSchema", b =>
                {
                    b.HasOne("Main.Models.AssetType", "assetType")
                        .WithMany()
                        .HasForeignKey("assetTypeName");
                });

            modelBuilder.Entity("Main.Models.Module", b =>
                {
                    b.HasOne("Main.Models.AssetType", "type")
                        .WithMany()
                        .HasForeignKey("name");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Main.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Main.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Main.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Main.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
