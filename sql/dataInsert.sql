
--Person
INSERT INTO ehotel.person (ssn,full_name,street_number,street_name,apt_number,city,p_state,zip) 
VALUES (768395382, 'Thomas Charette',5,'Donec Road',3,'Gatineau','CA','J7T4R1'),
(890567234, 'Walid Bounouar', 876, 'Pérez St.', 12, 'Ottawa', 'ON', 'A2B3N4'),
(876654543, 'Raphael Budd', 765, 'Kanata Rd.', null, 'Kanata', 'ON', 'C2D3H4'),
(345456567, 'Jean-Lou de Carufel', 120, 'Montcalm road', null, 'Gatineau', 'QC', 'H8N0K6'),
(567678789, 'Robert Laganière', 123, 'Montclair road', 1715, 'Gatineau', 'QC', 'H3K4L1');

--Customer
INSERT INTO ehotel.customer (ssn,register_date,username,password)
VALUES (768395382,'2019-04-04','thomas@gmail.com','qwerty'),
(890567234, '2019-04-04', 'wboun@gmail.com', 'walidb'),
(876654543, '2019-04-04', 'rbudd@gmail.com', 'raphaelb');

--Employee
INSERT INTO ehotel.employee(ssn,date_of_employment,username,password)
VALUES (345456567, '2019-04-01', 'jcaruf@gmail.com', 'jeanlou'),
(567678789, '2019-03-23', 'rlagan@gmail.com', 'boblaga');


--Hotel Chains
INSERT INTO ehotel.hotelchain (hotel_chain_name,street_number,street_name,apt_number,city,hc_state,zip,num_hotels) 
VALUES ('Magna Hotels',3586, 'Netus Road',32,'Carson City','NV','87143',0),
('Nisi Hotels',611,'Vehicula Rd.',55,'Louisville','KY','84522',0),
('Sagittis Corporation',404,'Eleifend Street',41,'Chattanooga','TN','72781',0),
('Velit Hotels Inc',1362, 'Risus Avenue',46,'Salt Lake City','UT','42153',0),
('Parturient',9466, 'Sem Street',56,'Pike Creek','DE','77621',0);



