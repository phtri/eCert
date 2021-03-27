delete from CertificateContent 
delete from Certificate 

--- Certificate
select * from Certificate
select * from CertificateContent

--User
select * from [User]
select * from User_Role
select * from Role

--EducationSystem
select * from EducationSystem
select * from Campus

--Signature
select * from Signature
select * from Signature_EducationSystem


select * from Certificate

SELECT * FROM CERTIFICATE C, [USER] U WHERE C.CERTIFICATEID = 25 AND C.ROLLNUMBER = U.ROLLNUMBER

SELECT * FROM CERTIFICATE WHERE ROLLNUMBER = 'HE130585' AND CERTIFICATENAME LIKE N'% Sáo %'

SELECT * FROM CERTIFICATE CERTIFICATENAME LIKE 'Java'


delete from CertificateContent
delete from Report
delete from [Certificate]