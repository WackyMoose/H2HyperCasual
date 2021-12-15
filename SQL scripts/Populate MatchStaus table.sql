-- Populate the enum table MatchStatus with predefined values.

use Tanx
go

insert into [Tanx].[dbo].[MatchStatus] (StatusName) values 
('Lobby'),
('Ongoing'),
('Finished'),
('Stopped');