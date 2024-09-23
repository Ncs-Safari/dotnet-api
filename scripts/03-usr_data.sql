INSERT INTO usr.appl (id, code, name, description, bgcolor, iconfile, imagefile) VALUES ('adm', '1', 'Aplikasi Tatakelola Data KOKKUN', 'Aplikasi Tatakelola Data KOKKUN', '#17a2b8', 'iconfile.png', 'imagefile.jpg');
INSERT INTO usr.appl (id, code, name, description, bgcolor, iconfile, imagefile) VALUES ('web', '2', 'Aplikasi Frontend (web) KOKKUN', 'Aplikasi Frontend (web) KOKKUN', '#17a2b8', 'iconfile.png', 'imagefile.jpg');

INSERT INTO usr.appl_news_category (id, name, description, file_logo) VALUES ('1', 'Berita Langsung', 'Berita langsung (straight news) adalah laporan peristiwa yang ditulis secara singkat, padat, lugas, dan apa adanya.', 'no_logo.png');
INSERT INTO usr.appl_news_category (id, name, description, file_logo) VALUES ('2', 'Berita Opini', 'Berita opini (opinion news) yaitu berita mengenai pendapat, pernyataan, atau gagasan seseorang, biasanya pendapat para cendekiawan, sarjana, ahli, atau pejabat, mengenai suatu peristiwa.', 'no_logo.png');

INSERT INTO usr.appl_task (id, appl_task_parent_id, appl_id, index_no, task_name, controller_name, action_name, description, icon_name, custom_id, status) VALUES ('adm-01', NULL, 'adm', 10000, 'Tata Kelola', 'controller_name', 'action_name', 'Menu Tata Kelola Data Referensi dan Data Master', 'icon_name', NULL, 1);
INSERT INTO usr.appl_task (id, appl_task_parent_id, appl_id, index_no, task_name, controller_name, action_name, description, icon_name, custom_id, status) VALUES ('adm-01.01', 'adm-01', 'adm', 10100, 'Pengguna', 'controller_name', 'action_name', 'Menu Tata Kelola Data Pengguna', 'icon_name', NULL, 1);

INSERT INTO usr.role (id, code, name) VALUES ('public', 'public', 'Public Account');
INSERT INTO usr.role (id, code, name) VALUES ('admin', 'admin', 'Administrator');
INSERT INTO usr.role (id, code, name) VALUES ('staf', 'staf', 'Staf');

INSERT INTO usr.role_appl_task (role_id, appl_task_id) VALUES ('admin', 'adm-01');
INSERT INTO usr.role_appl_task (role_id, appl_task_id) VALUES ('admin', 'adm-01.01');

--eNcEeS2024
INSERT INTO usr.user (id, username, password, email, first_name, middle_name, last_name, address, phone_number, mobile_number, orgid, status, last_login, created_by, created_date, updated_by, updated_date) VALUES 
 ('01J4CBVMDZ5B3C65JSYWJJ4KGP', 'admin', 'X5Zx63AgB1LImMw4BM52j2RTfsrONKccbwqsZbiWyck=', 'admin@kokkun.id', 'Administrator', 'Sistem', 'Aplikasi', 'Kota Bogor', '08787006xxxxx', '08787006xxxxx', NULL, 1, NULL, '01J4CBVMDZ5B3C65JSYWJJ4KGP', '2024-05-27 10:06:55.807876', NULL, NULL)
,('public', 'public', 'X5Zx63AgB1LImMw4BM52j2RTfsrONKccbwqsZbiWyck=', 'no_email@kokkun.id', 'Pengguna', 'public', null, 'Kota Bogor', '08787006xxxxx', '08787006xxxxx', NULL, 1, NULL, '01J4CBVMDZ5B3C65JSYWJJ4KGP', '2024-05-27 10:06:55.807876', NULL, NULL)
;

INSERT INTO usr.user_file(user_id, type, category, file_name, file_thumbnail, title, description) VALUES 
('01J4CBVMDZ5B3C65JSYWJJ4KGP', 'image', 'photo-profile', 'no-photo-profile.png', 'no-photo-profile.png', 'Pas Foto', 'Pas Foto')
;

INSERT INTO usr.user_role (user_id, role_id) VALUES ('01J4CBVMDZ5B3C65JSYWJJ4KGP', 'admin');
