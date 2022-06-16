USE AlumniTracker
GO

CREATE OR ALTER VIEW [Alumnis]
AS
SELECT [AlumniId],[StudentID], [Name], [EmployerName],
[FieldofEmployment], [YearGraduated], [Degree],
[Notes], [DateModified], [Address], [City], [State],
[Zip], [AspNetUsers].[Email], [AspNetUsers].[id] from AlumniUser
INNER JOIN AspNetUsers
ON AlumniUser.Id = AspNetUsers.id ;
GO

CREATE OR ALTER PROCEDURE GetAlumnis
AS
BEGIN
Select * from Alumnis
END
GO