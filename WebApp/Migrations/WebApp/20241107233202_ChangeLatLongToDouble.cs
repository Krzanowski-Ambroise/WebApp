using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations.WebApp
{
    /// <inheritdoc />
    public partial class ChangeLatLongToDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanieName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Siret = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Phone = table.Column<int>(type: "int", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Postes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ResponsabilityRank = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Postes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_RequirementTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_RequirementTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_TypeOfContracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_TypeOfContracts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Postcode = table.Column<int>(type: "int", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Sites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanieId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Postcode = table.Column<int>(type: "int", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Sites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Sites_CompanieId",
                        column: x => x.CompanieId,
                        principalTable: "T_Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "T_UserRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    RequirementTypeId = table.Column<int>(type: "int", nullable: false),
                    PersonnalEvaluation = table.Column<int>(type: "int", nullable: false),
                    OtherEvaluation = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_UserRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_UserRequirement_RequirementTypeId",
                        column: x => x.RequirementTypeId,
                        principalTable: "T_RequirementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_UserRequirements_Users",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCompanieAssociations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCompanieAssociations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCompanieAssociations_Companies",
                        column: x => x.CompanyId,
                        principalTable: "T_Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCompanieAssociations_Users",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_Exerces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    PosteId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    TypeOfContractId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Exerces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exerces_TypeOfContracts",
                        column: x => x.TypeOfContractId,
                        principalTable: "T_TypeOfContracts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_Exerces_PosteId",
                        column: x => x.PosteId,
                        principalTable: "T_Postes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_Exerces_SiteId",
                        column: x => x.SiteId,
                        principalTable: "T_Sites",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_Exerces_T_Users",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_JobPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PosteId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfContractId = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    CompanieId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_JobPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobPosts_TypeOfContracts",
                        column: x => x.TypeOfContractId,
                        principalTable: "T_TypeOfContracts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_JobPost_CompanieId",
                        column: x => x.CompanieId,
                        principalTable: "T_Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_JobPost_PosteId",
                        column: x => x.PosteId,
                        principalTable: "T_Postes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_JobPost_SiteId",
                        column: x => x.SiteId,
                        principalTable: "T_Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_JobPostRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobPostId = table.Column<int>(type: "int", nullable: true),
                    RequirementTypeId = table.Column<int>(type: "int", nullable: true),
                    Evaluation = table.Column<int>(type: "int", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_JobPostRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_JobPostRequirement_JobPostId",
                        column: x => x.JobPostId,
                        principalTable: "T_JobPosts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_T_JobPostRequirement_RequirementTypeId",
                        column: x => x.RequirementTypeId,
                        principalTable: "T_RequirementTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_Exerces_PosteId",
                table: "T_Exerces",
                column: "PosteId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Exerces_SiteId",
                table: "T_Exerces",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Exerces_TypeOfContractId",
                table: "T_Exerces",
                column: "TypeOfContractId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Exerces_UserId",
                table: "T_Exerces",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_JobPostRequirements_JobPostId",
                table: "T_JobPostRequirements",
                column: "JobPostId");

            migrationBuilder.CreateIndex(
                name: "IX_T_JobPostRequirements_RequirementTypeId",
                table: "T_JobPostRequirements",
                column: "RequirementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_T_JobPosts_CompanieId",
                table: "T_JobPosts",
                column: "CompanieId");

            migrationBuilder.CreateIndex(
                name: "IX_T_JobPosts_PosteId",
                table: "T_JobPosts",
                column: "PosteId");

            migrationBuilder.CreateIndex(
                name: "IX_T_JobPosts_SiteId",
                table: "T_JobPosts",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_T_JobPosts_TypeOfContractId",
                table: "T_JobPosts",
                column: "TypeOfContractId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Sites_CompanieId",
                table: "T_Sites",
                column: "CompanieId");

            migrationBuilder.CreateIndex(
                name: "IX_T_UserRequirements_RequirementTypeId",
                table: "T_UserRequirements",
                column: "RequirementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_T_UserRequirements_UserId",
                table: "T_UserRequirements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanieAssociations_CompanyId",
                table: "UserCompanieAssociations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanieAssociations_UserId",
                table: "UserCompanieAssociations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_Exerces");

            migrationBuilder.DropTable(
                name: "T_JobPostRequirements");

            migrationBuilder.DropTable(
                name: "T_UserRequirements");

            migrationBuilder.DropTable(
                name: "UserCompanieAssociations");

            migrationBuilder.DropTable(
                name: "T_JobPosts");

            migrationBuilder.DropTable(
                name: "T_RequirementTypes");

            migrationBuilder.DropTable(
                name: "T_Users");

            migrationBuilder.DropTable(
                name: "T_TypeOfContracts");

            migrationBuilder.DropTable(
                name: "T_Postes");

            migrationBuilder.DropTable(
                name: "T_Sites");

            migrationBuilder.DropTable(
                name: "T_Companies");
        }
    }
}
