delete from Certificate_User
delete from CertificateContent
delete from Certificate
delete from [User] where AcademicEmail = 'hainnhe130585@fpt.edu.vn'

--- Certificate
select * from Certificate
select * from CertificateContent
select * from Certificate_User

--- User
select * from [User]
select * from User_Role

-- Insert Organization, Role, User

ALTER TABLE [USER]
DROP COLUMN RoleId
