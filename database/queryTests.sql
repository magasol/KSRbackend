insert into route (train_name, departure_date, departure_hour)
	values ('TF024','2019-03-12','6:52');
insert into route (train_name, departure_date, departure_hour)
	values ('TF011','2019-03-12','11:45');
insert into route (train_name, departure_date, departure_hour)
	values ('TF027','2019-03-12','15:40');
insert into route (train_name, departure_date, departure_hour)
	values ('TF014','2019-03-12','7:40');
insert into route (train_name, departure_date, departure_hour)
	values ('TF014','2019-03-12','12:10');
insert into route (train_name, departure_date, departure_hour)
	values ('TF014','2019-03-12','19:38');
insert into route (train_name, departure_date, departure_hour)
	values ('TF007','2019-03-12','14:36');
	
insert into subroute (from_station, to_station, price, travel_duration)
	values ('Warszawa','Kutno',6.80,'01:02');
insert into subroute (from_station, to_station, price, travel_duration)
	values ('Kutno','Konin',7.10,'00:52');
insert into subroute (from_station, to_station, price, travel_duration)
	values ('Konin','Poznañ',8.20,'00:56');
	
insert into subroute (from_station, to_station, price, travel_duration)
	values ('Szczecin','Stargard',5.80,'00:25');
insert into subroute (from_station, to_station, price, travel_duration)
	values ('Stargard','£obez',8.20,'00:31');
insert into subroute (from_station, to_station, price, travel_duration)
	values ('£obez','Bia³ogard',6.50,'00:58');
insert into subroute (from_station, to_station, price, travel_duration)
	values ('Bia³ogard','Koszalin',6.40,'00:30');
insert into subroute (from_station, to_station, price, travel_duration)
	values ('Koszalin','S³upsk',5.90,'00:45');
insert into subroute (from_station, to_station, price, travel_duration)
	values ('S³upsk','Wejherowo',10.40,'1:32');
insert into subroute (from_station, to_station, price, travel_duration)
	values ('Wejherowo','Gdañsk',8.10,'0:44');

insert into subroute (from_station, to_station, price, travel_duration)
	values ('Gdañsk','Tczew',5.40,'0:23');
insert into subroute (from_station, to_station, price, travel_duration)
	values ('Tczew','Malbork',4.10,'0:21');

//poznan-warszawa 1
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (1,120,1,1);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (2,120,2,1);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (3,120,3,1);

//	poznan-warszawa 2
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (1,120,1,2);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (2,120,2,2);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (3,120,3,2);
	
//poznan-warszawa 3
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (1,120,1,3);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (2,120,2,3);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (3,120,3,3);
	
//szczecin-gdansk 1
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (1,120,4,4);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (2,120,5,4);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (3,120,6,4);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (4,120,7,4);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (5,120,8,4);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (6,120,9,4);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (7,120,10,4);

//szczecin-gdansk 2
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (1,120,4,5);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (2,120,5,5);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (3,120,6,5);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (4,120,7,5);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (5,120,8,5);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (6,120,9,5);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (7,120,10,5);

//szczecin-gdansk 3
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (1,120,4,6);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (2,120,5,6);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (3,120,6,6);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (4,120,7,6);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (5,120,8,6);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (6,120,9,6);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (7,120,10,6);
	
//wejherowo-mlbork
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (1,120,9,7);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (2,120,10,7);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (3,120,11,7);
insert into route_subroute (route_order_number, seats_amount,subroute_id,route_id)
	values (4,120,12,7);
	
insert into traveller (first_name, last_name, email, login, password)
	values ('Jan','Nowak','jan.nowak@gmail.com', 'jan.nowak', 'password');

insert into ticket (name,price_percentage)
	values ('normalny',100);
insert into ticket (name,price_percentage)
	values ('studencki',51);
insert into ticket (name,price_percentage)
	values ('uczniowski',37);
	
insert into sale (payment_status, from_station, to_station, route_id, traveller_id)
	values (true,'Warszawa','Poznañ',3,1);

insert into sale_ticket (amount,sale_id,ticket_id)
	values (2,1,1);