--Hotels
--First 2 hotels are in the same city + same state
INSERT INTO ehotel.hotel (hcid,hotel_name,manager,category,num_rooms,street_number,street_name,apt_number,city,h_state,zip,email) 
VALUES ('1','Magna Lorem Ipsum LLC',345456567,3,0,4580, 'Eleifend. Road',31,'San Francisco','CA','92781','Duis@Nulla.net'),
('1','Sollicitudin Inc.',345456567,2,0,4691,'Ac Road',25,'San Francisco','CA','18041','neque.et@auctor.edu'),
('1','Cursus Luctus Ipsum Inc.',345456567,5,0,1540,'Vivamus Rd.',19,'Columbia','MD','44049','eget.metus@Nunc.org'),
('1','Mi LLP',567678789,3,0,9336,'Dictum Ave',18,'Naperville','IL','16136','eu.tellus@Curabitursedtortor.org'),
('1','Id Magna Industries',567678789,2,0,7589,'Etiam St.',67,'Biloxi','MS','31789','ornare@sociisnatoquepenatibus.ca'),
('1','Cursus Purus Nullam LLC',567678789,3,0,2739,'Interdum Rd.',2,'Bloomington','MN','64327','ut.nisi.a@hendrerita.co.uk'),
('1','Lorem Ipsum Associates',567678789,2,0,1927,'Augue St.',15,'Newark','DE','38190','Sed.eu@consequat.net'),
('1','Bibendum Fermentum Metus Consulting',567678789,4,0,1597,'Et Avenue',77,'Glendale','AZ','86288','metus.In@Integervitae.co.uk'),
('2','Donec Porttitor Limited',567678789,1,0,9919,'Ut Street',30,'Lincoln','NE','36647','egestas.Fusce@elit.ca'),
('2','Duis A Ltd',567678789,1,0,2292,'Neque. St.',98,'West Valley City','UT','72147','magna.nec@duinec.co.uk'),
('2','Elit Erat Vitae Consulting',567678789,5,0,1315,'Neque Av.',91,'Montgomery','AL','35959','eget.ipsum.Donec@elitpede.co.uk'),
('2','Magnis Dis Parturient Consulting',567678789,1,0,1494,'Pharetra Rd.',25,'Sandy','UT','50384','egestas.a.scelerisque@Crasloremlorem.edu'),
('2','Ac Corp.',567678789,3,0,8741,'Lorem Ave',54,'Augusta','ME','77315','sollicitudin@conguea.edu'),
('2','Donec Est Institute',567678789,3,0,8061,'Aliquet St.',55,'Hilo','HI','19507','ornare.facilisis.eget@musAeneaneget.edu'),
('2','Hendrerit A Industries',567678789,4,0,2650,'Nunc. Street',8,'Baton Rouge','LA','84237','Sed.auctor.odio@cubiliaCuraeDonec.com'),
('2','Ac LLP',567678789,3,0,5922,'Viverra. Rd.',78,'South Portland','ME','75370','Sed.diam@iaculisaliquetdiam.co.uk'),
('3','Nibh Aliquam Ornare Institute',567678789,4,0,704,'Donec Rd.',24,'Chattanooga','TN','54109','varius@cursusin.edu'),
('3','Hendrerit Institute',567678789,4,0,528,'Magna Rd.',14,'Hilo','HI','65640','elit@condimentumeget.ca'),
('3','Turpis Company',567678789,2,0,6725,'Vitae St.',29,'Reno','NV','94615','sagittis.lobortis@elementum.edu'),
('3','Eget Varius Corporation',567678789,2,0,804,'Donec Road',51,'Broken Arrow','OK','87524','mi@augue.com'),
('3','Purus Mauris A LLC',567678789,2,0,6194,'Convallis Avenue',12,'Davenport','IA','16732','Morbi.metus@interdumNuncsollicitudin.edu'),
('3','Arcu LLC',567678789,2,0,762,'Penatibus Street',65,'Great Falls','MT','80189','suscipit.nonummy@Duis.org'),
('3','Nec Company',567678789,3,0,2146,'Varius Street',34,'Bridgeport','CT','85569','metus.Vivamus@Vivamussit.com'),
('3','Ipsum Ltd',567678789,5,0,3236,'Per Ave',64,'Lansing','MI','72258','erat@acarcu.org'),
('4','Aliquet Institute',567678789,5,0,467,'Eu Av.',46,'Knoxville','TN','98815','amet.risus@idmollisnec.com'),
('4','Ante Nunc Corp.',567678789,5,0,6905,'Phasellus Av.',21,'Sacramento','CA','92111','nulla.In.tincidunt@Aeneangravida.edu'),
('4','Pede Et Risus Incorporated',567678789,1,0,841,'Magna. Road',1,'Shreveport','LA','73563','tempus.non@magnaLoremipsum.org'),
('4','Natoque Penatibus Et Limited',567678789,1,0,361,'Amet St.',38,'Olympia','WA','72662','posuere@eu.ca'),
('4','Nec Eleifend Non Foundation',567678789,5,0,1825,'Tempus Road',42,'Dover','DE','64922','mi.eleifend.egestas@lorem.net'),
('4','Imperdiet Ornare In Inc.',567678789,2,0,840,'Vitae, St.',41,'Denver','CO','52809','ipsum.Phasellus@eunibh.co.uk'),
('4','Vel Vulputate Eu Incorporated',567678789,1,0,686,'Id Rd.',53,'Colchester','VT','24724','Duis.ac.arcu@mi.com'),
('4','Vulputate Corporation',567678789,1,0,184,'Nulla Street',31,'Akron','OH','70487','Donec@magnaa.edu'),
('5','Elit Pede Ltd',567678789,3,0,1445,'Sem Avenue',2,'Southaven','MS','15029','risus@ornare.ca'),
('5','Nonummy Ultricies Ornare LLP',567678789,4,0,7255,'Sit Road',56,'Des Moines','IA','48029','sollicitudin.adipiscing@sollicitudincommodoipsum.co.uk'),
('5','Urna Convallis Erat Incorporated',567678789,2,0,431,'Cras Road',90,'North Las Vegas','NV','26059','volutpat.nunc.sit@aliquamarcuAliquam.org'),
('5','Id Corporation',567678789,3,0,8060,'Odio. Rd.',4,'Fort Worth','TX','50595','ac.risus@Ut.com'),
('5','Mauris Blandit Industries',567678789,2,0,816,'Nec St.',68,'Mobile','AL','35141','vitae.aliquet@consectetuer.org'),
('5','Duis A Company',567678789,3,0,8778,'Duis Rd.',93,'Hattiesburg','MS','86185','vitae@blanditat.edu'),
('5','Mattis Limited',567678789,4,0,151,'Orci, Ave',91,'Miami','FL','84133','lorem.vehicula@metusurna.ca'),
('5','Tempus Scelerisque Lorem Company',567678789,4,0,205,'Egestas Road',44,'Boise','ID','22123','et.malesuada.fames@feugiat.net');



