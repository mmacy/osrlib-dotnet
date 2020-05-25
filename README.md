# OSRlib.NET [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

|Build status (master) |Docs|Reference|Package|
|:-|:-|:-|:-:|
| [![Build Status](https://marshallmacy.visualstudio.com/osrlib-dotnet/_apis/build/status/osrlib-dotnet?branchName=master)](https://marshallmacy.visualstudio.com/osrlib-dotnet/_build/latest?definitionId=1&branchName=master) | [Documentation](docs/README.md) | API reference | NuGet |

## What is OSRlib.NET?

The OSR library for .NET is a small set of .NET Core assemblies written in C# for use in creating turn-based role-playing games (RPGs) in the [old school revival](https://en.wikipedia.org/wiki/Old_School_Revival) (OSR) style.

OSRLib.NET provides an API for the core rules engine for a computer role-playing game (CRPG) in the Dungeons & Dragons Basic/Expert (or *B/X*) flavor, and lends itself well to Bard's Tale-style turn-based game play.

Use OSRLib.NET as the engine, add your own UI to interact with its API, and you have yourself a turn-based RPG.

## Prerequisites

- Visual Studio Code or another code editor
- .NET Core SDK 3.1+

## Install the package

```console
# NOT YET IMPLEMENTED - no NuGet package (yet)
nuget install Osrlib.Net -OutputDirectory packages
```

## Getting started

OSRlib.NET provides an object model representing well-known RPG entities like adventures, dungeons, beings (player characters and monsters), encounters, and weapons. It has an event-based interaction model for manipulating these entities, their relationships, and state.

Examples of a few primary OSRlib operations follow:

- [Create a character](#create-a-character)
- [Stock the dungeon](#stock-the-dungeon)
- [Set up a battle](#set-up-a-battle)
- [Start the battle](#start-the-battle)

### Create a character

<!-- START SECTION_CREATE_A_CHARACTER -->
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
<!-- END SECTION_CREATE_A_CHARACTER -->

### Stock the dungeon

<!-- START SECTION_STOCK_THE_DUNGEON -->
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
<!-- END SECTION_STOCK_THE_DUNGEON -->

### Set up a battle

The `Encounter`, just like most of the top-level entities in OSRlib, exposes several events to help you update your game's UI when those events occur.

<!-- START SECTION_BATTLE_SETUP -->
```csharp
#region SECTION_BATTLE_SETUP
// OSRlib is heavily event-driven and most major entities have public events. Subscribe to events
// like these to change the state of your UI and/or prompt the player for action (such as selecting
// a target to attack during an encounter).
encounter.EncounterStarted += (sender, eventArgs) =>
    {
        Console.WriteLine($"Encounter has started! Monsters:\r\n{((Encounter)sender).EncounterParty}");
    };

// Example of subscribing to an event that you might use to update the UI state to notify the layer
// or make some other changes within your application.
encounter.EncounterEnded += (sender, eventArgs) =>
    {
        Encounter enc = sender as Encounter;
        if (enc.AdventuringParty.IsAlive)
        {
            Console.WriteLine("Your party has won the battle!");
        }
        else if (enc.EncounterParty.IsAlive)
        {
            Console.WriteLine("Sorry, your party has been vanquished.");
        }
    };
```
<!-- END SECTION_BATTLE_SETUP -->

### Start the battle

Finally we're ready to let the adventuring party (currently comprised of only one character, *Blarg the Destroyer*) and the encounter party (the evil orcs) battle to the death.

<!-- START SECTION_BATTLE_START -->
```csharp
// Encounters can be set to auto-resolve the battle. This is OPTIONAL! In your game, you'd not
// typically set this to true, and instead allow your players to select a target(s) for a character.
encounter.AutoBattleEnabled = true;

// Add the adventuring party to the encounter
encounter.SetAdventuringParty(playerParty);

// Subscribe to some events on the combatants so we can respond to things that happen to them.
List<Being> combatants = encounter.AdventuringParty.Members.Concat(encounter.EncounterParty.Members).ToList();
foreach (Being combatant in combatants)
{
    combatant.SelectedAsTarget += (s, e) =>
        {
            Being attackedBeing = s as Being;
            BeingTargetingEventArgs args = e as BeingTargetingEventArgs;
            Console.WriteLine($"{attackedBeing.Name} is being targeted by {e.TargetingBeing.Name} with their {e.TargetingBeing.ActiveWeapon.Name}...");
        };
    combatant.Killed += (s, e) =>
        {
            Console.WriteLine($"{((Being)s).Name} was killed!");
        };
}

// Start the battle. This will fire the EncounterStarted event we subscribed to
// above, and since we set this encounter to resolve all combat automatically with
// AutoBattleEnabled, each member of both parties takes turns attacking each other
// until one side has been defeated.
encounter.StartEncounter();
```
<!-- END SECTION_BATTLE_START -->

### View event output

The battle resolves fully (because we set `Encounter.AutoBattleEnabled = true`) and, because we subscribed to a few `Being` and `Encounter` events, we can see what transpires during the battle:

```console
Encounter has started! Monsters:
[0] Orc Captain	Hit points: 10
[1] Orc	Hit points: 4

Orc is being targeted by Blarg the Destroyer with their Long Sword + 1...
Orc was killed!
Blarg the Destroyer is being targeted by Orc Captain with their Fists...
Orc Captain is being targeted by Blarg the Destroyer with their Long Sword + 1...
Blarg the Destroyer is being targeted by Orc Captain with their Fists...
Orc Captain is being targeted by Blarg the Destroyer with their Long Sword + 1...
Your party has won the battle!
Orc Captain was killed!
```

## Next steps

This was quick intro to a few of the primary types in OSRlib, as well as how those types can interact.

If you'd like to see the full code sample for the preceding snippets, see [`src/osrlib.Tests/ReadMeTests.cs`](src/osrlib.Tests/ReadMeTests.cs).

And be sure to check out the [API reference (NOT YET PUBLISHED)](404.md) and the rest of the [documentation (CURRENTLY MINIMAL)](docs/README.md).

Have fun!
