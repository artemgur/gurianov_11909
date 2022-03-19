--1
CREATE TABLE movies_actors(
	actor_id INTEGER,
	movie_id INTEGER,
	FOREIGN KEY (actor_id) REFERENCES actors(id) ON UPDATE CASCADE,
	FOREIGN KEY (movie_id) REFERENCES movies(id) ON UPDATE CASCADE,
	PRIMARY KEY (actor_id, movie_id)
);
--INSERT INTO movies_actors VALUES 
CREATE TABLE movies_producers(
	producer_id INTEGER,
	movie_id INTEGER,
	FOREIGN KEY (producer_id) REFERENCES producer(id) ON UPDATE CASCADE,
	FOREIGN KEY (movie_id) REFERENCES movies(id) ON UPDATE CASCADE,
	PRIMARY KEY (producer_id, movie_id)
);

INSERT INTO movies_actors VALUES (2, 6);
INSERT INTO movies_actors VALUES (3, 1);
INSERT INTO movies_actors VALUES (5, 2);
INSERT INTO movies_actors VALUES (6, 5);

INSERT INTO movies_producers VALUES (8, 6);
INSERT INTO movies_producers VALUES (9, 1);
INSERT INTO movies_producers VALUES (10, 1);
INSERT INTO movies_producers VALUES (12, 3);


ALTER TABLE producer ADD COLUMN best_movie_id INTEGER;--2
ALTER TABLE producer ADD CONSTRAINT best_movie_id_constraint FOREIGN KEY (best_movie_id) REFERENCES movies(id) ON UPDATE RESTRICT;--2

--3 already done in previous task

ALTER TABLE movies ALTER COLUMN country SET DEFAULT 'UK'; --4 works
--ALTER TABLE actors ALTER COLUMN country SET DEFAULT 'UK'; --4 works
--ALTER TABLE producer ALTER COLUMN country SET DEFAULT 'UK'; --4 works

UPDATE producer
    SET best_movie_id = 6
    WHERE id = 8;

UPDATE producer
    SET best_movie_id = 1
    WHERE id = 9;

UPDATE producer
    SET best_movie_id = 1
    WHERE id = 10;

UPDATE producer
    SET best_movie_id = 3
    WHERE id = 12;


ALTER TABLE actors DROP CONSTRAINT actors_number_of_movies_check; --5 works

ALTER TABLE movies DROP CONSTRAINT movies_budget_check; --6 works
ALTER TABLE movies ADD CHECK (budget >= 1000);

--7
CREATE TABLE movies_genres(
	movie_id INTEGER,
	FOREIGN KEY (movie_id) REFERENCES movies(id) ON UPDATE CASCADE,
	genre VARCHAR(20),
	PRIMARY KEY (movie_id, genre)
);
DO $$
DECLARE
	idColumn INTEGER := MIN(id) FROM movies;
	arr VARCHAR(20)[] := NULL;
BEGIN
WHILE idColumn IS NOT NULL
LOOP
    arr := (SELECT genres FROM movies WHERE id = idColumn);
	FOR i IN 1 .. array_upper(arr, 1)
    LOOP
		INSERT INTO movies_genres VALUES (idColumn, arr[i]);
    END LOOP;
    idColumn := (SELECT MIN(id) FROM movies WHERE id > idColumn);
END LOOP;
END $$;
ALTER TABLE movies DROP COLUMN genres;

--8
CREATE TYPE COUNTRY AS ENUM ('USA', 'UK', 'Russia', 'France', 'Germany', 'Morocco', 'Lebanon'); --Added more values as some famous actors are born in weird places
ALTER TABLE producer ALTER COLUMN motherland DROP DEFAULT;
ALTER TABLE actors ALTER COLUMN motherland TYPE country USING motherland::country;--What will happen with data?
ALTER TABLE producer ALTER COLUMN motherland TYPE country USING motherland::country;
ALTER TABLE producer ALTER COLUMN motherland SET DEFAULT 'USA'; 
--ALTER TABLE actors RENAME COLUMN motherland TO motherland_old;
--ALTER TABLE producer RENAME COLUMN motherland TO motherland_old;
--ALTER TABLE actors ADD COLUMN motherland COUNTRY;
--ALTER TABLE producer ADD COLUMN motherland COUNTRY;

ALTER TABLE actors ADD CHECK (birthday < CURRENT_DATE); --9 works
ALTER TABLE producer ADD CHECK (birthday < CURRENT_DATE);

--10
CREATE VIEW actors_in_movies_after_2000 AS
	SELECT DISTINCT actors.id, surname, actors.name, birthday, motherland, number_of_movies
	FROM movies
	INNER JOIN movies_actors
	ON movies.id = movie_id AND year > 2000
	INNER JOIN actors
	ON actors.id = actor_id;
	

ALTER TABLE movies ALTER COLUMN name TYPE VARCHAR(100);
UPDATE movies SET name = CONCAT(name, ' (', year, ')') --11 works
