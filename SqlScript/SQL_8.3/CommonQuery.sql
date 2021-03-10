delete from Certificate_User where CertificateId != 14
delete from CertificateContent where CertificateId != 14
delete from Certificate where CertificateId != 14
delete from [User] where AcademicEmail = 'hainnhe130585@fpt.edu.vn'

--- Certificate
select * from Certificate
select * from CertificateContent
select * from Certificate_User

--- User
select * from [User]
select * from User_Role

-- Insert Organization, Role, User
IN



select * from Certificate

SELECT * FROM CERTIFICATE C, [USER] U WHERE C.CERTIFICATEID = 4 AND C.ROLLNUMBER = U.ROLLNUMBER