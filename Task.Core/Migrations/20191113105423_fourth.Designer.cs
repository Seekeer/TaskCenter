﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Task.Core.Data;

namespace Task.Core.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20191113105423_fourth")]
    partial class fourth
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Task.Core.Model.Executor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LastPower")
                        .HasColumnType("int");

                    b.Property<int>("Power")
                        .HasColumnType("int");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Executors");
                });

            modelBuilder.Entity("Task.Core.Model.ExecutorTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ExecutorId")
                        .HasColumnType("int");

                    b.Property<string>("Groups")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TaskId")
                        .HasColumnType("int");

                    b.Property<int>("TelegramMessageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExecutorId");

                    b.HasIndex("TaskId");

                    b.ToTable("ExecutorTasks");
                });

            modelBuilder.Entity("Task.Core.Model.GlobalTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GlobalTasks");

                    b.HasDiscriminator<string>("Discriminator").HasValue("GlobalTask");
                });

            modelBuilder.Entity("Task.Core.Model.TaskStatusData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ExecutorTaskId")
                        .HasColumnType("int");

                    b.Property<bool>("IsUserSet")
                        .HasColumnType("bit");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ExecutorTaskId");

                    b.ToTable("TaskStatusData");
                });

            modelBuilder.Entity("Task.Core.Model.CommentsTask", b =>
                {
                    b.HasBaseType("Task.Core.Model.GlobalTask");

                    b.HasDiscriminator().HasValue("CommentsTask");
                });

            modelBuilder.Entity("Task.Core.Model.ExecutorTask", b =>
                {
                    b.HasOne("Task.Core.Model.Executor", "Executor")
                        .WithMany("ExecutorTasks")
                        .HasForeignKey("ExecutorId");

                    b.HasOne("Task.Core.Model.GlobalTask", "Task")
                        .WithMany("ExecutorTasks")
                        .HasForeignKey("TaskId");
                });

            modelBuilder.Entity("Task.Core.Model.TaskStatusData", b =>
                {
                    b.HasOne("Task.Core.Model.ExecutorTask", null)
                        .WithMany("Statuses")
                        .HasForeignKey("ExecutorTaskId");
                });
#pragma warning restore 612, 618
        }
    }
}
