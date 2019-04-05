CREATE SCHEMA eHotel;

CREATE TABLE eHotel.person (
	ssn INT PRIMARY KEY,
	full_name VARCHAR(50) NOT NULL,
	street_number INT NOT NULL,
	street_name VARCHAR(50) NOT NULL,
	apt_number INT,
	city VARCHAR(50) NOT NULL,
	p_state VARCHAR(50) NOT NULL,
	zip VARCHAR(6) NOT NULL
);

CREATE TABLE eHotel.employee (
	ssn INT PRIMARY KEY,
	date_of_employment DATE NOT NULL,
	username VARCHAR(50) NOT NULL,
	password VARCHAR(50) NOT NULL,
	CONSTRAINT employee_person_fkey FOREIGN KEY (ssn)
      REFERENCES eHotel.person (ssn) MATCH SIMPLE
	  ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE eHotel.customer (
	ssn INT PRIMARY KEY,
	register_date DATE NOT NULL,
	username VARCHAR(50) NOT NULL,
	password VARCHAR(50) NOT NULL,
	CONSTRAINT customer_person_fkey FOREIGN KEY (ssn)
      REFERENCES eHotel.person (ssn) MATCH SIMPLE
	  ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE eHotel.hotelChain (
	hcID serial PRIMARY KEY,
	hotel_chain_name VARCHAR(50) NOT NULL,
	street_number INT NOT NULL,
	street_name VARCHAR(50) NOT NULL,
	apt_number INT,
	city VARCHAR(50) NOT NULL,
	hc_state VARCHAR(50) NOT NULL,
	zip VARCHAR(6) NOT NULL,
	num_hotels INT NOT NULL DEFAULT 0
);

CREATE TABLE eHotel.hotel (
	hID serial PRIMARY KEY,
	hcID INT NOT NULL,
	hotel_name VARCHAR(50) NOT NULL,
	manager INT NOT NULL,
	category INT NOT NULL,
	num_rooms INT NOT NULL DEFAULT 0,
	street_number INT NOT NULL,
	street_name VARCHAR(50) NOT NULL,
	apt_number INT,
	city VARCHAR(50) NOT NULL,
	h_state VARCHAR(50) NOT NULL,
	zip VARCHAR(6) NOT NULL,
	email VARCHAR(355) NOT NULL,
	
	CHECK (category BETWEEN 1 AND 5),
	CONSTRAINT hotel_hotelChain_fkey FOREIGN KEY (hcID)
      REFERENCES eHotel.hotelChain (hcID) MATCH SIMPLE
		ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT hotel_employee_fkey FOREIGN KEY (manager)
      REFERENCES eHotel.employee (ssn) MATCH SIMPLE
);

CREATE TABLE eHotel.room (
	rID serial PRIMARY KEY,
	room_num INT NOT NULL,
	hID INT NOT NULL,
	price NUMERIC(8,2) NOT NULL,
	capacity SMALLINT NOT NULL,
	isExtandable BOOLEAN NOT NULL,
	landscape INT NOT NULL,	
	
	CONSTRAINT room_hotel_fkey FOREIGN KEY (hID)
      REFERENCES eHotel.hotel (hID) MATCH SIMPLE
	  ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE eHotel.hotelChainPhone(
	phone_number VARCHAR(20) NOT NULL,
	hcID INT NOT NULL,
	PRIMARY KEY (phone_number, hcID),
	CONSTRAINT phone_hotelChain_fkey FOREIGN KEY (hcID)
      REFERENCES eHotel.hotelChain (hcID) MATCH SIMPLE
	  ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE eHotel.hotelChainEmail(
	email VARCHAR(355) NOT NULL,
	hcID INT NOT NULL,
	PRIMARY KEY (email, hcID),
	CONSTRAINT email_hotelChain_fkey FOREIGN KEY (hcID)
      REFERENCES eHotel.hotelChain (hcID) MATCH SIMPLE
	  ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE eHotel.hotelPhone(
	phone_number VARCHAR(20) NOT NULL,
	hID INT NOT NULL,
	PRIMARY KEY (phone_number, hID),
	CONSTRAINT phone_hotel_fkey FOREIGN KEY (hID)
      REFERENCES eHotel.hotel (hID) MATCH SIMPLE
	  ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE eHotel.amenity(
	aID serial PRIMARY KEY,
	amenity TEXT NOT NULL,
	rID INT NOT NULL,
	CONSTRAINT amenity_room_fkey FOREIGN KEY (rID)
      REFERENCES eHotel.room (rID) MATCH SIMPLE
	  ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE eHotel.damage(
	dID serial PRIMARY KEY,
	damage TEXT NOT NULL,
	rID INT NOT NULL,
	CONSTRAINT damage_room_fkey FOREIGN KEY (rID)
      REFERENCES eHotel.room (rID) MATCH SIMPLE
	  ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE eHotel.role(
	role VARCHAR(100) NOT NULL,
	employee_ssn INT NOT NULL,
	PRIMARY KEY (role, employee_ssn),
	CONSTRAINT role_employee_fkey FOREIGN KEY (employee_ssn)
      REFERENCES eHotel.employee (ssn) MATCH SIMPLE
	  ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE  eHotel.booking(
	bID serial PRIMARY KEY,
	rID INT NOT NULL,
	customer_ssn INT NOT NULL,
	start_date TIMESTAMP NOT NULL,
	end_date TIMESTAMP NOT NULL,
	CONSTRAINT booking_room_fkey FOREIGN KEY (rID)
      REFERENCES eHotel.room (rID) MATCH SIMPLE
	  ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT booking_customer_fkey FOREIGN KEY (customer_ssn)
      REFERENCES eHotel.customer (ssn) MATCH SIMPLE
	  ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE  eHotel.renting(
	rentID serial PRIMARY KEY,
	rID INT NOT NULL,
	customer_ssn INT NOT NULL,
	employee_ssn INT NOT NULL,
	start_date TIMESTAMP NOT NULL,
	end_date TIMESTAMP NOT NULL,
	CONSTRAINT booking_room_fkey FOREIGN KEY (rID)
      REFERENCES eHotel.room (rID) MATCH SIMPLE
	  ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT booking_customer_fkey FOREIGN KEY (customer_ssn)
      REFERENCES eHotel.customer (ssn) MATCH SIMPLE
	  ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT booking_employee_fkey FOREIGN KEY (employee_ssn)
      REFERENCES eHotel.employee (ssn) MATCH SIMPLE
);

CREATE TABLE eHotel.bookingArc(
	baID serial PRIMARY KEY,
	rID INT NOT NULL,
	customer_ssn INT NOT NULL,
	start_date TIMESTAMP NOT NULL,
	end_date TIMESTAMP NOT NULL
);

CREATE TABLE eHotel.rentingArc(
	rentaID serial PRIMARY KEY,
	rID INT NOT NULL,
	customer_ssn INT NOT NULL,
	employee_ssn INT NOT NULL,
	start_date TIMESTAMP NOT NULL,
	end_date TIMESTAMP NOT NULL
);	

CREATE OR REPLACE FUNCTION eHotel.increment_num_rooms()
  RETURNS trigger AS
	$BODY$
	BEGIN
		UPDATE eHotel.hotel
		SET num_rooms = num_rooms + 1
		WHERE NEW.hID = hotel.hID;
		RETURN NEW;
	END;
	$BODY$ LANGUAGE plpgsql;

CREATE TRIGGER num_rooms_inc AFTER INSERT
ON eHotel.room
FOR EACH ROW
EXECUTE PROCEDURE eHotel.increment_num_rooms ();
	
CREATE OR REPLACE FUNCTION eHotel.decrement_num_rooms()
  RETURNS trigger AS
	$BODY$
	BEGIN
		UPDATE eHotel.hotel
		SET num_rooms = num_rooms - 1
		WHERE OLD.hID = hotel.hID;
		RETURN OLD;
	END;
	$BODY$ LANGUAGE plpgsql;

CREATE TRIGGER num_rooms_dec AFTER DELETE
ON eHotel.room
FOR EACH ROW
EXECUTE PROCEDURE eHotel.decrement_num_rooms ();
	
CREATE OR REPLACE FUNCTION eHotel.increment_num_hotels()
  RETURNS trigger AS
	$BODY$
	BEGIN
		UPDATE eHotel.hotelChain
		SET num_hotels = num_hotels + 1
		WHERE NEW.hcID = hotelChain.hcID;
		RETURN NEW;
	END;
	$BODY$ LANGUAGE plpgsql;

CREATE TRIGGER num_hotels_inc AFTER INSERT
ON eHotel.hotel
FOR EACH ROW
EXECUTE PROCEDURE eHotel.increment_num_hotels ();
	
CREATE OR REPLACE FUNCTION eHotel.decrement_num_hotels()
  RETURNS trigger AS
	$BODY$
	BEGIN
		UPDATE eHotel.hotelChain
		SET num_hotels = num_hotels - 1
		WHERE OLD.hcID = hotelChain.hcID;
		RETURN OLD;
	END;
	$BODY$ LANGUAGE plpgsql;

CREATE TRIGGER num_hotels_dec AFTER DELETE
ON eHotel.hotel
FOR EACH ROW
EXECUTE PROCEDURE eHotel.decrement_num_hotels ();