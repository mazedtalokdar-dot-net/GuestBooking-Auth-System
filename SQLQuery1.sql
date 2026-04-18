USE HotelBookingDB_01
go
CREATE PROCEDURE spRoom_Insert
    @RoomNo NVARCHAR(20),
    @Capacity INT,
    @PricePerNight MONEY,
    @RoomTypeId INT,
    @RoomCategoryId INT
AS
BEGIN
    INSERT INTO Rooms (RoomNo, Capacity, PricePerNight, IsActive, RoomTypeId, RoomCategoryId)
    VALUES (@RoomNo, @Capacity, @PricePerNight, 1, @RoomTypeId, @RoomCategoryId)
END
GO
CREATE PROCEDURE spRoom_Update
    @RoomId INT,
    @RoomNo NVARCHAR(20),
    @Capacity INT,
    @PricePerNight MONEY,
    @IsActive BIT,
    @RoomTypeId INT,
    @RoomCategoryId INT
AS
BEGIN
    UPDATE Rooms
    SET RoomNo = @RoomNo,
        Capacity = @Capacity,
        PricePerNight = @PricePerNight,
        IsActive = @IsActive,
        RoomTypeId = @RoomTypeId,
        RoomCategoryId = @RoomCategoryId
    WHERE RoomId = @RoomId
END
GO
CREATE PROCEDURE spRoom_Delete
    @RoomId INT
AS
BEGIN
    DELETE FROM Rooms WHERE RoomId = @RoomId
END
GO
CREATE PROCEDURE spRoom_List
AS
BEGIN
    SELECT * FROM Rooms
END
GO
CREATE OR ALTER PROCEDURE spGuest_Insert
    @FullName    NVARCHAR(60),
    @Phone       NVARCHAR(20),
    @Email       NVARCHAR(80),
    @Address     NVARCHAR(200),
    @PhotoPath   NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Guests (FullName, Phone, Email, Address, PhotoPath)
    VALUES (@FullName, @Phone, @Email, @Address, @PhotoPath);
END
GO

Go
CREATE OR ALTER PROCEDURE spGuest_Update
    @GuestId INT,
    @FullName NVARCHAR(60),
    @Phone NVARCHAR(20),
    @Email NVARCHAR(80),
    @Address NVARCHAR(200),
    @PhotoPath   NVARCHAR(200)
AS
BEGIN
    UPDATE Guests
    SET FullName = @FullName,
        Phone = @Phone,
        Email = @Email,
        Address = @Address,
         PhotoPath = @PhotoPath
    WHERE GuestId = @GuestId
END
GO
CREATE PROCEDURE spGuest_Delete
    @GuestId INT
AS
BEGIN
    DELETE FROM Guests WHERE GuestId = @GuestId
END
GO
CREATE PROCEDURE spGuest_List
AS
BEGIN
    SELECT * FROM Guests
END
GO



CREATE PROCEDURE spBookedRoom_Insert
    @BookingId INT,
    @RoomId INT,
    @PricePerNight MONEY,
    @Nights INT
AS
BEGIN
    INSERT INTO BookedRooms (BookingId, RoomId, PricePerNight, Nights)
    VALUES (@BookingId, @RoomId, @PricePerNight, @Nights)
END
GO
CREATE OR ALTER PROCEDURE spGuest_Update
    @GuestId     INT,
    @FullName    NVARCHAR(60),
    @Phone       NVARCHAR(20),
    @Email       NVARCHAR(80),
    @Address     NVARCHAR(200),
    @PhotoPath   NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Guests
    SET FullName = @FullName,
        Phone = @Phone,
        Email = @Email,
        Address = @Address,
        PhotoPath = @PhotoPath
    WHERE GuestId = @GuestId;
END
Go
CREATE OR ALTER PROCEDURE spBooking_Update
    @BookingId    INT,
    @GuestId      INT,
    @CheckInDate  DATE,
    @CheckOutDate DATE,
    @Notes        NVARCHAR(200),
    @Status       INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Bookings
    SET GuestId = @GuestId,
        CheckInDate = @CheckInDate,
        CheckOutDate = @CheckOutDate,
        Notes = @Notes,
        Status = @Status
    WHERE BookingId = @BookingId;
END
Go
CREATE OR ALTER PROCEDURE spBookedRoom_DeleteByBookingId
    @BookingId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM BookedRooms WHERE BookingId = @BookingId;
END
GO
CREATE OR ALTER PROCEDURE spBookedRoom_DeleteByBookingId
    @BookingId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM BookedRooms WHERE BookingId = @BookingId;
END
GO

CREATE OR ALTER PROCEDURE spBooking_Delete
    @BookingId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Bookings WHERE BookingId = @BookingId;
END