--Rooms
INSERT INTO ehotel.room (room_num,hid,price,capacity,landscape,isextandable) 
VALUES (396,'1','190.57',5,1,'true'),(257,'1','373.07',3,0,'false'),(258,'1','137.67',8,0,'false'),(254,'1','78.20',3,0,'true'),(236,'1','156.79',2,0,'true'),(434,'2','89.65',7,0,'false'),(374,'2','308.81',4,1,'true'),(117,'2','67.08',8,1,'false'),(352,'2','321.00',6,1,'false'),(460,'2','62.96',4,1,'true'),(367,'3','179.43',7,0,'true'),(488,'3','390.97',7,1,'false'),(211,'3','260.68',8,1,'true'),(124,'3','318.37',8,1,'false'),(444,'3','56.55',2,0,'false'),(212,'4','347.82',6,1,'true'),(352,'4','353.89',5,1,'true'),(288,'4','335.87',3,1,'false'),(165,'4','396.71',2,0,'true'),(117,'4','273.02',8,0,'false'),(215,'5','224.20',6,1,'false'),(497,'5','237.25',2,1,'true'),(426,'5','302.07',7,0,'true'),(465,'5','86.27',6,0,'false'),(298,'5','187.09',3,0,'true'),(435,'6','196.80',6,1,'false'),(104,'6','238.94',6,0,'false'),(198,'6','312.06',4,0,'true'),(209,'6','140.57',7,0,'true'),(144,'6','352.92',7,1,'false'),(131,'7','60.56',4,0,'true'),(326,'7','323.51',7,1,'false'),(455,'7','136.59',6,0,'false'),(119,'7','246.19',4,0,'true'),(291,'7','117.48',6,1,'true'),(499,'8','377.64',8,1,'false'),(191,'8','158.72',2,1,'true'),(346,'8','375.30',6,1,'false'),(284,'8','65.03',3,1,'false'),(369,'8','365.85',6,1,'true'),(200,'9','211.93',2,1,'true'),(489,'9','333.50',4,1,'false'),(214,'9','132.51',7,1,'true'),(127,'9','153.81',6,1,'false'),(381,'9','245.49',8,1,'false'),(461,'10','131.59',6,0,'true'),(443,'10','207.09',3,0,'true'),(454,'10','381.77',6,1,'false'),(460,'10','130.10',2,0,'true'),(368,'10','186.36',6,0,'false'),(274,'11','87.80',6,0,'false'),(383,'11','234.02',3,0,'true'),(402,'11','184.87',2,1,'true'),(147,'11','65.62',2,1,'false'),(267,'11','381.57',5,0,'true'),(208,'12','378.44',8,1,'false'),(165,'12','77.99',3,0,'false'),(413,'12','61.70',7,0,'true'),(150,'12','242.34',6,1,'true'),(173,'12','77.54',7,1,'false'),(214,'13','397.73',8,1,'true'),(398,'13','165.26',5,0,'false'),(221,'13','200.24',2,1,'false'),(120,'13','140.81',6,0,'true'),(379,'13','148.03',8,1,'true'),(289,'14','226.94',8,1,'false'),(278,'14','300.61',2,1,'true'),(386,'14','75.45',4,1,'false'),(264,'14','239.87',8,1,'false'),(150,'14','245.86',4,0,'true'),(169,'15','304.82',2,0,'true'),(416,'15','249.04',5,1,'false'),(226,'15','100.35',5,0,'true'),(134,'15','190.83',5,1,'false'),(400,'15','376.25',3,1,'false'),(339,'16','178.46',2,0,'true'),(402,'16','82.88',6,0,'true'),(161,'16','263.56',3,0,'false'),(342,'16','236.89',8,1,'true'),(292,'16','107.26',8,0,'false'),(299,'17','339.31',2,1,'false'),(202,'17','329.24',3,1,'true'),(403,'17','234.59',2,0,'true'),(455,'17','309.22',4,0,'false'),(282,'17','326.30',7,0,'true'),(444,'18','311.16',8,0,'false'),(247,'18','271.87',5,0,'false'),(184,'18','297.40',2,1,'true'),(245,'18','123.93',6,1,'true'),(406,'18','62.70',4,0,'false'),(222,'19','336.44',3,1,'true'),(310,'19','293.34',2,1,'false'),(158,'19','382.99',4,1,'false'),(310,'19','249.45',6,0,'true'),(440,'19','110.95',3,0,'true'),(487,'20','162.52',2,0,'false'),(102,'20','174.39',7,0,'true'),(280,'20','108.51',2,1,'false'),(277,'20','338.73',7,1,'false'),(118,'20','184.17',4,1,'true'),
(242,'21','341.02',4,0,'true'),(138,'21','278.71',7,0,'false'),(418,'21','149.60',5,0,'false'),(276,'21','68.53',3,1,'true'),(437,'21','169.90',3,1,'true'),(126,'22','52.50',5,1,'false'),(191,'22','189.07',4,0,'true'),(446,'22','95.57',5,0,'false'),(115,'22','132.28',3,0,'false'),(170,'22','202.23',7,1,'true'),(473,'23','155.96',2,0,'true'),(410,'23','83.19',4,0,'false'),(193,'23','199.72',6,1,'true'),(364,'23','84.59',5,0,'false'),(288,'23','217.12',7,0,'false'),(257,'24','172.33',4,1,'true'),(476,'24','359.83',5,0,'true'),(463,'24','325.58',5,1,'false'),(370,'24','154.95',3,0,'true'),(228,'24','377.53',6,0,'false'),(176,'25','257.12',2,1,'false'),(414,'25','67.58',7,1,'true'),(330,'25','91.34',6,1,'true'),(341,'25','115.72',5,0,'false'),(172,'25','377.91',4,1,'true'),(228,'26','256.86',3,1,'false'),(197,'26','245.35',5,1,'false'),(441,'26','195.77',8,1,'true'),(314,'26','290.03',5,1,'true'),(303,'26','386.89',5,1,'false'),(366,'27','93.98',8,1,'true'),(369,'27','191.52',3,1,'false'),(162,'27','348.56',2,1,'false'),(156,'27','225.76',6,0,'true'),(135,'27','282.08',3,1,'true'),(275,'28','313.43',3,1,'false'),(282,'28','228.60',6,0,'true'),(334,'28','370.10',5,1,'false'),(259,'28','145.69',4,1,'false'),(221,'28','281.01',5,0,'true'),(265,'29','310.53',2,1,'true'),(110,'29','295.97',2,0,'false'),(309,'29','103.29',3,1,'true'),(232,'29','68.58',6,0,'false'),(190,'29','72.52',2,0,'false'),(320,'30','285.37',5,0,'true'),(486,'30','210.93',8,0,'true'),(499,'30','354.34',8,1,'false'),(284,'30','108.65',6,1,'true'),(222,'30','99.19',2,1,'false'),(205,'31','268.80',3,0,'false'),(469,'31','130.36',8,0,'true'),(189,'31','291.76',5,1,'true'),(382,'31','133.92',8,1,'false'),(378,'31','169.28',3,1,'true'),(165,'32','78.58',8,0,'false'),(284,'32','338.66',3,1,'false'),(479,'32','276.93',8,1,'true'),(115,'32','241.01',5,1,'true'),(369,'32','244.10',6,0,'false'),(175,'33','290.57',6,0,'true'),(234,'33','74.76',2,1,'false'),(375,'33','151.33',3,1,'false'),(225,'33','123.64',5,1,'true'),(470,'33','268.79',6,0,'true'),(141,'34','383.09',2,1,'false'),(343,'34','233.22',7,0,'true'),(220,'34','190.80',7,0,'false'),(100,'34','182.15',7,0,'false'),(347,'34','364.29',2,0,'true'),(186,'35','69.10',5,0,'true'),(184,'35','77.87',2,1,'false'),(305,'35','378.08',6,1,'true'),(320,'35','197.10',2,1,'false'),(278,'35','89.80',2,0,'false'),(420,'36','233.92',6,0,'true'),(158,'36','66.13',5,0,'true'),(404,'36','193.33',8,0,'false'),(472,'36','66.05',7,0,'true'),(461,'36','162.65',4,0,'false'),(388,'37','128.77',5,1,'false'),(343,'37','292.55',2,0,'true'),(348,'37','101.39',2,1,'true'),(148,'37','74.90',5,1,'false'),(335,'37','391.24',7,1,'true'),(396,'38','77.18',6,1,'false'),(415,'38','68.80',6,0,'false'),(257,'38','216.74',5,1,'true'),(406,'38','248.10',7,1,'true'),(327,'38','222.59',2,0,'false'),(413,'39','64.21',6,0,'true'),(228,'39','374.46',6,1,'false'),(407,'39','51.18',7,1,'false'),(259,'39','170.10',2,1,'true'),(170,'39','112.86',7,1,'true'),(450,'40','204.45',2,1,'false'),(274,'40','257.00',2,0,'true'),(260,'40','395.13',7,0,'false'),(137,'40','117.70',6,0,'false'),(473,'40','285.82',6,1,'true');


