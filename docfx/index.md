# Welcome to OSRlib.NET!

OSRlib.NET is a .NET Core class library written in C# that provides an API for turn-based role-playing games (RPGs) in the [old school revival](https://en.wikipedia.org/wiki/Old_School_Revival) (OSR) style.

It's designed for use as the core rules engine of a computer role-playing game (CRPG) in the Dungeons & Dragons Basic/Expert (or *B/X*) flavor, and lends itself well to Bard's Tale-style turn-based game play.

Add your own UI to interact with the OSRlib.NET API and you have yourself a turn-based RPG.

## Object model

The object model of OSRlib is intended to be simple and intuitive. The following sections describe the primary classes and their relationships.

### Being

A [Being](../src/osrlib.CoreRules/Being.cs) is any living entity that can initiate interaction with another entity within the game world, or can be interacted with by another Being. Player characters (PCs), non-player characters (NPCs), and monsters are all **Beings**.

![OSRlib Being diagram](./images/being_sm.png)

### Party

A [Party](../src/osrlib.CoreRules/Party.cs) is a collection of **Beings**. Two Party objects are added to an [Encounter](#adventure-dungeon--encounter), whose Beings can then battle.

![OSRlib Party diagram](./images/party_sm.png)

### Adventure, Dungeon, & Encounter

An [Adventure](../src/osrlib.CoreRules/Adventure.cs) consists of one or more [Dungeons](../src/osrlib.CoreRules/Dungeon.cs). Each Dungeon is comprised of one or more [Encounters](../src/osrlib.CoreRules/Encounter).

An Encounter is typically populated with a monster [Party](#party). Adding a player Party to the Encounter starts the Encounter (begins the battle).

![OSRlib Adventure diagram](./images/adventure_sm.png)