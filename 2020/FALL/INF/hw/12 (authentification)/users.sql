CREATE TABLE users(
    id SERIAL PRIMARY KEY,
    login VARCHAR(20) NOT NULL UNIQUE,--store hash instead of actual value?
    password VARCHAR(32) NOT NULL, --length???
    salt VARCHAR(32) NOT NULL --length???
)