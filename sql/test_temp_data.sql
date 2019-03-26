--TEST Temp DATA

INSERT INTO ehotel.person
VALUES (123, 'Thomas Charette', 1,'test street',null,'Gatineau','QC','12345');

INSERT INTO ehotel.employee
VALUES (123, '2019-01-23', 'nopose', 'password');

INSERT INTO ehotel.hotelchain (hcid,hotel_chain_name,street_number,street_name,apt_number,city,hc_state,zip,num_hotels) 
VALUES (1,'Magna Hotels',3586, 'Netus Road',32,'Carson City','NV','87143',0);

INSERT INTO ehotel.hotel (hid,hcid,hotel_name,manager,category,num_rooms,street_number,street_name,apt_number,city,h_state,zip,email) 
VALUES (1,'1','Magna Lorem Ipsum LLC',123,3,0,4580, 'Eleifend. Road',31,'San Francisco','CA','92781','Duis@Nulla.net'),
(2,'1','Sollicitudin Inc.',123,2,0,4691,'Ac Road',25,'San Francisco','CA','18041','neque.et@auctor.edu'),
(3,'1','Cursus Luctus Ipsum Inc.',123,5,0,1540,'Vivamus Rd.',19,'Columbia','MD','44049','eget.metus@Nunc.org');

INSERT INTO ehotel.room (rid,room_num,hid,price,capacity,landscape,isextandable) 
VALUES (1,232,'1','873.43',4,'0','true'),(2,121,'1','685.01',4,'1','false'),(3,274,'1','746.83',3,'0','true'),(4,186,'1','237.63',2,'1','false'),(5,181,'1','532.81',3,'0','true'),(6,106,'2','735.99',1,'1','false'),(7,213,'2','108.99',4,'0','true'),(8,286,'2','718.14',5,'1','false'),(9,116,'2','955.26',4,'0','true'),(10,202,'2','632.83',6,'1','false'),(11,160,'3','251.20',1,'0','true'),(12,111,'3','125.34',6,'1','false'),(13,166,'3','257.50',4,'0','true'),(14,174,'3','241.59',6,'1','false'),(15,292,'3','628.25',5,'0','true');