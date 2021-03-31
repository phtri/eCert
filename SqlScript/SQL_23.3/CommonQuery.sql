delete from CertificateContent 
delete from Certificate 

--- Certificate
select * from Certificate
select * from CertificateContent

--User
select * from [User]
select * from User_Role
select * from Role


--delete user
delete from User_Role where UserId = 60
delete from [User] where UserId = 60

alter table [User]
add MemberCode varchar(50)

--EducationSystem
select * from EducationSystem
select * from Campus

--Signature
select * from Signature
select * from Signature_EducationSystem

delete from Signature_EducationSystem
delete from Signature

select * from Certificate

SELECT * FROM CERTIFICATE C, [USER] U WHERE C.CERTIFICATEID = 25 AND C.ROLLNUMBER = U.ROLLNUMBER

SELECT * FROM CERTIFICATE WHERE ROLLNUMBER = 'HE130585' AND CERTIFICATENAME LIKE N'% Sáo %'

SELECT * FROM CERTIFICATE CERTIFICATENAME LIKE 'Java'


delete from CertificateContent
delete from Report
delete from [Certificate]

select * from [User] where UserId = 56