# BossManager
A tShock plugin for Terraria that adds more Boss Management Tools. [Download](https://github.com/Ozz5581/BossManager/releases/download/v1.1.0/BossManager.dll)

## How to Install
- 1 - Download the Plugin File [Here!](https://github.com/Ozz5581/BossManager/releases/download/v1.1.0/BossManager.dll)
- 2 - Drop the `BossManager.dll` File into the `ServerPlugins` Folder
- 3 - Start your TShock Server

## Features
- Disable Specific Bosses
- Prevent Players to Join during a Boss Fight
- Adds Useful Boss Related Commands

## Commands & Permissions 
UndoBoss Command
- Command Aliases: ` /undoboss `, ` /uboss `
- Command Syntax: ` /undoboss <boss name or alias> `
- Description: Toggle a Bosses Defeated State
- Permission: ` bossmgr.undoboss `

ListBoss Command: 
- Command Aliases: ` /listboss `, ` /lboss ` , ` /bosses `
- Command Syntax: ` /listboss <boss|event> `
- Description: List Defeated Bosses
- Permission: ` bossmgr.listboss `

BossManager Reload Command: 
- Command Aliases: ` /bossrel `
- Description: Reload the BossManager Plugin
- Permission: ` bossmgr.reload `

## How to Enable/Disable a Boss
- Open the BossManager Config File at `/tshock/BossManagerConfig.json`
- Change `true` to `false` or vise versa to change a setting
- Type ` /bossrel ` in the chat to reload the your configurations 

## Configuration File Options

AllowJoinDuringBoss
- Default: ` true `
- Description: Allow Players to Join the Server during a Boss Fight

PreventIllegalBoss
- Default: ` false `
- Description: Prevent Bosses from being spawned illegally (eg. Plantera before WoF)

RequiredPlayersforBoss
- Default: ` 1 `
- Description: The required amount of online players needed to spawn a Boss
- Note - This setting doesn't apply to Bosses that have already been defeated)

## Boss Aliases

King Slime
- ` kingslime `
- ` king `
- ` ks `

Eye of Cthulhu: 
- ` eyeofcthulhu `
- ` eye `
- ` eoc `

Brain of Cthulhu / Eater of Worlds:
- ` evilboss `
- ` boc `
- ` eow `
- ` eaterofworlds `
- ` brainofcthulhu `
- ` brain `
- ` eater `

Skeletron
- ` skeletron `
- ` sans `

Queen Bee
- ` queenbee `
- ` qb `

Hardmode / Wall of Flesh
- ` hardmode `
- ` wallofflesh `
- ` wof `
Queen Slime
- ` queenslime `
- ` qs `

The Destroyer
- ` thedestroyer `
- ` mech1 `
- ` destroyer `

The Twins
- ` thetwins `
- ` mech2 `
- ` twins `

Skeletron Prime
- ` skeletronprime `
- ` mech3 `
- ` prime `

Plantera
- ` plantera `

Golem
- ` golem `

Duke Fishron
- ` dukefishron `
- ` duke `
- ` fishron `

Lunatic Cultist
- ` lunaticcultist `
- ` cultist `
- ` lunatic `

Empress of Light
- ` empressoflight `
- ` empress `
- ` eol `

Moonlord
- ` moonlord `
- ` ml `

----

Plugin Last Tested on Terraria v1.4.3.6
