DROP TABLE IF EXISTS Sensor_Data;
DROP TABLE IF EXISTS Sensor;
DROP TABLE IF EXISTS Access_Log;
DROP TABLE IF EXISTS Authorisation;
DROP TABLE IF EXISTS Access_Point;
DROP TABLE IF EXISTS Facility;
DROP TABLE IF EXISTS Smiley_User;

CREATE TABLE Smiley_User (
    user_id	VARCHAR(20)    	PRIMARY KEY,
    full_name   VARCHAR(80)     NOT NULL,
    email	VARCHAR (50)   	NOT NULL,
    password 	VARBINARY (50) 	NOT NULL,
    voicefile	VARCHAR(50)    	NOT NULL,
    picfile	VARCHAR(50)    	NOT NULL,
    role     	VARCHAR(50)    	NOT NULL,
    phone_num	VARCHAR(20)    	NULL,
    last_login 	DATETIME      	NULL
);
GO

INSERT INTO Smiley_User (user_id, full_name, email, password, voicefile, picfile, role) VALUES 
('jonlee','jonlee tan', 'jonlee@email.com', HASHBYTES('SHA1','password'), 'voice1', 'default', 'User'),
('felkang', 'felkang lee','felkang@email.com', HASHBYTES('SHA1','password'), 'voice1', 'default', 'User'),
('dorta','dorta lim', 'dorta@email.com', HASHBYTES('SHA1','password'), 'voice1', 'default', 'User'),
('deslee', 'deslee loh','deslee@email.com', HASHBYTES('SHA1','password'), 'voice1', 'default', 'User'),
('jiamei', 'zeng jiamei','Jiameizeng@gmail.com', HASHBYTES('SHA1','password'), 'voice2', 'default', 'Admin'),
('jinhan', 'ke jinhan','kejinhan6@gmail.com', HASHBYTES('SHA1','password'), 'voice2', 'default', 'BuildingAdmin'),
('angeline', 'mok siew theng','angelinemok123@gmail.com', HASHBYTES('SHA1','password'), 'voice3', 'default', 'BuildingAdmin')
GO

CREATE TABLE Facility(
    facility_id		INT IDENTITY (1, 1) PRIMARY KEY,
    admin_id		VARCHAR(20)    	NOT NULL,
    facility_name 	VARCHAR(50)    	NOT NULL,
    postal_code		INT	    	NOT NULL,
    block_number	VARCHAR(10)    	NOT NULL,
    street_name         VARCHAR(50)    	NOT NULL,
    banner_pic		VARCHAR(50)    	NULL,
    CONSTRAINT fk1 FOREIGN KEY(admin_id) REFERENCES Smiley_User(user_id)
);

SET IDENTITY_INSERT Facility ON 
INSERT INTO Facility (facility_id, admin_id, facility_name, postal_code, block_number, street_name) VALUES 
(1, 'jiamei', 'Trellis Towers', 319773, '700', 'Lor 1 Toa Payoh'),
(2, 'jiamei', 'Vista Residences', 329425, '26', 'Jln Datoh'),
(3, 'jinhan', 'Gaia Residence', 329371, '33', 'Jln Dusun')
SET IDENTITY_INSERT Facility OFF


CREATE TABLE Access_Point(
    access_point_id	INT IDENTITY (1, 1) PRIMARY KEY,
    facility_id		INT	    	NOT NULL,
    description         VARCHAR(50)    	NOT NULL,
    CONSTRAINT fk2 FOREIGN KEY(facility_id) REFERENCES Facility(facility_id)
)

SET IDENTITY_INSERT Access_Point ON 
INSERT INTO Access_Point (access_point_id, facility_id, description) VALUES 
(1, 1, 'Entrance'),
(2, 1, 'Gym'),
(3, 1, 'Lift'),
(4, 1, '02-03 Entrance'),
(5, 1, '02-06 Entrance'),
(6, 2, 'Entrance'),
(7, 2, 'Gym'),
(8, 2, 'Lift'),
(9, 2, '03-05 Entrance'),
(10, 2, '03-08 Entrance'),
(11, 3, 'Entrance'),
(12, 3, 'Gym'),
(13, 3, 'Lift'),
(14, 3, '05-07 Entrance'),
(15, 3, '05-02 Entrance')
SET IDENTITY_INSERT Access_Point OFF

