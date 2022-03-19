-- CREATE DATABASE filmography
--     WITH
--     OWNER = postgres
--     ENCODING = 'UTF8'
--     CONNECTION LIMIT = -1;
--
CREATE SEQUENCE people_ids
	AS INTEGER;
	
CREATE TABLE movies(
	name VARCHAR(50),
	description VARCHAR(500),
	year SMALLINT CHECK (year > 1900 AND date_part('year', CURRENT_DATE) + 10 >= YEAR),
	genres VARCHAR(20)[],
	country VARCHAR(50),
	budget INTEGER CHECK (budget >= 10000),
	PRIMARY KEY(name, year)
);
CREATE TABLE actors(
	surname VARCHAR(50),
	name VARCHAR(50),
	birthday DATE,
	motherland VARCHAR(50),
	number_of_movies SMALLINT CHECK (number_of_movies > 5),
	UNIQUE (surname, name, birthday),
	id INTEGER PRIMARY KEY DEFAULT nextval('people_ids')
);
CREATE TABLE producer(
	surname VARCHAR(50),
	name VARCHAR(50),
	birthday DATE,
	motherland VARCHAR(50) DEFAULT 'USA',
	id INTEGER PRIMARY KEY DEFAULT nextval('people_ids')
);

INSERT INTO movies VALUES ('Star Wars: Episode IV – A New Hope', 'A 1977 American epic space-opera film written and directed by George Lucas, produced by Lucasfilm and distributed by 20th Century Fox.', 1977, ARRAY['Space-opera', 'Science Fiction'], 'USA', 11000000);
INSERT INTO movies VALUES ('Ocean''s Eleven', 'A 2001 American heist comedy film directed by Steven Soderbergh and written by Ted Griffin.', 2001, ARRAY['Crime', 'Heist', 'Comedy'], 'USA', 85000000);
INSERT INTO movies VALUES ('The Lord of the Rings: The Return of the King', 'A 2003 epic fantasy adventure film directed by Peter Jackson, based on the third volume of J. R. R. Tolkien''s The Lord of the Rings.', 2003, ARRAY['Fantasy', 'Adventure'], 'USA', 94000000);
INSERT INTO movies VALUES ('Harry Potter and the Philosopher''s Stone', 'A 2001 fantasy film directed by Chris Columbus and distributed by Warner Bros. Pictures, based on J. K. Rowling''s 1997 novel of the same name.', 2001, ARRAY['Fantasy'], 'UK', 125000000);
INSERT INTO movies VALUES ('Now You See Me', 'A 2013 American heist thriller film directed by Louis Leterrier from a screenplay by Ed Solomon, Boaz Yakin, and Edward Ricourt and a story by Yakin and Ricourt.', 2013, ARRAY['Heist', 'Thriller'], 'USA', 75000000);
INSERT INTO movies VALUES ('Leon', 'a 1994 English-language French action-thriller film[5][6][7][8] written and directed by Luc Besson.', 1994, ARRAY['Action', 'Thriller'], 'France', 16000000);

INSERT INTO actors VALUES ('Willis', 'Bruce', '1955-03-19', 'Germany', 105);
INSERT INTO actors VALUES ('Reno', 'Jean', '1948-06-30', 'Morocco', 83);
INSERT INTO actors VALUES ('Ford', 'Harrison', '1942-06-13', 'USA', 56);
INSERT INTO actors VALUES ('Reeves', 'Keanu', '1964-09-02', 'Lebanon', 75);
INSERT INTO actors VALUES ('Clooney', 'George', '1961-05-06', 'USA', 48);
INSERT INTO actors VALUES ('Caine', 'Michael', '1933-03-14', 'UK', 133);

INSERT INTO producer VALUES ('Spielberg', 'Steven', '1946-12-18');
INSERT INTO producer VALUES ('Besson', 'Luc ', '1959-03-18', 'France');
INSERT INTO producer VALUES ('Lucas', 'George', '1944-05-14');
INSERT INTO producer VALUES ('Kurtz', 'Gary', '1940-06-27');
INSERT INTO producer VALUES ('McCallum', 'Rick', '1954-08-22', 'Germany');
INSERT INTO producer VALUES ('Osborne', 'Barrie', '1944-02-07');

ALTER TABLE movies DROP CONSTRAINT movies_pkey;
CREATE SEQUENCE movies_ids
	AS INTEGER;
ALTER TABLE movies ADD COLUMN id INTEGER PRIMARY KEY DEFAULT nextval('movies_ids');