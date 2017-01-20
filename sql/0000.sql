-- Single entry table to store stats
CREATE TABLE stats (
	DatabaseVersion	INT
);

-- Table containing all players
CREATE TABLE players (
	ID INTEGER PRIMARY KEY AUTOINCREMENT,
	SocialClubName VARCHAR(255) NOT NULL UNIQUE,
	Password VARCHAR(255) NOT NULL,
	Level INTEGER DEFAULT 1,
	Cash INTEGER DEFAULT 1000,
	PositionX REAL,
	PositionY REAL,
	PositionZ REAL,
	RotationX REAL,
	RotationY REAL,
	RotationZ REAL
);