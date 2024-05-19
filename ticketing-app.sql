CREATE TABLE App_User (
    Name VARCHAR(70) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Password VARCHAR(70) NOT NULL,
    User_type VARCHAR(15) NOT NULL,
    CONSTRAINT User_Credentials_PK PRIMARY KEY (Email)
);

INSERT INTO App_User (name, email, password, User_type) VALUES ('admin', 'admin@gmail.com', '1234', 'admin');

CREATE TABLE Bus (
    Bus_id INT IDENTITY(1,1),
    Bus_number VARCHAR(10) NOT NULL,
    Arrival_location VARCHAR(70) NOT NULL,
    Departure_location VARCHAR(70) NOT NULL,
    Date_time DATETIME,
    Seat_price INT NOT NULL,
    Total_seats INT NOT NULL
    CONSTRAINT Bus_Credentials_PK PRIMARY KEY (Bus_id)
);

CREATE TABLE Seat_t (
    Seat_no INT NOT NULL,
    isBooked BIT NOT NULL DEFAULT 0, 
    Booking_time DATETIME,
    Passenger_email VARCHAR(100),
    Bus_id INT NOT NULL,
    CONSTRAINT Seat_Credentials_PK PRIMARY KEY (Seat_no, Bus_id),
    CONSTRAINT Seat_Credentials_FK_1 FOREIGN KEY (Passenger_email) REFERENCES App_User(Email),
    CONSTRAINT Seat_Credentials_FK_2 FOREIGN KEY (Bus_id) REFERENCES Bus(Bus_id)
    ON DELETE CASCADE
);

--trigger for inserts in seat table after insert in BUS
CREATE TRIGGER InsertSeatsOnBusInsert
ON Bus
AFTER INSERT
AS
BEGIN
    -- Declare variables
    DECLARE @BusNumber VARCHAR(10)
    DECLARE @TotalSeats INT
    DECLARE @SeatNo INT = 1
    
    -- Get the inserted bus number and total seats
    SELECT @BusNumber = Bus_number, @TotalSeats = Total_seats FROM inserted
    
    -- Insert seats into Seat_t table
    WHILE @SeatNo <= @TotalSeats
    BEGIN
        INSERT INTO Seat_t (Seat_no, Bus_number)
        VALUES (@SeatNo, @BusNumber)
        
        SET @SeatNo = @SeatNo + 1
    END
END;