CREATE TABLE Authorisation (
    start_date                  DATETIME    	NOT NULL, 
    end_date                    DATETIME    	NOT NULL, 
    access_point_id		INT	    	NOT NULL,
    user_id 		        VARCHAR(20)    	NOT NULL,
    CONSTRAINT fk3 FOREIGN KEY(access_point_id) REFERENCES Access_Point(access_point_id),
    CONSTRAINT fk4 FOREIGN KEY(user_id) REFERENCES Smiley_User(user_id),
    CONSTRAINT PK_Authorisation PRIMARY KEY(access_point_id,user_id)
);

--SET IDENTITY_INSERT Authorisation ON
INSERT INTO Authorisation (start_date, end_date, access_point_id, user_id) VALUES 
('2021-01-01 00:00:00','2021-05-05 23:59:59',1,'jonlee'),
('2021-02-02 00:00:00','2021-02-05 23:59:59',2,'jonlee'),
('2021-03-03 00:00:00','2021-03-05 23:59:59',3,'jonlee'),
('2021-03-05 00:00:00','2021-03-08 23:59:59',4,'jonlee'),
('2021-03-07 00:00:00','2021-04-05 23:59:59',5,'felkang'),
('2021-04-01 00:00:00','2022-04-05 23:59:59',6,'felkang'),
('2021-04-02 00:00:00','2022-04-06 23:59:59',7,'felkang'),
('2021-04-28 00:00:00','2022-04-30 23:59:59',8,'felkang'),
('2021-04-29 00:00:00','2022-05-05 23:59:59',9,'dorta'),
('2021-05-01 00:00:00','2022-05-07 23:59:59',10,'dorta'),
('2021-05-02 00:00:00','2022-05-10 23:59:59',11,'dorta'),
('2021-05-06 00:00:00','2022-05-11 23:59:59',12,'dorta'),
('2021-06-01 00:00:00','2023-06-05 23:59:59',13,'deslee'),
('2021-06-02 00:00:00','2023-06-06 23:59:59',14,'deslee'),
('2021-06-05 00:00:00','2023-06-22 23:59:59',15,'deslee');
--SET IDENTITY_INSERT Authorisation OFF


CREATE TABLE Access_Log (
    sno			       INT	        IDENTITY (1, 1) PRIMARY KEY, 
    user_id                    VARCHAR(20)    	NOT NULL,
    direction                  VARCHAR(20)    	NOT NULL, -- IN or OUT --
    time_stamp		       DATETIME    	NOT NULL, 
    CONSTRAINT fk5 FOREIGN KEY(user_id) REFERENCES Smiley_User(user_id)
);

SET IDENTITY_INSERT Access_Log ON;
INSERT INTO Access_Log (sno, user_id, direction, time_stamp) VALUES
(1, 'jonlee', 'In', '2020-01-21 11:00:00'),
(2, 'jonlee', 'In', '2020-01-21 11:10:11'),
(3, 'jonlee', 'In', '2020-01-21 11:12:12'),
(4, 'jonlee', 'Out', '2020-01-21 12:13:01'),
(5, 'felkang', 'In', '2020-03-03 12:05:07'),
(6, 'felkang', 'In', '2020-03-03 13:00:01'),
(7, 'felkang', 'In', '2020-03-03 14:00:01'),
(8, 'dorta', 'In', '2020-03-03 14:05:07'),
(9, 'dorta', 'Out', '2020-04-28 15:11:08'),
(10, 'dorta', 'In', '2020-04-28 15:14:07'),
(11, 'dorta', 'In', '2020-04-28 12:05:07'),
(12, 'dorta', 'Out', '2021-01-01 12:05:07'),
(13, 'deslee', 'Out', '2021-01-21 13:11:05'),
(14, 'deslee', 'In', '2021-01-21 13:10:06'),
(15, 'deslee', 'In', '2021-01-21 14:05:02'),
(16, 'deslee', 'Out', '2021-01-28 14:05:01'),
(17, 'deslee', 'Out', '2021-01-28 15:20:11'),
(18, 'deslee', 'Out', '2021-01-28 15:20:50'),
(19, 'dorta', 'In', '2021-01-28 16:30:40'),
(20, 'felkang', 'Out', '2021-01-28 16:30:33');
SET IDENTITY_INSERT Access_Log OFF;

