ALTER TABLE Certificates
ADD FOREIGN KEY ([OrganizationId]) REFERENCES Organizations(OrganizationId);

ALTER TABLE Certificates
ADD FOREIGN KEY ([UserId]) REFERENCES Users(UserId);

ALTER TABLE Portfolio_Certificates
ADD FOREIGN KEY ([CertificateId]) REFERENCES Certificates(CertificateId);

ALTER TABLE Portfolio_Certificates
ADD FOREIGN KEY ([PortfolioId]) REFERENCES Portfolios(PortfolioId);

ALTER TABLE Certificate_Categories
ADD FOREIGN KEY ([CertificateId]) REFERENCES Certificates(CertificateId);

ALTER TABLE Certificate_Categories
ADD FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId);

ALTER TABLE Categories
ADD FOREIGN KEY (UserId) REFERENCES Users(UserId);

ALTER TABLE Reports
ADD FOREIGN KEY (CertificateId) REFERENCES Certificates(CertificateId);

ALTER TABLE Reports
ADD FOREIGN KEY (UserId) REFERENCES Users(UserId);

ALTER TABLE Portfolios
ADD FOREIGN KEY ([UserId]) REFERENCES Users(UserId);

ALTER TABLE Users
ADD FOREIGN KEY ([RoleId]) REFERENCES Roles(RoleId);

ALTER TABLE Role_Permissions
ADD FOREIGN KEY ([RoleId]) REFERENCES Roles(RoleId);

ALTER TABLE Role_Permissions
ADD FOREIGN KEY ([PermissionId]) REFERENCES [Permissions](PermissionId);

ALTER TABLE UserLogs
ADD FOREIGN KEY ([UserId]) REFERENCES Users(UserId);

/* 08/02/20221 - TRIHP */
ALTER TABLE Certificate_User
ADD FOREIGN KEY ([UserId]) REFERENCES Users(UserId);

ALTER TABLE Certificate_User	
ADD FOREIGN KEY ([CertificateId]) REFERENCES Certificates(CertificateId);

ALTER TABLE Transcripts
ADD FOREIGN KEY ([UserId]) REFERENCES Users(UserId);

ALTER TABLE CertificateContents
ADD FOREIGN KEY ([CertificateId]) REFERENCES Certificates(CertificateId);

