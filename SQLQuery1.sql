/*To create database*/
--create database APIDB;

/*To create tables*/
/*create table Designation (
DesignationId int primary key identity(1,1) not null,
DesignationName varchar(500) 
);

create table Department(
DepartmentId int primary key identity(1,1) not null,
DepartmentName varchar(500) 
);*/


create table Employee(
EmployeeId int primary key identity(1,1) not null,
FirstName varchar(500),
LastName varchar(500),
Email varchar(500),
PhoneNumber varchar(500),
DepartmentId int,
DesignationId int,
foreign key (DepartmentId) References dbo.Department(DepartmentId) ON DELETE SET NULL, --set null value whenever department is deleted
foreign key (DesignationId) References dbo.Designation(DesignationId) ON DELETE SET NULL
);