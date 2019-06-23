USE CocoUserDb

-- USER STATUS --
INSERT INTO Account.[Status]
([Name], [Description])
VALUES
('Pending', 'The user is wating for approval')

INSERT INTO Account.[Status]
([Name], [Description])
VALUES
('Actived', 'Active user')

INSERT INTO Account.[Status]
([Name], [Description])
VALUES
('Reported', 'The user is being reported')

INSERT INTO Account.[Status]
([Name], [Description])
VALUES
('Inactived', 'The user is Inactived')

INSERT INTO Account.[Status]
([Name], [Description])
VALUES
('Blocked', 'The user is being blocked')

-- USER ROLE --
INSERT INTO Auth.[Role]
([Name], [Description])
VALUES
('Admin', 'The administrator')

INSERT INTO Auth.[Role]
([Name], [Description])
VALUES
('Moderator', 'The moderator')

INSERT INTO Auth.[Role]
([Name], [Description])
VALUES
('Approver', 'The approver')

-- GENDER --
INSERT INTO Account.Gender
([Name])
VALUES
('Male')

INSERT INTO Account.Gender
([Name])
VALUES
('Female')

INSERT INTO Account.Gender
([Name])
VALUES
('Undefined')