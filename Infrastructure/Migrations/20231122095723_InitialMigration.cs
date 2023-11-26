using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Molo.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InterestRate",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false),
                    Percentage = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestRate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriber",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Msisdn = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Pin = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    SubscriptionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DateInactive = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionType",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriberId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Msisdn = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_Subscriber_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Subscriber",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriberId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriberClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CollectionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsSettled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    SettlementDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    InterestRateId = table.Column<byte>(type: "smallint", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loan_Client_SubscriberClientId",
                        column: x => x.SubscriberClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loan_InterestRate_InterestRateId",
                        column: x => x.InterestRateId,
                        principalTable: "InterestRate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loan_Subscriber_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Subscriber",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriberId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriberClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriberLoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionTypeId = table.Column<byte>(type: "smallint", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Client_SubscriberClientId",
                        column: x => x.SubscriberClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_Loan_SubscriberLoanId",
                        column: x => x.SubscriberLoanId,
                        principalTable: "Loan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_Subscriber_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Subscriber",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_TransactionType_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "TransactionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "InterestRate",
                columns: new[] { "Id", "Description", "Percentage" },
                values: new object[,]
                {
                    { (byte)1, "5 Percent", 5m },
                    { (byte)2, "10 Percent", 10m },
                    { (byte)3, "15 Percent", 15m },
                    { (byte)4, "20 Percent", 20m },
                    { (byte)5, "25 Percent", 25m },
                    { (byte)6, "30 Percent", 30m }
                });

            migrationBuilder.InsertData(
                table: "TransactionType",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { (byte)1, "Credit" },
                    { (byte)2, "Debit" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_SubscriberId",
                table: "Client",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_InterestRateId",
                table: "Loan",
                column: "InterestRateId");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_SubscriberClientId",
                table: "Loan",
                column: "SubscriberClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_SubscriberId",
                table: "Loan",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_SubscriberClientId",
                table: "Transaction",
                column: "SubscriberClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_SubscriberId",
                table: "Transaction",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_SubscriberLoanId",
                table: "Transaction",
                column: "SubscriberLoanId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionTypeId",
                table: "Transaction",
                column: "TransactionTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Loan");

            migrationBuilder.DropTable(
                name: "TransactionType");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "InterestRate");

            migrationBuilder.DropTable(
                name: "Subscriber");
        }
    }
}
