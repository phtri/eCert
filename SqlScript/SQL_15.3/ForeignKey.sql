ALTER TABLE Certificate
ADD FOREIGN KEY ([CampusId]) REFERENCES Campus(CampusId);

ALTER TABLE Portfolio_Certificate
ADD FOREIGN KEY ([CertificateId]) REFERENCES Certificate(CertificateId);

ALTER TABLE Portfolio_Certificate
ADD FOREIGN KEY ([PortfolioId]) REFERENCES Portfolio(PortfolioId);

ALTER TABLE Certificate_Categorie
ADD FOREIGN KEY ([CertificateId]) REFERENCES Certificate(CertificateId);

ALTER TABLE Certificate_Categorie
ADD FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId);

ALTER TABLE Category
ADD FOREIGN KEY (UserId) REFERENCES [User](UserId);

ALTER TABLE Report
ADD FOREIGN KEY (CertificateId) REFERENCES Certificate(CertificateId);

ALTER TABLE Report
ADD FOREIGN KEY (UserId) REFERENCES [User](UserId);

ALTER TABLE Portfolio
ADD FOREIGN KEY ([UserId]) REFERENCES [User](UserId);

ALTER TABLE [User]
ADD FOREIGN KEY ([RoleId]) REFERENCES Role(RoleId);

ALTER TABLE UserLog
ADD FOREIGN KEY ([UserId]) REFERENCES [User](UserId);

ALTER TABLE CertificateContent
ADD FOREIGN KEY ([CertificateId]) REFERENCES Certificate(CertificateId);

/* 15.03 - TriHP */
ALTER TABLE Campus
ADD FOREIGN KEY ([EducationSystemId]) REFERENCES EducationSystem(EducationSystemId);

ALTER TABLE [Role]
ADD FOREIGN KEY ([CampusId]) REFERENCES Campus(CampusId);

/*
ALTER TABLE Role_Permission
ADD FOREIGN KEY ([RoleId]) REFERENCES Role(RoleId);

ALTER TABLE Role_Permission
ADD FOREIGN KEY ([PermissionId]) REFERENCES [Permission](PermissionId);
*/

