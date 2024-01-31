using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AjoloApp.Repository.Migrations
{
    public partial class CreationKidBackgroundTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KidBackground",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AgeMotherDuringGestation = table.Column<int>(type: "integer", nullable: false),
                    IsPlanified = table.Column<bool>(type: "boolean", nullable: false),
                    ThreatOfAbortion = table.Column<bool>(type: "boolean", nullable: false),
                    PrenatalCheckUp = table.Column<bool>(type: "boolean", nullable: false),
                    UrineTestDone = table.Column<bool>(type: "boolean", nullable: false),
                    BloodTestDone = table.Column<bool>(type: "boolean", nullable: false),
                    OtherTests = table.Column<string>(type: "text", nullable: false),
                    XRays3rdMonth = table.Column<bool>(type: "boolean", nullable: false),
                    DrinkDuringGestation = table.Column<bool>(type: "boolean", nullable: false),
                    MedicinesConsumed = table.Column<string>(type: "text", nullable: false),
                    PhysicalConditionsDuringGestation = table.Column<string>(type: "text", nullable: false),
                    PsychologicalConditionsDuringGestation = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KidId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidBackground", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidBackground_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidBackground_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidBackground_Kids_KidId",
                        column: x => x.KidId,
                        principalTable: "Kids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KidBirthBackground",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ManyMonths = table.Column<int>(type: "integer", nullable: false),
                    HaveBreakupOfTheBag = table.Column<bool>(type: "boolean", nullable: false),
                    Premature = table.Column<bool>(type: "boolean", nullable: false),
                    Postmature = table.Column<bool>(type: "boolean", nullable: false),
                    NormalDelivery = table.Column<bool>(type: "boolean", nullable: false),
                    Fast = table.Column<bool>(type: "boolean", nullable: false),
                    Delayed = table.Column<bool>(type: "boolean", nullable: false),
                    HowManyTime = table.Column<string>(type: "text", nullable: false),
                    Induced = table.Column<bool>(type: "boolean", nullable: false),
                    MotherReceivedAnesthesia = table.Column<bool>(type: "boolean", nullable: false),
                    BornInHospital = table.Column<bool>(type: "boolean", nullable: false),
                    BornInHome = table.Column<bool>(type: "boolean", nullable: false),
                    WasAttendedBy = table.Column<string>(type: "text", nullable: false),
                    HeadPositionAtBirth = table.Column<string>(type: "text", nullable: false),
                    FeetPositionAtBirth = table.Column<string>(type: "text", nullable: false),
                    ButtocksPositionAtBirth = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KidId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidBirthBackground", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidBirthBackground_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidBirthBackground_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidBirthBackground_Kids_KidId",
                        column: x => x.KidId,
                        principalTable: "Kids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KidConditionNewBorn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Weight = table.Column<decimal>(type: "numeric", nullable: false),
                    Size = table.Column<decimal>(type: "numeric", nullable: false),
                    HeadLarge = table.Column<bool>(type: "boolean", nullable: false),
                    HeadSmall = table.Column<bool>(type: "boolean", nullable: false),
                    CryImmediately = table.Column<bool>(type: "boolean", nullable: false),
                    NeedOxigen = table.Column<bool>(type: "boolean", nullable: false),
                    UseIncubator = table.Column<bool>(type: "boolean", nullable: false),
                    HadMalformations = table.Column<bool>(type: "boolean", nullable: false),
                    DescribeMalformations = table.Column<string>(type: "text", nullable: false),
                    SufferedAnyDisease = table.Column<bool>(type: "boolean", nullable: false),
                    DescribeDisease = table.Column<string>(type: "text", nullable: false),
                    AgeDisease = table.Column<int>(type: "integer", nullable: false),
                    MedicamentsTreatDisease = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KidId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidConditionNewBorn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidConditionNewBorn_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidConditionNewBorn_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidConditionNewBorn_Kids_KidId",
                        column: x => x.KidId,
                        principalTable: "Kids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KidDreamBackground",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BabyDreamCalm = table.Column<bool>(type: "boolean", nullable: false),
                    BabyDreamShaken = table.Column<bool>(type: "boolean", nullable: false),
                    CurrentlyDreamCalm = table.Column<bool>(type: "boolean", nullable: false),
                    Shifts = table.Column<bool>(type: "boolean", nullable: false),
                    Speaks = table.Column<bool>(type: "boolean", nullable: false),
                    NocturnalFear = table.Column<bool>(type: "boolean", nullable: false),
                    Breathes = table.Column<bool>(type: "boolean", nullable: false),
                    ScratchesTeeth = table.Column<bool>(type: "boolean", nullable: false),
                    WakesUpALot = table.Column<bool>(type: "boolean", nullable: false),
                    Enuresis = table.Column<bool>(type: "boolean", nullable: false),
                    HasSingleRoom = table.Column<bool>(type: "boolean", nullable: false),
                    HasSingleBed = table.Column<bool>(type: "boolean", nullable: false),
                    WhoSleep = table.Column<string>(type: "text", nullable: false),
                    Bedtime = table.Column<string>(type: "text", nullable: false),
                    AwakeTime = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KidId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidDreamBackground", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidDreamBackground_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidDreamBackground_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidDreamBackground_Kids_KidId",
                        column: x => x.KidId,
                        principalTable: "Kids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KidFoodBackground",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WasBreastfed = table.Column<bool>(type: "boolean", nullable: false),
                    UntielWhenBreastfed = table.Column<string>(type: "text", nullable: false),
                    MotherBreastfed = table.Column<bool>(type: "boolean", nullable: false),
                    OtherPersonBreastfed = table.Column<string>(type: "text", nullable: false),
                    SinceGiveSolidFood = table.Column<string>(type: "text", nullable: false),
                    DescribeSolidFood = table.Column<string>(type: "text", nullable: false),
                    AcceptEasilySolidFood = table.Column<bool>(type: "boolean", nullable: false),
                    IntervalsSolidFood = table.Column<string>(type: "text", nullable: false),
                    CurrentlyEatGood = table.Column<string>(type: "text", nullable: false),
                    DescribeFoodEat = table.Column<string>(type: "text", nullable: false),
                    ChooseFood = table.Column<bool>(type: "boolean", nullable: false),
                    DescribeChooseFood = table.Column<string>(type: "text", nullable: false),
                    IsAllergicFood = table.Column<bool>(type: "boolean", nullable: false),
                    DescribeAllergicFood = table.Column<string>(type: "text", nullable: false),
                    HasTendencyToVomit = table.Column<bool>(type: "boolean", nullable: false),
                    HasTendencyToDiarrhea = table.Column<bool>(type: "boolean", nullable: false),
                    MedicationsInCaseFever = table.Column<string>(type: "text", nullable: false),
                    DosisInCaseFever = table.Column<string>(type: "text", nullable: false),
                    DescribeYourChildren = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KidId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidFoodBackground", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidFoodBackground_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidFoodBackground_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidFoodBackground_Kids_KidId",
                        column: x => x.KidId,
                        principalTable: "Kids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KidLanguageBackground",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PronounceSyllablesAt = table.Column<string>(type: "text", nullable: false),
                    DescribeSyllables = table.Column<string>(type: "text", nullable: false),
                    WordWithCorrectArticulationAt = table.Column<string>(type: "text", nullable: false),
                    DescribeWordArticulation = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KidId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidLanguageBackground", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidLanguageBackground_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidLanguageBackground_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidLanguageBackground_Kids_KidId",
                        column: x => x.KidId,
                        principalTable: "Kids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KidPsychomotorBackgroud",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AffirmTheHead = table.Column<string>(type: "text", nullable: false),
                    SatUp = table.Column<string>(type: "text", nullable: false),
                    CrawlAt = table.Column<string>(type: "text", nullable: false),
                    StandAt = table.Column<string>(type: "text", nullable: false),
                    WalkedTo = table.Column<string>(type: "text", nullable: false),
                    HaveAnyDifficultiesInMovements = table.Column<bool>(type: "boolean", nullable: false),
                    DescribeDifficultiesMovements = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KidId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidPsychomotorBackgroud", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidPsychomotorBackgroud_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidPsychomotorBackgroud_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidPsychomotorBackgroud_Kids_KidId",
                        column: x => x.KidId,
                        principalTable: "Kids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KidRelationBackground",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RelationWithParents = table.Column<string>(type: "text", nullable: false),
                    RelationWithMother = table.Column<string>(type: "text", nullable: false),
                    MotherPlays = table.Column<bool>(type: "boolean", nullable: false),
                    TimeOfDayPlayMother = table.Column<string>(type: "text", nullable: false),
                    RelationWithFather = table.Column<string>(type: "text", nullable: false),
                    TimeOfDayPlayFather = table.Column<string>(type: "text", nullable: false),
                    RelationWithSiblings = table.Column<string>(type: "text", nullable: false),
                    RelationParentsWithSiblings = table.Column<string>(type: "text", nullable: false),
                    RelationBetweenParentsAndChildren = table.Column<string>(type: "text", nullable: false),
                    RelationBetweenParentsAndGrandParents = table.Column<string>(type: "text", nullable: false),
                    MakeFriendsEasily = table.Column<bool>(type: "boolean", nullable: false),
                    PrefferOlderOrYounger = table.Column<string>(type: "text", nullable: false),
                    ShareWithOtherChildren = table.Column<bool>(type: "boolean", nullable: false),
                    GetAlongWellWithAdults = table.Column<bool>(type: "boolean", nullable: false),
                    IsCheerful = table.Column<bool>(type: "boolean", nullable: false),
                    IsAgresive = table.Column<bool>(type: "boolean", nullable: false),
                    HowShowBehavior = table.Column<string>(type: "text", nullable: false),
                    IsIndependent = table.Column<bool>(type: "boolean", nullable: false),
                    CriesForNoReason = table.Column<bool>(type: "boolean", nullable: false),
                    HowItReactsWhencontracted = table.Column<string>(type: "text", nullable: false),
                    TakeCareYourToys = table.Column<bool>(type: "boolean", nullable: false),
                    WhatKindOfToysYouLike = table.Column<string>(type: "text", nullable: false),
                    HowIsCorrect = table.Column<string>(type: "text", nullable: false),
                    WhenIsGoodWhatDoYouDo = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserCreation = table.Column<int>(type: "integer", nullable: false),
                    UserModification = table.Column<int>(type: "integer", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModification = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KidId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidRelationBackground", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KidRelationBackground_AspNetUsers_UserCreation",
                        column: x => x.UserCreation,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidRelationBackground_AspNetUsers_UserModification",
                        column: x => x.UserModification,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KidRelationBackground_Kids_KidId",
                        column: x => x.KidId,
                        principalTable: "Kids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KidBackground_KidId",
                table: "KidBackground",
                column: "KidId");

            migrationBuilder.CreateIndex(
                name: "IX_KidBackground_UserCreation",
                table: "KidBackground",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_KidBackground_UserModification",
                table: "KidBackground",
                column: "UserModification");

            migrationBuilder.CreateIndex(
                name: "IX_KidBirthBackground_KidId",
                table: "KidBirthBackground",
                column: "KidId");

            migrationBuilder.CreateIndex(
                name: "IX_KidBirthBackground_UserCreation",
                table: "KidBirthBackground",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_KidBirthBackground_UserModification",
                table: "KidBirthBackground",
                column: "UserModification");

            migrationBuilder.CreateIndex(
                name: "IX_KidConditionNewBorn_KidId",
                table: "KidConditionNewBorn",
                column: "KidId");

            migrationBuilder.CreateIndex(
                name: "IX_KidConditionNewBorn_UserCreation",
                table: "KidConditionNewBorn",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_KidConditionNewBorn_UserModification",
                table: "KidConditionNewBorn",
                column: "UserModification");

            migrationBuilder.CreateIndex(
                name: "IX_KidDreamBackground_KidId",
                table: "KidDreamBackground",
                column: "KidId");

            migrationBuilder.CreateIndex(
                name: "IX_KidDreamBackground_UserCreation",
                table: "KidDreamBackground",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_KidDreamBackground_UserModification",
                table: "KidDreamBackground",
                column: "UserModification");

            migrationBuilder.CreateIndex(
                name: "IX_KidFoodBackground_KidId",
                table: "KidFoodBackground",
                column: "KidId");

            migrationBuilder.CreateIndex(
                name: "IX_KidFoodBackground_UserCreation",
                table: "KidFoodBackground",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_KidFoodBackground_UserModification",
                table: "KidFoodBackground",
                column: "UserModification");

            migrationBuilder.CreateIndex(
                name: "IX_KidLanguageBackground_KidId",
                table: "KidLanguageBackground",
                column: "KidId");

            migrationBuilder.CreateIndex(
                name: "IX_KidLanguageBackground_UserCreation",
                table: "KidLanguageBackground",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_KidLanguageBackground_UserModification",
                table: "KidLanguageBackground",
                column: "UserModification");

            migrationBuilder.CreateIndex(
                name: "IX_KidPsychomotorBackgroud_KidId",
                table: "KidPsychomotorBackgroud",
                column: "KidId");

            migrationBuilder.CreateIndex(
                name: "IX_KidPsychomotorBackgroud_UserCreation",
                table: "KidPsychomotorBackgroud",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_KidPsychomotorBackgroud_UserModification",
                table: "KidPsychomotorBackgroud",
                column: "UserModification");

            migrationBuilder.CreateIndex(
                name: "IX_KidRelationBackground_KidId",
                table: "KidRelationBackground",
                column: "KidId");

            migrationBuilder.CreateIndex(
                name: "IX_KidRelationBackground_UserCreation",
                table: "KidRelationBackground",
                column: "UserCreation");

            migrationBuilder.CreateIndex(
                name: "IX_KidRelationBackground_UserModification",
                table: "KidRelationBackground",
                column: "UserModification");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KidBackground");

            migrationBuilder.DropTable(
                name: "KidBirthBackground");

            migrationBuilder.DropTable(
                name: "KidConditionNewBorn");

            migrationBuilder.DropTable(
                name: "KidDreamBackground");

            migrationBuilder.DropTable(
                name: "KidFoodBackground");

            migrationBuilder.DropTable(
                name: "KidLanguageBackground");

            migrationBuilder.DropTable(
                name: "KidPsychomotorBackgroud");

            migrationBuilder.DropTable(
                name: "KidRelationBackground");
        }
    }
}
