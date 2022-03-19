--1-5 were in previous task

--6 works
SELECT actors.id, actors.surname, actors.name, actors.birthday, actors.motherland, actors.number_of_movies
FROM producer
INNER JOIN movies_producers ON producer.motherland = 'UK' AND producer.id = movies_producers.producer_id
INNER JOIN movies ON movies.id = movies_producers.movie_id AND 1990 <= movies.year AND movies.year <= 2010 --changed numbers for test
INNER JOIN movies_actors ON movies.id = movies_actors.movie_id
INNER JOIN actors ON actors.id = movies_actors.actor_id;

--7 works
SELECT year_interval, avg(budget)
FROM (SELECT
    CASE
        WHEN year < 2000 THEN 'Before 2000'
        WHEN 2000 <= year AND year < 2005 THEN '2000-2004'
        WHEN 2005 <= year AND year < 2010 THEN '2005-2009'
        WHEN 2010 <= year THEN 'After 2009'
    END year_interval, budget FROM movies) AS a
GROUP BY year_interval;

--8 works
SELECT sum(budget)
FROM producer
INNER JOIN movies_producers ON producer.id = movies_producers.producer_id AND (left(producer.surname, 1) = 'B' OR left(producer.surname, 1) = 'T') --changed letters for test
INNER JOIN movies ON movies.id = movies_producers.movie_id;

--9 works
SELECT year, max(budget)
FROM movies GROUP BY year;

--10 works
SELECT movies.id, movies.name, description, year, country, budget
FROM movies, (SELECT min(budget) FROM movies WHERE year > 2010) AS min
WHERE budget < min;

--11 works
SELECT producer.id, surname, producer.name, birthday, motherland
FROM (SELECT min(budget) FROM movies WHERE year = 2013) AS min, (SELECT max(budget) FROM movies WHERE year = 2001) AS max, movies --changed dates for test
INNER JOIN movies_producers ON movies_producers.movie_id = movies.ID
INNER JOIN producer ON producer.id = movies_producers.producer_id
WHERE min < budget AND budget < max;

--12 works
SELECT producer.id, surname, producer.name, birthday, motherland
FROM producer
WHERE producer.id IN (SELECT producer_id FROM movies_producers GROUP BY producer_id HAVING count(movies_producers) > 1 UNION --changed number for test
     SELECT producer_id FROM movies INNER JOIN movies_producers ON movies_producers.movie_id = movies.id AND year < 2000);