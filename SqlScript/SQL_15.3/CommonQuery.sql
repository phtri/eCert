delete from CertificateContent 
delete from Certificate 


--- Certificate
select * from Certificate
select * from CertificateContent


--- User
select * from [User]
select * from User_Role
select * from Role

-- Insert Organization, Role, User
IN

ALTER TABLE CERTIFICATE
ADD SignatureId INT


select * from Certificate

SELECT * FROM CERTIFICATE C, [USER] U WHERE C.CERTIFICATEID = 25 AND C.ROLLNUMBER = U.ROLLNUMBER

SELECT * FROM CERTIFICATE WHERE ROLLNUMBER = 'HE130585' AND CERTIFICATENAME LIKE N'% S�o %'

SELECT * FROM CERTIFICATE CERTIFICATENAME LIKE 'Java'


