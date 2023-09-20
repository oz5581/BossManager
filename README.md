
# BossManager
A TShock plugin for Terraria which adds more Boss Management Tools. [Download](https://github.com/Ozz5581/BossManager/releases/download/v1.2.0/BossManager.dll)

## How to Install
- Download the Plugin file [here!](https://github.com/Ozz5581/BossManager/releases/download/v1.2.0/BossManager.dll)
- Drop the `BossManager.dll` File into the `ServerPlugins` folder
- Start your TShock server

## Features (Can be modified in the config file)
- Prevent players from joining while a boss is active
- Prevent specific bosses from spawning
- Require specific amount of players online to spawn a boss
- More boss related commands

## Commands & Permissions 
Undo Boss Command
- Command Aliases: ` /undoboss `, ` /uboss `
- Command Syntax: ` /undoboss <boss name / alias> `
- Description: Toggle a Bosses Defeated State
- Permission: ` bossmgr.undoboss `

List Boss Command: 
- Command Aliases: ` /listboss `, ` /lboss ` , ` /bosses `
- Description: List All Defeated Bosses and Events
- Permission: ` bossmgr.listboss `

Enable Boss Command: 
- Command Aliases: ` /enableboss `, ` /enblb ` 
- Description: Allow / Prevent a specific boss to spawn
- Permission: ` bossmgr.enableboss `
- Bosses can also  be changed manually via the config file!

Boss Mgr Reload Command: 
- Command Aliases: ` /bossrel `
- Description: Reload the BossMgr Plugin
- Permission: ` bossmgr.reload `

## How to Prevent/Allow a Boss
#### Method 1
- Open the Config file at `/tshock/BossManagerConfig.json`
- Change the specified boss enabled state to `true` to `false`
- Use  ` /bossrel ` in the chat to reload the your configurations 
#### Method 2
- Use the ` /enableboss ` command followed by a boss alias

## Configuration File Options

AllowJoinDuringBoss
- Default: ` true `
- Description: Allow players to join the server while a boss is active in the world.

PreventIllegalBoss (Experimental)
- Default: ` false `
- Description: Prevent bosses from spawning at the wrong game progression state (Eg. Spawning Plantera before Wall of Flesh is defeated)
- For now, this configuration option only applies to bosses that are *Hard mode before Hard mode*

RequiredPlayersforBoss
- Default: ` 1 `
- Description: The required amount of players needed online in order to spawn a boss
- This configuration option doesn't apply to bosses that have already been defeated

## Boss Aliases (For command identifiers)

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
- ` destroyer `

The Twins
- ` thetwins `
- ` twins `

Skeletron Prime
- ` skeletronprime `
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

Last Tested: 19/9/2023 (Terraria 1.4.4.9)
