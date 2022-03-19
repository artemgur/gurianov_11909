--1
WITH RECURSIVE factorial(i, n) AS (
    VALUES (1, 1::BIGINT)
  UNION ALL
    SELECT i+1, n*(i+1) FROM factorial
)
SELECT i, n FROM factorial LIMIT 19;

--2
CREATE TABLE geographical_info(
    id SERIAL PRIMARY KEY,
    par_id INT,
    name VARCHAR(40),
    FOREIGN KEY (par_id) REFERENCES geographical_info(id)
);
INSERT INTO geographical_info VALUES
	(1, null, 'Планета Земля'),
	(2, 1, 'Континент Евразия'),
	(3, 1, 'Континент Северная Америка'),
	(4, 2, 'Европа'),
	(5, 4, 'Россия'),
	(6, 4, 'Германия'),
	(7, 5, 'Москва'),
	(8, 5, 'Санкт-Петербург'),
	(9, 6, 'Берлин');
WITH RECURSIVE europe(id, par_id, name, level) AS(
    SELECT id, par_id, name, 0 FROM geographical_info WHERE name='Европа'
    UNION ALL
    SELECT gi.id, gi.par_id, gi.name, level+1 FROM geographical_info gi, europe WHERE gi.par_id = europe.id
)
SELECT * FROM europe;

--4
CREATE TYPE GENDER AS ENUM ('Male', 'Female');
ALTER TABLE producer ADD COLUMN gender GENDER;
UPDATE producer SET gender='Male' WHERE gender IS NULL;--All real (not test) producers in table are male anyway
CREATE VIEW ordinary_view AS
    SELECT producer.id, surname, producer.name, birthday, motherland
    FROM movies
    INNER JOIN movies_producers ON movies.id = movies_producers.movie_id AND budget < 20000000
    INNER JOIN producer ON movies.id = producer.best_movie_id AND producer.gender = 'Male';
CREATE MATERIALIZED VIEW materialized_view AS
    SELECT producer.id, surname, producer.name, birthday, motherland
    FROM movies
    INNER JOIN movies_producers ON movies.id = movies_producers.movie_id AND budget < 20000000
    INNER JOIN producer ON movies.id = producer.best_movie_id AND producer.gender = 'Male';
UPDATE producer SET gender='Female' WHERE name='Test';

--5
SELECT movies.id, movies.name, description, year, country, budget
    FROM ordinary_view
    INNER JOIN movies_producers ON ordinary_view.id = movies_producers.producer_id
    INNER JOIN movies ON movies.id = movies_producers.movie_id;
SELECT movies.id, movies.name, description, year, country, budget
    FROM materialized_view
    INNER JOIN movies_producers ON materialized_view.id = movies_producers.producer_id
    INNER JOIN movies ON movies.id = movies_producers.movie_id;

--6
SELECT year FROM movies
    GROUP BY year