--annoymously detect and store gesture--
CREATE TABLE Sensor ( 
    sensor_id		INT	IDENTITY (1, 1) PRIMARY KEY,
    access_point_id	INT	    	NOT NULL,
    sensor_name		VARCHAR(50)    	NOT NULL,
    CONSTRAINT fk6 FOREIGN KEY(access_point_id) REFERENCES Access_Point(access_point_id)
);

SET IDENTITY_INSERT Sensor ON 
INSERT INTO Sensor (sensor_id, access_point_id, sensor_name) VALUES 
(1, 1, 'Entrance Sensor'),
(2, 2, 'Gym Sensor'),
(3, 3, 'Lift Sensor'),
(4, 4, 'Toilet Sensor'),
(5, 5, 'Pool Sensor'),
(6, 6, 'Entrance Sensor'),
(7, 7, 'Gym Sensor'),
(8, 8, 'Lift Sensor'),
(9, 9, 'Toilet Sensor'),
(10, 10, 'Pool Sensor'),
(11, 11, 'Entrance Sensor'),
(12, 12, 'Gym Sensor'),
(13, 13, 'Lift Sensor'),
(14, 14, 'Toilet Sensor'),
(15, 15, 'Pool Sensor')
SET IDENTITY_INSERT Sensor OFF

CREATE TABLE Sensor_Data (
    sno	                INT	IDENTITY (1, 1) PRIMARY KEY, --serial number--
    sensor_id		INT	    	NOT NULL,
    feedback_gesture	VARCHAR(30)    	NOT NULL,
    time_stamp		DATETIME    	NOT NULL,
    CONSTRAINT fk7 FOREIGN KEY(sensor_id) REFERENCES Sensor(sensor_id)
);

SET IDENTITY_INSERT Sensor_Data ON;
INSERT INTO Sensor_Data (sno, sensor_id, feedback_gesture, time_stamp) VALUES
(1, 1, 'anger', '2020-01-21 09:00:01'),
(2, 3, 'anger', '2020-01-21 09:10:01'),
(3, 5, 'anticipation', '2020-01-21 09:11:01'),
(4, 2, 'anticipation', '2020-01-21 09:12:01'),
(5, 4, 'joy', '2020-01-21 10:01:21'),
(6, 6, 'joy', '2020-01-21 11:00:01'),
(7, 8, 'joy', '2020-01-21 11:22:01'),
(8, 10, 'trust', '2020-01-21 12:23:01'),
(9, 7, 'trust', '2020-01-21 13:11:01'),
(10, 9, 'trust', '2020-01-21 13:11:01'),
(11, 12, 'fear', '2020-01-21 14:12:01'),
(12, 14, 'fear', '2020-01-21 14:14:01'),
(13, 11, 'fear', '2020-01-21 15:11:01'),
(14, 13, 'surprise', '2020-01-21 15:12:01'),
(15, 15, 'surprise', '2020-01-21 16:13:01'),
(16, 7, 'surprise', '2020-01-21 16:14:01'),
(17, 5, 'sadness', '2020-01-21 17:11:01'),
(18, 15, 'disgust', '2020-01-21 17:11:01'),
(19, 11, 'digust', '2020-01-21 16:11:01'),
(20, 5, 'sadness', '2020-01-21 16:11:01');
SET IDENTITY_INSERT Sensor_Data OFF;



