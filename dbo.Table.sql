CREATE TABLE [dbo].[MemberList]
(
	[MemeberID] INT NOT NULL PRIMARY KEY, 
    [FirstName] VARCHAR(50) NOT NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [DOB] DATE NOT NULL, 
    [Rank] VARCHAR(50) NOT NULL
)