--Amenity
INSERT INTO ehotel.amenity (amenity,rid) 
VALUES ('TV',1),('AC',2),('Fridge',3),('TV and AC',4),('Fridge and mini-bar',5),('AC and fridge',6),('TV',7),('AC',8),('Fridge',9),('TV and AC',10),('Fridge and mini-bar',11),('AC and fridge',12),('TV',13),('AC',14),('Fridge',15),('TV and AC',16),('Fridge and mini-bar',17),('AC and fridge',18),('TV',19),('AC',20),('Fridge',21),('TV and AC',22),('Fridge and mini-bar',23),('AC and fridge',24),('TV',25),('AC',26),('Fridge',27),('TV and AC',28),('Fridge and mini-bar',29),('AC and fridge',30),('TV',31),('AC',32),('Fridge',33),('TV and AC',34),('Fridge and mini-bar',35),('AC and fridge',36),('TV',37),('AC',38),('Fridge',39),('TV and AC',40),('Fridge and mini-bar',41),('AC and fridge',42),('TV',43),('AC',44),('Fridge',45),('TV and AC',46),('Fridge and mini-bar',47),('AC and fridge',48),('TV',49),('AC',50),('Fridge',51),('TV and AC',52),('Fridge and mini-bar',53),('AC and fridge',54),('TV',55),('AC',56),('Fridge',57),('TV and AC',58),('Fridge and mini-bar',59),('AC and fridge',60),('TV',61),('AC',62),('Fridge',63),('TV and AC',64),('Fridge and mini-bar',65),('AC and fridge',66),('TV',67),('AC',68),('Fridge',69),('TV and AC',70),('Fridge and mini-bar',71),('AC and fridge',72),('TV',73),('AC',74),('Fridge',75),('TV and AC',76),('Fridge and mini-bar',77),('AC and fridge',78),('TV',79),('AC',80),('Fridge',81),('TV and AC',82),('Fridge and mini-bar',83),('AC and fridge',84),('TV',85),('AC',86),('Fridge',87),('TV and AC',88),('Fridge and mini-bar',89),('AC and fridge',90),('TV',91),('AC',92),('Fridge',93),('TV and AC',94),('Fridge and mini-bar',95),('AC and fridge',96),('TV',97),('AC',98),('Fridge',99),('TV and AC',100),
('AC',101),('Fridge',102),('TV and AC',103),('Fridge and mini-bar',104),('AC and fridge',105),('TV',106),('AC',107),('Fridge',108),('TV and AC',109),('Fridge and mini-bar',110),('AC and fridge',111),('TV',112),('AC',113),('Fridge',114),('TV and AC',115),('Fridge and mini-bar',116),('AC and fridge',117),('TV',118),('AC',119),('Fridge',120),('TV and AC',121),('Fridge and mini-bar',122),('AC and fridge',123),('TV',124),('AC',125),('Fridge',126),('TV and AC',127),('Fridge and mini-bar',128),('AC and fridge',129),('TV',130),('AC',131),('Fridge',132),('TV and AC',133),('Fridge and mini-bar',134),('AC and fridge',135),('TV',136),('AC',137),('Fridge',138),('TV and AC',139),('Fridge and mini-bar',140),('AC and fridge',141),('TV',142),('AC',143),('Fridge',144),('TV and AC',145),('Fridge and mini-bar',146),('AC and fridge',147),('TV',148),('AC',149),('Fridge',150),('TV and AC',151),('Fridge and mini-bar',152),('AC and fridge',153),('TV',154),('AC',155),('Fridge',156),('TV and AC',157),('Fridge and mini-bar',158),('AC and fridge',159),('TV',160),('AC',161),('Fridge',162),('TV and AC',163),('Fridge and mini-bar',164),('AC and fridge',165),('TV',166),('AC',167),('Fridge',168),('TV and AC',169),('Fridge and mini-bar',170),('AC and fridge',171),('TV',172),('AC',173),('Fridge',174),('TV and AC',175),('Fridge and mini-bar',176),('AC and fridge',177),('TV',178),('AC',179),('Fridge',180),('TV and AC',181),('Fridge and mini-bar',182),('AC and fridge',183),('TV',184),('AC',185),('Fridge',186),('TV and AC',187),('Fridge and mini-bar',188),('AC and fridge',189),('TV',190),('AC',191),('Fridge',192),('TV and AC',193),('Fridge and mini-bar',194),('AC and fridge',195),('TV',196),('AC',197),('Fridge',198),('TV and AC',199),('TV',200),
('TV',2),('AC',3),('Fridge',4),('TV and AC',5),('Fridge and mini-bar',1);


