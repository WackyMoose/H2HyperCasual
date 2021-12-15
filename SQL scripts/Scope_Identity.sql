use Tanx
go

INSERT INTO [Tanx].[dbo].[Players]
                    (PlayerName, Kills, Deaths)
                    VALUES ('TestUser', 0, 0);
 
declare @id int = (SELECT CAST(scope_identity() AS int))

print @id;
