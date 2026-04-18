namespace _1291163_MVC_EF_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialHotelBooking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookedRooms",
                c => new
                    {
                        BookedRoomId = c.Int(nullable: false, identity: true),
                        BookingId = c.Int(nullable: false),
                        RoomId = c.Int(nullable: false),
                        PricePerNight = c.Decimal(nullable: false, storeType: "money"),
                        Nights = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookedRoomId)
                .ForeignKey("dbo.Bookings", t => t.BookingId, cascadeDelete: true)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .Index(t => t.BookingId)
                .Index(t => t.RoomId);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        CheckInDate = c.DateTime(nullable: false, storeType: "date"),
                        CheckOutDate = c.DateTime(nullable: false, storeType: "date"),
                        Status = c.Int(nullable: false),
                        Notes = c.String(maxLength: 200),
                        GuestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.Guests", t => t.GuestId, cascadeDelete: true)
                .Index(t => t.GuestId);
            
            CreateTable(
                "dbo.Guests",
                c => new
                    {
                        GuestId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 60),
                        Phone = c.String(maxLength: 20),
                        Email = c.String(maxLength: 80),
                        Address = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.GuestId);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomId = c.Int(nullable: false, identity: true),
                        RoomNo = c.String(nullable: false, maxLength: 20),
                        Capacity = c.Int(nullable: false),
                        PricePerNight = c.Decimal(nullable: false, storeType: "money"),
                        IsActive = c.Boolean(nullable: false),
                        RoomTypeId = c.Int(nullable: false),
                        RoomCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoomId)
                .ForeignKey("dbo.RoomCategories", t => t.RoomCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.RoomTypes", t => t.RoomTypeId, cascadeDelete: true)
                .Index(t => t.RoomTypeId)
                .Index(t => t.RoomCategoryId);
            
            CreateTable(
                "dbo.RoomCategories",
                c => new
                    {
                        RoomCategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.RoomCategoryId);
            
            CreateTable(
                "dbo.RoomTypes",
                c => new
                    {
                        RoomTypeId = c.Int(nullable: false, identity: true),
                        TypeName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.RoomTypeId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Rooms", "RoomTypeId", "dbo.RoomTypes");
            DropForeignKey("dbo.Rooms", "RoomCategoryId", "dbo.RoomCategories");
            DropForeignKey("dbo.BookedRooms", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.Bookings", "GuestId", "dbo.Guests");
            DropForeignKey("dbo.BookedRooms", "BookingId", "dbo.Bookings");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Rooms", new[] { "RoomCategoryId" });
            DropIndex("dbo.Rooms", new[] { "RoomTypeId" });
            DropIndex("dbo.Bookings", new[] { "GuestId" });
            DropIndex("dbo.BookedRooms", new[] { "RoomId" });
            DropIndex("dbo.BookedRooms", new[] { "BookingId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RoomTypes");
            DropTable("dbo.RoomCategories");
            DropTable("dbo.Rooms");
            DropTable("dbo.Guests");
            DropTable("dbo.Bookings");
            DropTable("dbo.BookedRooms");
        }
    }
}
