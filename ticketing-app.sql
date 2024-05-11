CREATE TABLE Passenger (
    Name VARCHAR(70) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Password VARCHAR(70) NOT NULL,
    CONSTRAINT Passenger_Credentials_PK PRIMARY KEY (Email)
);

CREATE TABLE Bus (
    Bus_number VARCHAR(10) NOT NULL,
    From VARCHAR(70) NOT NULL,
    To VARCHAR(70) NOT NULL,
    Date_time DATETIME,
    Seat_price INT NOT NULL,
    Total_seats INT NOT NULL
    CONSTRAINT Bus_Credentials_PK PRIMARY KEY (Bus_number)
);
CREATE TABLE Seats (
    Seat_no INT NOT NULL,
    isBooked BIT NOT NULL DEFAULT 0, 
    Booking_time DATETIME NOT NULL,
    Passenger_email VARCHAR(100) NOT NULL,
    Bus_number VARCHAR(10) NOT NULL,
    CONSTRAINT Seat_Credentials_PK PRIMARY KEY (Seat_no),
    CONSTRAINT Seat_Credentials_FK_1 FOREIGN KEY (Passenger_email) REFERENCES Passenger(Email),
    CONSTRAINT Seat_Credentials_FK_2 FOREIGN KEY (Bus_number) REFERENCES Bus(Bus_number)
);


