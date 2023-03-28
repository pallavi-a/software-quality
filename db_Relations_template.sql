DROP TABLE Befriend;
DROP TABLE Favor;
DROP TABLE Review;
DROP TABLE Hours;
DROP TABLE Category;
DROP TABLE Checkin;
DROP TABLE Person;
DROP TABLE Business;
DROP TABLE Use;


CREATE TABLE Use (
	username VARCHAR (40),
	password VARCHAR(200),
	email VARCHAR(40)
);

#IF exists(select * from Use where username='yo') THEN insert into Use (username, password, email) values ('hey','cat','bug') END IF

#insert into Use (username, password, email) select 'game','yo','yo' where not exists(select 1 from Use where username = 'game');

CREATE TABLE Person (
	personID VARCHAR (40),
	numVotes INTEGER,
	numFans INTEGER,
	joinDate VARCHAR (10),
	name VARCHAR(40),
	staravg FLOAT,
	latitude FLOAT,
	longitude FLOAT,
	PRIMARY KEY(personID)
);


CREATE TABLE Befriend(
	requestFrom VARCHAR (40),
	requestTo VARCHAR (40),
	PRIMARY KEY(requestFrom, requestTo),
	FOREIGN KEY(requestFrom) REFERENCES Person(personID),
	FOREIGN KEY(requestTo) REFERENCES Person(personID)
);


--All businessID values must be strings of at least length 22
CREATE TABLE Business(
	businessID VARCHAR (22),
	stars FLOAT DEFAULT 0.0,
	businessRating FLOAT,
	city VARCHAR (20),
	zipcode VARCHAR(20),
	state VARCHAR (20),
	address VARCHAR (80),
	businessName VARCHAR (100),
	numCheckIns INTEGER DEFAULT 0,
	latitude FLOAT,
	longitude FLOAT,
	ReviewCount INTEGER DEFAULT 0,
	PRIMARY KEY(businessID)
);

CREATE TABLE Favor(
	personID VARCHAR (40),
	businessID VARCHAR (22),
	PRIMARY KEY(personID, businessID),
	FOREIGN KEY (personID) REFERENCES Person(personID),
	FOREIGN KEY (businessID) REFERENCES Business(businessID)


);


CREATE TABLE Review(
	reviewID VARCHAR (40),
	createDate VARCHAR (10),
	text VARCHAR(2000), 
	rating FLOAT, 
	personID VARCHAR (40),
	businessID VARCHAR (22),
	PRIMARY KEY(reviewID),
	FOREIGN KEY(personID) REFERENCES Person(personID),
	FOREIGN KEY(businessID) REFERENCES Business(businessID)
);

CREATE TABLE Hours(
    businessID VARCHAR (22),
	day VARCHAR(10),
	open VARCHAR (10),
	close VARCHAR (10),
	PRIMARY KEY(day,businessID),
	FOREIGN KEY(businessID) REFERENCES Business(businessID)
);

CREATE TABLE Category(
	businessID VARCHAR (22),
	categoryname VARCHAR(50),
	PRIMARY KEY(categoryname,businessID),
	FOREIGN KEY(businessID) REFERENCES Business(businessID)
);

CREATE TABLE Checkin(
	businessID VARCHAR (22), 
	day VARCHAR(10),
	time VARCHAR (10),
	numpeople INTEGER,
	PRIMARY KEY(businessID,time,day),
	FOREIGN KEY(businessID) REFERENCES Business(businessID)
);
