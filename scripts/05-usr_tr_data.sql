INSERT INTO public.tb_language (id, name, description) VALUES ('en', 'English', 'English');

INSERT INTO usr_tr.appl (id, language_id, name, description) VALUES ('adm', 'en', 'KOKKUN Data Management Application', 'KOKKUN Data Management Application');
INSERT INTO usr_tr.appl (id, language_id, name, description) VALUES ('web', 'en', 'KOKKUN Frontend (web) Application', 'KOKKUN Frontend (web) Application');

INSERT INTO usr_tr.appl_news_category (id, language_id, name, description) VALUES ('1', 'en', 'Straight News', 'Straight news is a report of events that is written briefly, concisely, straightforwardly and matter-of-factly.');
INSERT INTO usr_tr.appl_news_category (id, language_id, name, description) VALUES ('2', 'en', 'Opinion news', 'Opinion news is news about someone''s opinion, statement, or idea, usually the opinion of scholars, scholars, experts, or officials, about an event.');


INSERT INTO usr_tr.appl_task (id, language_id, task_name, description) VALUES ('adm-01', 'en', 'Utilities', 'Utilities Page');
INSERT INTO usr_tr.appl_task (id, language_id, task_name, description) VALUES ('adm-01.01', 'en', 'Users', 'Users Menu');
