

CREATE DOMAIN role_dom AS VARCHAR CHECK (VALUE IN ('WarehouseWorker', 'DispatchManager'));
CREATE DOMAIN status_dom AS VARCHAR CHECK (VALUE IN ('Registered', 'Dispatched', 'Delayed'));

CREATE TABLE Van (
    vanId CHAR(6) PRIMARY KEY,
    destination VARCHAR(100) NOT NULL,
    capacity INT
);

CREATE TABLE "User" (
    "userId" SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    username VARCHAR(50) NOT NULL,
    password VARCHAR(100) NOT NULL,
    role role_dom NOT NULL
);

CREATE TABLE WarehouseWorker (
    id INT PRIMARY KEY,
    FOREIGN KEY (id) references "User"("userId")
);

CREATE TABLE DispatchManager (
    id INT PRIMARY KEY,
    FOREIGN KEY (id) references "User"("userId")
);

CREATE TABLE Parcel (
    parcelId SERIAL PRIMARY KEY,
    volume DECIMAL(6,2),
    weight DECIMAL(6,2),
    destination VARCHAR(100) NOT NULL,
    status status_dom NOT NULL,
    time_registered TIME,
    vanId VARCHAR(6) NOT NULL,
    registeredByUserId INT,
    FOREIGN KEY (vanId) references Van(vanId),
    FOREIGN KEY (registeredByUserId) references "User"("userId")
);


INSERT INTO Van (vanId, destination, capacity) VALUES
('AB1234', 'Copenhagen', 10),
('CD5678', 'Aarhus', 5),
('EF1111', 'Horsens', 3),
('GH2222', 'Viborg', 3),
('IJ3333', 'Vejle', 4),
('KL4444', 'Aalborg', 6),
('MN5555', 'Odense', 4);

INSERT INTO "User" (name, username, password, role) VALUES
('Henrik', 'WWHenrik', 'password1', 'WarehouseWorker'),
('Jens', 'DMJens', 'password2', 'DispatchManager');

INSERT INTO Parcel (volume, weight, destination, status, time_registered, vanId, registeredByUserId) VALUES
(10.5, 1.6, 'Aarhus', 'Registered', '10:40', 'CD5678', 1),
(20.5, 2, 'Copenhagen', 'Registered', '06:20', 'AB1234', 1);

INSERT INTO WarehouseWorker VALUES (1);
INSERT INTO DispatchManager VALUES (2);