--damage
INSERT INTO ehotel.damage (damage,rid) 
VALUES ('Broken bed',146),
('Lots of damage',102),
('Broken toilet',191),
('Stains on the carpet',19),
('Broken mirror',34),
('Minor damage',156),
('Need deep cleaning',4),
('Broken lamp',48),
('Major damage',135),
('Scratches on the walls',108);


--hotelChainPhone
INSERT INTO ehotel.hotelchainphone (phone_number,hcid) 
VALUES ('(770) 569-2636',1),('(756) 484-4506',1),
('(226) 901-1468',2),('(273) 503-7690',2),
('(661) 865-6930',3),('(582) 571-8083',3),
('(526) 329-5266',4),('(230) 196-4059',4),
('(686) 845-0384',5),('(401) 322-1122',5);


--hotelChainEmail
INSERT INTO ehotel.hotelchainemail (email,hcid)
VALUES ('gravida.mauris@convallisincursus.ca',1),('amet.nulla@at.org',1),
('pede.Cum.sociis@sedsapienNunc.co.uk',2),('dolor@justoPraesent.com',2),
('nunc.sed@Curabituregestasnunc.edu',3),('ultrices.Vivamus@Fuscediamnunc.edu',3),
('vitae@blanditenimconsequat.ca',4),('sapien.Nunc@massarutrum.ca',4),
('semper.dui.lectus@tinciduntpedeac.net',5),('sagittis.felis.Donec@Vestibulum.co.uk',5);


