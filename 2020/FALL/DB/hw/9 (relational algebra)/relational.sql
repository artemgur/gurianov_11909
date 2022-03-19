--1
SELECT * FROM movies WHERE year = 2012;

--2
SELECT producer.id, producer.name, surname, birthday, motherland, best_movie_id FROM movies
    JOIN movies_producers ON movies.id = movies_producers.movie_id
    JOIN producer ON producer.id = movies_producers.producer_id;

--3
SELECT movies.id, movies.name, description, year, country, budget, rating, duration FROM movies
    JOIN movies_producers mp ON movies.id = mp.movie_id AND rating > 7
    JOIN producer p ON p.id = mp.producer_id AND motherland = 'Russia';

--4
SELECT actors.name FROM
    (SELECT actor_id/*, COUNT(movie_id) AS count*/
        FROM movies_actors GROUP BY actor_id
        HAVING COUNT(movie_id) > 10
        ) AS a
    JOIN actors ON actor_id = id /*AND count > 10*/ AND motherland = 'USA';

--5
WITH left1 AS(
    SELECT producer.id, producer.name, surname, birthday, motherland, best_movie_id, genre FROM movies_genres
    JOIN movies_producers ON movies_genres.movie_id = movies_producers.movie_id
    JOIN producer ON id = movies_producers.producer_id
), right1 AS (
    SELECT genre FROM movies_genres
)
SELECT id, name, surname, birthday, motherland, best_movie_id FROM left1
    JOIN right1 ON left1.genre = right1.genre
    GROUP BY id
    HAVING COUNT(id) = (SELECT COUNT(genre) FROM right1);

--6
WITH left1 AS(
    SELECT producer.id, producer.name, surname, birthday, motherland, best_movie_id, year FROM movies
    JOIN movies_producers ON movies.id = movies_producers.movie_id
    JOIN producer ON movies.id = best_movie_id
), right1 AS (
    SELECT year FROM actors
    JOIN movies_actors ON actors.id = movies_actors.actor_id AND surname='Аль Пачино'
    JOIN movies ON movies.id >= movies_actors.movie_id
)
SELECT id, name, surname, birthday, motherland, best_movie_id FROM left1
    JOIN right1 ON left1.year = right1.year
    GROUP BY id, name, surname, birthday, motherland, best_movie_id
    HAVING COUNT(id) >= (SELECT COUNT(year) FROM right1);

--7 TODO finish
SELECT name FROM (SELECT id, name FROM producer
    EXCEPT (SELECT id, name FROM movies_genres
            JOIN movies_producers mp ON movies_genres.movie_id = mp.movie_id AND genre = 'Комедия'
            JOIN producer p ON p.id = mp.producer_id)) AS a;



