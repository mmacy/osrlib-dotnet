# OSRlib.NET [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

|Build status |Docs|Ref|Package|
|:-|:-|:-:|:-:|
| [![Build Status](https://marshallmacy.visualstudio.com/osrlib-dotnet/_apis/build/status/osrlib-dotnet?branchName=master)](https://marshallmacy.visualstudio.com/osrlib-dotnet/_build/latest?definitionId=1&branchName=master) | [Documentation](docs/README.md) | API reference | NuGet |

## What is OSRlib.NET?

The OSR library for .NET is a small set of .NET Core assemblies written in C# for use in creating turn-based role-playing games (RPGs) in the [old school revival](https://en.wikipedia.org/wiki/Old_School_Revival) (OSR) style.

OSRLib.NET provides an API for the core rules engine for a computer role-playing game (CRPG) in the Dungeons & Dragons Basic/Expert (or *B/X*) flavor, and lends itself well to Bard's Tale-style turn-based game play.

Use OSRLib.NET as the engine, add your own UI to interact with its API, and you have yourself a turn-based RPG.

## Getting started

OSRlib.NET provides an object model representing well-known RPG entities like adventures, dungeons, beings (player characters and monsters), encounters, and weapons. It has an event-based interaction model for manipulating these entities, their relationships, and state.

### Install the package

```console
nuget install Osrlib.Net -OutputDirectory packages
```

### Create a character

```csharp
// Get our ten-sided die ready
DiceRoll roll = new DiceRoll(new DiceHand(1, DieType.d10));

// Roll up a fighter-type character
Being fighter = new Being
{
    Name = "Blarg the Destroyer",
    Defense = roll.RollDice(),
    MaxHitPoints = roll.RollDice() + 10
};
fighter.HitPoints = fighter.MaxHitPoints;
fighter.RollAbilities();

// Give Blarg a sweet sword
Weapon magicSword = new Weapon
{
    Name = "Long Sword + 1",
    Description = "A finely crafted sword, its blade dimly glows.",
    Type = WeaponType.Melee,
    DamageDie = new DiceHand(1, DieType.d8),
};
fighter.ActiveWeapon = magicSword;

// Now, add the fighter to the player's party
Party playerParty = new Party();
playerParty.AddPartyMember(fighter);
```

### Stock the dungeon

```csharp
Dungeon dungeon = new Dungeon();

// Create some monsters for an encounter
Being orc1 = new Being
{
    Name = "Orc Captain",
    Defense = 8,
    HitPoints = 10,
    MaxHitPoints = 10
};

Being orc2 = new Being
{
    Name = "Orc",
    Defense = 5,
    HitPoints = 4,
    MaxHitPoints = 4
};

// Add the orcs to the monster party
Party monsterParty = new Party();
monsterParty.AddPartyMember(orc1);
monsterParty.AddPartyMember(orc2);

// Add the monsters to an encounter
Encounter encounter = new Encounter
{
    EncounterParty = monsterParty,
    Position = new GamePosition(10, 10)
};

// Add the encounter to the dungeon
dungeon.Encounters.Add(encounter);
```

### Battle!

Start the encounter...

```csharp
// Add the adventuring party to the encounter
encounter.SetAdventuringParty(playerParty);

// TODO: subscribe to encounter events

// Start the battle
encounter.StartEncounter();
```

TODO: Example of stepping the battle.
