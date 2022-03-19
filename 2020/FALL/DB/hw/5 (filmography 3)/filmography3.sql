--Some test data
INSERT INTO movies VALUES ('Test film (2000)', 'Just a non-existent test film', 2000, 'UK', 100000, 900); --Don't like horror films and don't know good films from 2000, so I will create test film
INSERT INTO movies VALUES ('Test film 2 (2001)', 'Just a non-existent test film', 2001, 'UK', 100000, 901); --Don't like horror films and don't know good films from 2000, so I will create test film
INSERT INTO producer(surname, name, birthday, best_movie_id, id) VALUES ('Test', 'Test', '1970-01-01', 900, 500);
INSERT INTO movies_producers VALUES (500, 900);
INSERT INTO movies_producers VALUES (500, 901);
INSERT INTO actors VALUES ('Tester', 'Test', '1970-01-01', 'UK', 42, 600);
INSERT INTO actors VALUES ('Test', 'Debug', '1970-01-01', 'UK', 42, 601);
INSERT INTO movies_actors VALUES (601, 900);
INSERT INTO movies_actors VALUES (600, 900);
INSERT INTO movies_genres VALUES (900, 'Horror');


--1 works
SELECT producer.id, surname, producer.name, birthday, motherland
	FROM producer
	INNER JOIN movies m ON m.id = producer.best_movie_id AND m.year = 2000;

-- SELECT producer.id, surname, producer.name, birthday, motherland
-- 	FROM movies
-- 	INNER JOIN movies_producers
-- 	ON movies.id = movie_id AND year > 2000
-- 	INNER JOIN producer
-- 	ON producer.id = producer_id;

--2 works
SELECT producer.id, surname, producer.name, birthday, motherland FROM producer
    WHERE producer.id IN
        (SELECT producer_id FROM movies_producers
            GROUP BY producer_id HAVING COUNT (producer_id) > 1); --5 is too much test data, 1 will go, easy to change number in query to any another number anyway

--3 works
SELECT movies.id FROM movies
    WHERE movies.id IN
        (SELECT movie_id FROM movies_actors
            GROUP BY movie_id HAVING COUNT (movie_id) > 1); --10 is too much test data, 1 will go, easy to change number in query to any another number anyway

--4 works
ALTER TABLE movies ADD COLUMN rating SMALLINT;
UPDATE movies
    SET rating = trunc(random() * 9 + 1) --OK for test
    WHERE rating IS NULL;
SELECT * FROM movies
    WHERE movies.country = 'USA'
    ORDER BY movies.rating DESC
    LIMIT 3; --10 is too much test data, 3 will go, easy to change number in query to any another number anyway

--5 works
SELECT DISTINCT movies.id, movies.name, description, year, country, budget
    FROM movies_genres
    INNER JOIN movies_actors
    ON movies_genres.genre = 'Horror' AND movies_genres.movie_id = movies_actors.movie_id
    INNER JOIN actors
    ON actors.id = movies_actors.actor_id AND actors.motherland = 'UK'
    INNER JOIN movies ON movies.id = movies_actors.movie_id;

--6 works
SELECT movies_genres.genre FROM movies_genres
    GROUP BY genre HAVING COUNT (genre) > 1; --5 is too much test data, 1 will go, easy to change number in query to any another number anyway

--7 works
ALTER TABLE movies ADD COLUMN duration SMALLINT;
UPDATE movies
    SET duration = trunc(random() * 200 + 1) --OK for test
    WHERE duration IS NULL;
SELECT * FROM movies
    ORDER BY movies.duration DESC --Not clear if we need longest or shortest films. Let it be longest
    OFFSET 4 ROWS --10 is too much test data, 3 will go, easy to change number in query to any another number anyway
    FETCH FIRST 4 ROW ONLY;

--8 works
SELECT genre
    FROM movies_genres
    INNER JOIN movies ON movies_genres.movie_id = movies.id
    INNER JOIN movies_producers ON movies.id = movies_producers.movie_id
    INNER JOIN producer ON movies_producers.producer_id = producer.id AND (producer.motherland = 'UK' OR producer.motherland = 'France')