--hotelPhone
INSERT INTO ehotel.hotelphone (phone_number,hid)
VALUES ('(851) 294-2076','1'),('(883) 927-0977','2'),('(646) 203-4091','3'),('(217) 595-7972','4'),('(990) 527-1501','5'),('(777) 276-2699','6'),('(647) 959-9878','7'),('(849) 272-6362','8'),('(847) 413-5707','9'),('(863) 259-1297','10'),('(644) 625-2027','11'),('(676) 776-8169','12'),('(815) 598-5520','13'),('(113) 157-9349','14'),('(357) 804-1699','15'),('(993) 641-7929','16'),('(421) 805-9220','17'),('(474) 526-8948','18'),('(876) 479-5313','19'),('(196) 868-5078','20'),('(796) 788-9480','21'),('(443) 712-6988','22'),('(559) 950-1978','23'),('(656) 980-5985','24'),('(106) 937-6792','25'),('(702) 586-7273','26'),('(361) 823-3321','27'),('(898) 478-9523','28'),('(799) 568-4104','29'),('(570) 437-0405','30'),('(524) 179-6086','31'),('(642) 249-5915','32'),('(147) 111-1356','33'),('(746) 993-9878','34'),('(575) 360-4416','35'),('(934) 270-0518','36'),('(817) 710-2925','37'),('(377) 909-6384','38'),('(675) 242-7495','39'),('(488) 257-1117','40');



