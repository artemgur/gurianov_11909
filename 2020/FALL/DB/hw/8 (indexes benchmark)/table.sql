CREATE TABLE indexes_test(
	id SERIAL PRIMARY KEY,
	name VARCHAR(30),
	date TIMESTAMP,
	description TEXT
);

CREATE INDEX name_index ON indexes_test(name ASC);--Сортировка + покрывающий

CREATE INDEX name_date_index ON indexes_test(name, date);--По набору полей

CREATE INDEX date_XIX_century_a ON indexes_test(date) WHERE EXTRACT(CENTURY FROM date) = 19;--По условию