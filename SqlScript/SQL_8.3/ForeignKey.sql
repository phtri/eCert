ALTER TABLE Certificate
ADD FOREIGN KEY ([OrganizationId]) REFERENCES Organization(OrganizationId);

ALTER TABLE Portfolio_Certificate
ADD FOREIGN KEY ([CertificateId]) REFERENCES Certificate(CertificateId);

ALTER TABLE Portfolio_Certificate
ADD FOREIGN KEY ([PortfolioId]) REFERENCES Portfolio(PortfolioId);

ALTER TABLE Certificate_Categorie
ADD FOREIGN KEY ([CertificateId]) REFERENCES Certificate(CertificateId);

ALTER TABLE Certificate_Categorie
ADD FOREIGN KEY (CategoryId) REFERENCES Categorie(CategoryId);

ALTER TABLE Categorie
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

/* 08/02/2021 - TRIHP */
ALTER TABLE Certificate_User	
ADD FOREIGN KEY ([CertificateId]) REFERENCES Certificate(CertificateId);

ALTER TABLE CertificateContent
ADD FOREIGN KEY ([CertificateId]) REFERENCES Certificate(CertificateId);

/* 08/03/2021 - TRIHP */
ALTER TABLE Certificate_User
ADD FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]);

/*
ALTER TABLE Role_Permission
ADD FOREIGN KEY ([RoleId]) REFERENCES Role(RoleId);

ALTER TABLE Role_Permission
ADD FOREIGN KEY ([PermissionId]) REFERENCES [Permission](PermissionId);
*/