-- BOOKINGS --
INSERT INTO eHotel.booking (rid,customer_ssn,start_date,end_date)
VALUES (135, 890567234, '2019-05-01', '2019-05-15'),
(135, 876654543, '2019-05-16', '2019-05-19'),
(29, 890567234, '2019-07-03', '2019-07-18'),
(189, 876654543, '2019-08-05', '2019-08-08'),
(105, 876654543, '2019-12-23', '2019-01-09');

-- RENTINGS --
INSERT INTO eHotel.renting (rid,customer_ssn,employee_ssn,start_date,end_date)
VALUES (127, 890567234, 345456567, '2019-05-01', '2019-05-15'),
(129, 876654543, 345456567, '2019-05-16', '2019-05-19'),
(23, 890567234, 567678789, '2019-07-03', '2019-07-18'),
(200, 876654543, 567678789, '2019-08-05', '2019-08-08'),
(86, 876654543, 345456567, '2019-12-23', '2019-01-09');

-- ROLE --
INSERT INTO eHotel.role (role,employee_ssn)
VALUES('Front Desk', 567678789),
('Event Manager', 345456567),
('Hotel General Manager', 567678789),
('Event Planner', 345456567),
('Concierge', 567678789),
('Groups Sales', 345456567);

-- BOOKING ARCHIVES --
INSERT INTO eHotel.booking (rid,customer_ssn,start_date,end_date)
VALUES (127, 890567234, '2019-05-01', '2019-05-15'),
(129, 876654543, '2019-05-16', '2019-05-19'),
(23, 890567234, '2019-07-03', '2019-07-18'),
(200, 876654543, '2019-08-05', '2019-08-08'),
(86, 876654543, '2019-12-23', '2019-01-09');