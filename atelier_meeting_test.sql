use meta_survey;

select * from SELECTION_WS where id_atelier = 152;
insert SELECTION_SESSION (id_person, id_atelier, creation_date, attended, attended_date, etat) values
						(50, 173, GETDATE(), 1, GETDATE(), 2),
						(80, 173, GETDATE(), 1, GETDATE(), 2),
						(83, 173, GETDATE(), 1, GETDATE(), 2);

insert SELECTION_SESSION (id_person, id_atelier, creation_date, attended, attended_date, etat) values
						(84, 174, GETDATE(), 1, GETDATE(), 2),
						(80, 174, GETDATE(), 1, GETDATE(), 2),
						(83, 174, GETDATE(), 1, GETDATE(), 2);

insert SELECTION_WS (id_person, id_atelier, attended, attended_date, etat) values
					(50, 151, 1, GETDATE(), 2),
					(80, 151, 1, GETDATE(), 2),
					(86, 151, 1, GETDATE(), 2);

insert SELECTION_WS (id_person, id_atelier, attended, attended_date, etat) values
					(83, 152, 1, GETDATE(), 2),
					(86, 152, 1, GETDATE(), 2);


-- meetings 
	-- id_meeting 65 : 80, 83, 84
	-- id_meeting 66 : 50, 80, 86

