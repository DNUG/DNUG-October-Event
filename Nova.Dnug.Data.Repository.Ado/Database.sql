use DNUG
go

if object_id('Clear', 'P') is not null drop procedure Clear
go
create procedure Clear as
begin

	if object_id('Students', 'U') is not null drop table Students
	if object_id('Courses', 'U') is not null drop table Courses
	if object_id('Faculties', 'U') is not null drop table Faculties
	if object_id('Universities', 'U') is not null drop table Universities
	if object_id('Addresses', 'U') is not null drop table Addresses
	if object_id('Forenames', 'I') is not null drop index Forenames on Students

	create table Addresses
	(
		Id uniqueidentifier primary key not null,
		HouseNameOrNumber varchar(100) not null,
		Street varchar(100) not null,
		City varchar(100) not null,
		State varchar(100) not null,
		ZipCode varchar(100) not null
	)

	create table Universities
	(
		Id uniqueidentifier primary key not null,
		Name varchar(100) not null,
		LocationId uniqueidentifier not null foreign key references Addresses(Id)
	)
	
	create table Faculties
	(
		Id uniqueidentifier primary key not null,
		Name varchar(100) not null,
		UniversityId uniqueidentifier not null foreign key references Universities(Id) on delete cascade,
		AddressId uniqueidentifier not null foreign key references Addresses(Id)
	)

	create table Courses
	(
		Id uniqueidentifier primary key not null,
		Code int not null,
		Description varchar(100) not null,
		StartDate date not null,
		EndDate date not null,
		FacultyId uniqueidentifier not null foreign key references Faculties(Id) on delete cascade
	)

	create table Students
	(
		Id uniqueidentifier primary key not null,
		StudentNumber uniqueidentifier not null,
		Forename varchar(100) not null,
		Surname varchar(100) not null,
		AddressId uniqueidentifier not null foreign key references Addresses(Id),
		DateOfBirth date not null,
		CourseId uniqueidentifier not null foreign key references Courses(Id) on delete cascade
	)

	create nonclustered index [Forenames] on Students(Forename)
end
go

--create type UniversityTableType as table (Id uniqueidentifier, Name varchar(100), LocationId uniqueidentifier)
--create type AddressTableType as table (Id uniqueidentifier,	HouseNameOrNumber varchar(100),	Street varchar(100), City varchar(100),	State varchar(100),	ZipCode varchar(100))
--create type FacultyTableType as table (Id uniqueidentifier, Name varchar(100), UniversityId uniqueidentifier, AddressId uniqueidentifier)
--create type CourseTableType as table (Id uniqueidentifier, Code int, Description varchar(100), StartDate date, EndDate date, FacultyId uniqueidentifier)
--create type StudentTableType as table (Id uniqueidentifier, StudentNumber uniqueidentifier, Forename varchar(100), Surname varchar(100), AddressId uniqueidentifier, DateOfBirth date, CourseId uniqueidentifier)
--go

if object_id('CreateUniversity', 'P') is not null drop procedure CreateUniversity
go
create procedure CreateUniversity
(
	@Universities dbo.UniversityTableType readonly,
	@Faculties dbo.FacultyTableType readonly,
	@Courses dbo.CourseTableType readonly,
	@Students dbo.StudentTableType readonly,
	@Addresses dbo.AddressTableType readonly
)
as begin

	insert into Addresses
	select * from @Addresses

	insert into Universities
	select * from @Universities

	insert into Faculties
	select * from @Faculties

	insert into Courses
	select * from @Courses

	insert into Students
	select * from @Students
end
go

if object_id('RetrieveUniversity', 'P') is not null drop procedure RetrieveUniversity
go
create procedure RetrieveUniversity
(
	@Id uniqueidentifier
)
as begin

	select 
		u.*, 
		ua.Id as AddressId,
		ua.HouseNameOrNumber,
		ua.Street,
		ua.City,
		ua.State,
		ua.ZipCode 
	from Universities u
		inner join Addresses ua on u.LocationId = ua.Id
	where u.Id = @Id
	
	select 
		f.*, 
		fa.HouseNameOrNumber,
		fa.Street,
		fa.City,
		fa.State,
		fa.ZipCode 
	from Universities u
		inner join Addresses ua on u.LocationId = ua.Id
		inner join Faculties f on u.Id = f.UniversityId
		inner join Addresses fa on f.AddressId = fa.Id
	where u.Id = @Id

	select 
		c.* 
	from Universities u
		inner join Addresses ua on u.LocationId = ua.Id
		inner join Faculties f on u.Id = f.UniversityId
		inner join Addresses fa on f.AddressId = fa.Id
		inner join Courses c on f.Id = c.FacultyId
	where u.Id = @Id

	select 
		s.*, 
		sa.HouseNameOrNumber,
		sa.Street,
		sa.City,
		sa.State,
		sa.ZipCode 
	from Universities u
		inner join Addresses ua on u.LocationId = ua.Id
		inner join Faculties f on u.Id = f.UniversityId
		inner join Addresses fa on f.AddressId = fa.Id
		inner join Courses c on f.Id = c.FacultyId
		inner join Students s on c.Id = s.CourseId
		inner join Addresses sa on s.AddressId = sa.Id
	where u.Id = @Id
end 
go


if object_id('UpdateUniversity', 'P') is not null drop procedure UpdateUniversity
go
create procedure UpdateUniversity
(
	@Id uniqueidentifier,
	@Name varchar(100)
)
as begin
	update Universities set Name = @Name where Id = @Id
end
go

if object_id('DeleteUniversity', 'P') is not null drop procedure DeleteUniversity
go
create procedure DeleteUniversity
(
	@Id uniqueidentifier
)
as begin
	delete from Universities where Id = @Id
end
go

if object_id('GetUniversityCountByForename', 'P') is not null drop procedure GetUniversityCountByForename
go
create procedure GetUniversityCountByForename
(
	@Forename varchar(100),
	@Count int out
)
as begin
	select @Count = count (u.Id)
	from Universities u
		inner join Faculties f on u.Id = f.UniversityId
		inner join Courses c on f.Id = c.FacultyId
		inner join Students s on c.Id = s.CourseId
	where s.Forename = @Forename
end
go