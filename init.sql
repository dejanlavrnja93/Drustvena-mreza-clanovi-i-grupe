CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL,
    Name TEXT NOT NULL,
    Surname TEXT NOT NULL,
    Birthday TEXT NOT NULL
);

CREATE TABLE Groups (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    CreationDate TEXT NOT NULL
);

INSERT INTO Users (Username, Name, Surname, Birthday) VALUES
    ('jdoe', 'John', 'Doe', '1990-05-15'),
    ('asmith', 'Alice', 'Smith', '1985-10-22'),
    ('bwhite', 'Bob', 'White', '1992-07-09'),
    ('cjones', 'Carol', 'Jones', '1988-03-17'),
    ('dlee', 'David', 'Lee', '1995-12-01');

INSERT INTO Groups (Name, CreationDate) VALUES
    ('Admins', '2023-01-10'),
    ('Users', '2023-06-15');
