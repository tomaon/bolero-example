CREATE TABLE `Book`
(
  `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
, `Title` TEXT
, `Author` TEXT
, `PublishDate` TEXT
, `Isbn` TEXT
)
;

INSERT INTO `Book`
  (
    `Title`
  , `Author`
  , `PublishDate`
  , `Isbn`
  )
VALUES
  (
    "The Fellowship of the Ring"
  , "J.R.R Tolkien"
  , "1954-07-29"
  , "978-0345339706"
  )
, (
    "The Two Towers"
  , "J.R.R Tolkien"
  , "1954-11-11"
  , "978-0345339713"
  )
, (
    "The Return of the King"
  , "J.R.R Tolkien"
  , "1955-10-20"
  , "978-0345339737"
  )
;
