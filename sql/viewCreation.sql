
--We assume an area is a state
CREATE VIEW ehotel.num_hotel_area AS 
SELECT COUNT(r.rid) AS room_count, h.h_state
FROM ehotel.hotel h
JOIN ehotel.room r
ON r.hid = h.hid
WHERE r.rid NOT IN (
	SELECT r.rid
	FROM ehotel.room r
	LEFT JOIN ehotel.booking b
	ON r.rid = b.rid
	LEFT JOIN ehotel.renting ren
	ON r.rid = ren.rid
	WHERE NOW() > b.start_date AND NOW() < b.end_date
	OR NOW() > ren.start_date AND NOW() < ren.end_date
)
GROUP BY h.h_state;


CREATE VIEW ehotel.capacity_room AS 
SELECT h.hid, h.hotel_name, r.rid, r.room_num, r.capacity
FROM ehotel.room AS r, ehotel.hotel AS h
WHERE r.hid = h.hid;