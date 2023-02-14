# OSRlib.NET

|License|Build status (main) |Docs|Reference|Package|
|:-|:-|:-|:-|:-:|
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)| [![Build Status](https://dev.azure.com/marshallmacy/osrlib-dotnet/_apis/build/status/osrlib-dotnet?branchName=main)](https://marshallmacy.visualstudio.com/osrlib-dotnet/_build/latest?definitionId=1&branchName=main) | [Documentation][docs] | [API reference][api-ref] | [NuGet](https://www.nuget.org/packages/osrlib.Core) |

![OSRlib.NET logo](https://raw.githubusercontent.com/mmacy/osrlib-dotnet/d0260ced0b34194121a220ab6f4b596806af2c50/docs/images/logo-osr-128x128.png)

OSRlib.NET is a .NET Core class library written in C# that you can use as the game mechanics engine for a turn-based computer role-playing game (CRPG).

The `osrlib.Core` object model and API were designed with the original *Bard's Tale* and similar CRPGs in mind. The library is appropriate for use in any game modeled after the *Dungeons & Dragons* Basic/Expert (or *B/X*) edition or other tabletop RPG in the [Old School Renaissance](https://en.wikipedia.org/wiki/Old_School_Renaissance) (OSR) style.

Add your own UI (or even CLI) that talks to the OSRlib.NET API, and you've got yourself a turn-based RPG!

> :warning: OSRlib.NET is early in development and several critical systems have yet to be built. To get an idea of what's missing, [check out the open issues](https://github.com/mmacy/osrlib-dotnet/issues?q=is%3Aissue+is%3Aopen).

## Prerequisites

- .NET SDK 7.0+

## Install the package

Run this `dotnet` CLI command to add a reference to your project:

```bash
dotnet add package osrlib.Core --version 0.0.2-alpha
```

## Getting started

The OSRlib.NET object model represents well-known RPG entities like adventures, dungeons, beings (player characters and monsters), encounters, and weapons. It has an event-based interaction model for manipulating these entities, their relationships, and state.

Any game you build with OSRlib.NET will include at least these four core operations, presented here with example code and in typical order of operation:

- [Create a player character](#create-a-character)
- [Stock the dungeon](#stock-the-dungeon) with monsters and encounters
- [Subscribe to battle events](#subscribe-to-battle-events)
- [Start the battle](#start-the-battle)

> :information_source: TIP: You can see these code snippets in context in OSRlib.NET's test project: [_src/osrlib.Tests/ReadMeTests.cs_](src/osrlib.Tests/ReadMeTests.cs)

### Create a character

```csharp
// Get a ten-sided die ready
DiceRoll roll = new DiceRoll(new DiceHand(1, DieType.d10));

// Roll up a fighter-type character
Being fighter = new Being
{
    Name = "Blarg the Destructor",
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
    DamageDie = new DiceHand(1, DieType.d8)
};
magicSword.AttackModifiers.Add(new Modifier { ModifierSource = magicSword, ModifierValue = 1 });
magicSword.DamageModifiers.Add(new Modifier { ModifierSource = magicSword, ModifierValue = 1 });
fighter.ActiveWeapon = magicSword;

// Now, add the fighter to the player's party
Party playerParty = new Party();
playerParty.AddPartyMember(fighter);
```

### Stock the dungeon

```csharp
Dungeon dungeon = new Dungeon();

// Create some monsters for an encounter
Being goblin1 = new Being
{
    Name = "Goblin Chieftain",
    Defense = 10,
    HitPoints = 10,
    MaxHitPoints = 10
};
goblin1.RollAbilities();

Being goblin2 = new Being
{
    Name = "Goblin",
    Defense = 5,
    HitPoints = 4,
    MaxHitPoints = 4
};
goblin2.RollAbilities();

// Add the goblins to the monster party
Party monsterParty = new Party();
monsterParty.AddPartyMember(goblin1);
monsterParty.AddPartyMember(goblin2);

// Add the monsters to an encounter
Encounter encounter = new Encounter
{
    EncounterParty = monsterParty,
    Position = new GamePosition(10, 10)
};

// Add the encounter to the dungeon
dungeon.Encounters.Add(encounter);
```

### Subscribe to battle events

The `Encounter`, like most top-level entities in OSRlib, exposes events to notify subscribers of actions it performs and actions performed on it.

Subscribe to events like these and use them as triggers to update your game's user interface or perform other runtime actions.

```csharp
// OSRlib is heavily event-driven and most top-level classes expose public events. Determine when and
// how to change the state of your game at runtime by subscribing to events exposed such objects. For
// example, to know when to prompt for target selection or play a sound when a monster is killed.
encounter.EncounterStarted += (sender, eventArgs) =>
    {
        Console.WriteLine($"Encounter has started! Monsters:\r\n{((Encounter)sender).EncounterParty}");
    };

// Example of subscribing to an event that you might use to update the UI state to notify the player or
// make some other change in your application at runtime.
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

### Start the battle

We're now ready to let the adventuring party (currently comprised of only one character, *Blarg the Destructor*) and the encounter party (the evil orcs) battle to the death.

<!-- START SECTION_BATTLE_START -->
```csharp
// You can set encounters auto-resolve the battle as is done in this example. In a typical game, however,
// you wouldn't enable auto-battle, and instead would prompt your player to select a target(s) or perform
// some other action before proceeding with the next battle step.
encounter.AutoBattleEnabled = true;

// Add the adventuring party to the encounter
encounter.SetAdventuringParty(playerParty);

// Subscribe to some events on the combatants so we can respond to things
// that happen to them.
List<Being> combatants = encounter.AdventuringParty.Members.Concat(encounter.EncounterParty.Members).ToList();
foreach (Being combatant in combatants)
{
    combatant.SelectedAsTarget += (s, e) =>
        {
            Being attackedBeing = s as Being;
            BeingTargetingEventArgs args = e as BeingTargetingEventArgs;

            Console.WriteLine($"{e.TargetingBeing} attacks {attackedBeing} with their {e.TargetingBeing.ActiveWeapon}...");
        };
    combatant.ActionPerformed += (s, e) =>
        {
            GameActionEventArgs actionArgs = e as GameActionEventArgs;
            GameAction action = actionArgs.Action;

            if (action.Victor.Equals(combatant))
            {
                Console.WriteLine($"{combatant} rolled a {action.AttackRoll} and hit for {action.DamageRoll} points of damage.");
            }
            else
            {
                Console.WriteLine($"{combatant} rolled a {action.AttackRoll} and missed.");
            }
        };
    combatant.Killed += (s, e) =>
        {
            Console.WriteLine($"{((Being)s).Name} was killed!");
        };
}

// Start the battle. This will fire the EncounterStarted event we subscribed to above, and since we set
// this encounter to resolve all combat automatically with AutoBattleEnabled, each member of both parties
// takes turns attacking each other until one side has been defeated.
encounter.StartEncounter();
```

### Display event data

The battle resolves fully (because we set `Encounter.AutoBattleEnabled = true`) and, because we subscribed `Being` and `Encounter` events, we can see what transpires during the battle:

```console
Encounter has started! Monsters:
[0] Goblin Chieftain    Hit points: 10
[1] Goblin      Hit points: 4

Blarg the Destructor (18/18) attacks Goblin (4/4) with their Long Sword + 1...
Goblin was killed!
Blarg the Destructor (18/18) rolled a 9 (1d20+1) and hit for 6 (1d8+1) points of damage.
Goblin Chieftain (10/10) attacks Blarg the Destructor (18/18) with their Fists...
Goblin Chieftain (10/10) rolled a 16 (1d20-1) and hit for 0 (1d2-1) points of damage.
Blarg the Destructor (18/18) attacks Goblin Chieftain (10/10) with their Long Sword + 1...
Blarg the Destructor (18/18) rolled a 15 (1d20+1) and hit for 7 (1d8+1) points of damage.
Goblin Chieftain (3/10) attacks Blarg the Destructor (18/18) with their Fists...
Goblin Chieftain (3/10) rolled a 15 (1d20-1) and hit for 1 (1d2-1) points of damage.
Blarg the Destructor (17/18) attacks Goblin Chieftain (3/10) with their Long Sword + 1...
Your party has won the battle!
Goblin Chieftain was killed!
Blarg the Destructor (17/18) rolled a 13 (1d20+1) and hit for 3 (1d8+1) points of damage.
```

## Next steps

This README was a quick intro to a few of the types and operations available in OSRlib. Here are some other resources to help you use OSRlib.NET in your turn-based RPG:

- [API reference][api-ref]
- [Library documentation][docs]
- [OSRlib tests](src/osrlib.Tests)
- [ASP.NET Web API for OSRlib.NET](https://github.com/mmacy/osrlib-demo-api) (a work-in-progress code sample)

Have fun!

[docs]: https://mmacy.github.io/osrlib-dotnet
[api-ref]: https://mmacy.github.io/osrlib-dotnet/api/osrlib.Core.html