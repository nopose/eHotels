
--We assume an area is a state
CREATE VIEW ehotel.num_hotel_area AS 
SELECT COUNT(rid) AS room_count, h.h_state
FROM ehotel.room AS r, ehotel.hotel AS h
WHERE r.hid = h.hid
GROUP BY h.h_state;


CREATE VIEW ehotel.capacity_room AS 
SELECT h.hotel_name, r.room_num, r.capacity
FROM ehotel.room AS r, ehotel.hotel AS h
WHERE r.hid = h.hid;