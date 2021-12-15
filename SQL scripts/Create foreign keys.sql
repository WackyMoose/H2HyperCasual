-- Version 1.0

use Tanx
go

alter table [PlayerMatches]
add foreign key (PlayerId) references Players(Id);
go

alter table [PlayerMatches]
add foreign key (MatchId) references Matches(Id);
go

alter table [Users]
add foreign key (PlayerId) references Players(Id);
go

alter table [Matches]
add foreign key (WinnerPlayerId) references Players(Id);
go

alter table [Matches]
add foreign key (Id) references MatchStatus(Id);

alter table [MatchKills]
add foreign key (Id) references Matches(Id);