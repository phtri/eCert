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
delete from User_Role where UserId = 62
delete from [User] where UserId = 62

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

delete from Campus
delete from EducationSystem



delete from CertificateContent
delete from Report
delete from [Certificate]

select * from [User] where UserId = 56