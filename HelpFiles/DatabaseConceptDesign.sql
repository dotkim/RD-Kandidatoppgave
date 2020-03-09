USE Test;
GO

IF OBJECT_ID('Contacts')    IS NOT NULL DROP TABLE Contacts;
IF OBJECT_ID('People')      IS NOT NULL DROP TABLE People;
IF OBJECT_ID('Enterprise')  IS NOT NULL DROP TABLE Enterprise;

CREATE TABLE Enterprise(
  Id      INT IDENTITY(1,1) CONSTRAINT PK_Enterprise PRIMARY KEY,
  [Name]  NVARCHAR(100)     CONSTRAINT UQ_Name UNIQUE
);
GO

CREATE TABLE People(
  Id            INT IDENTITY(1,1) CONSTRAINT PK_People PRIMARY KEY,
  [Name]        NVARCHAR(100),
  EnterpriseId  INT
    CONSTRAINT FK_Enterprise_Id FOREIGN KEY
    REFERENCES Enterprise
);
GO

CREATE TABLE Contacts(
  PersonId        INT NOT NULL,
  ContactPersonId INT NOT NULL,
    CONSTRAINT CK_Contacts_PersonId_ContactPersonId
    PRIMARY KEY (PersonId, ContactPersonId)
);
GO

INSERT INTO Enterprise(Name)
VALUES  ('ACME Inc'),
        ('BnL'),
        ('Generic Enterprises');
GO

INSERT INTO People(Name, EnterpriseId)
VALUES  ('Willifred Manford', 1),
        ('Kim Nerli', 2),
        ('Bill Gates', 3),
        ('Steve Balmer', 3),
        ('Finn Erik', 1);
GO

INSERT INTO Contacts(PersonId, ContactPersonId)
VALUES  (1,2),
        (1,4),
        (2,5),
        (3,4),
        (3,1),
        (4,3),
        (5,1),
        (5,2);
GO

-- Select a person with enterprise data
SELECT  *
FROM    People AS ppl
JOIN    Enterprise AS ent
  ON    ppl.EnterpriseId = ent.Id
WHERE   ppl.Id = 1;

-- Select contacts for each person
SELECT    p1.[Name] AS PersonName, p2.[Name] AS ContactName
FROM      Contacts c
LEFT JOIN People p1 ON c.PersonId = p1.Id
LEFT JOIN People p2 ON c.ContactPersonId = p2.Id
WHERE     C.PersonId = 1;