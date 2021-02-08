ALTER TABLE Certificates
ADD FOREIGN KEY ([OrganizationId]) REFERENCES Organizations(OrganizationId);

ALTER TABLE Certificates
ADD FOREIGN KEY ([UserId]) REFERENCES Users(UserID);

ALTER TABLE Portfolio_Certificates
ADD FOREIGN KEY ([CertificateId]) REFERENCES Certificates(CertificateID);

ALTER TABLE Portfolio_Certificates
ADD FOREIGN KEY ([PortfolioId]) REFERENCES Portfolios(PortfolioId);

ALTER TABLE Certificate_Categories
ADD FOREIGN KEY ([CertificateId]) REFERENCES Certificates(CertificateID);

ALTER TABLE Certificate_Categories
ADD FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId);

ALTER TABLE Categories
ADD FOREIGN KEY (UserID) REFERENCES Users(UserID);

ALTER TABLE Reports
ADD FOREIGN KEY (CertificateId) REFERENCES Certificates(CertificateID);

ALTER TABLE Reports
ADD FOREIGN KEY (UserId) REFERENCES Users(UserID);

ALTER TABLE Portfolios
ADD FOREIGN KEY ([UserId]) REFERENCES Users(UserID);


-----
ALTER TABLE Users
ADD FOREIGN KEY ([UserID]) REFERENCES Roles(RoleId);

ALTER TABLE Role_Permissions
ADD FOREIGN KEY ([RoleId]) REFERENCES Roles(RoleId);

ALTER TABLE Role_Permissions
ADD FOREIGN KEY ([PermissionId]) REFERENCES [Permissions](PermissionId);

ALTER TABLE UserLogs
ADD FOREIGN KEY ([UserId]) REFERENCES Users(UserID);