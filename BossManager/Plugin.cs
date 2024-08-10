using TerrariaApi.Server;
using TShockAPI;
using Terraria;
using Terraria.ID;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace BossManager
{
    [ApiVersion(2, 1)]
    public partial class Plugin : TerrariaPlugin
    {
        public override void Initialize()
        {
            ServerApi.Hooks.NpcSpawn.Register(this, OnNpcSpawn);
            ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
            ServerApi.Hooks.ServerBroadcast.Register(this, OnServerBroadcast);

            GetDataHandlers.PlayerUpdate += OnPlayerUpdate;

            Commands.ChatCommands.Add(new Command("bossmgr.reload", ReloadCommand, "bossrel")
            {
                HelpText = "Reload Boss Manager."
            });

            Commands.ChatCommands.Add(new Command("bossmgr.undoboss", UndoBossCommand, "undoboss", "uboss")
            {
                HelpText = "Toggle a boss defeated state."
            });

            Commands.ChatCommands.Add(new Command("bossmgr.listboss", ListBossCommand, "listboss", "lboss", "bosses")
            {
                HelpText = "List defeated bosses and events."
            });

            Commands.ChatCommands.Add(new Command("bossmgr.enableboss", EnableBossCommand, "enableboss", "enblb")
            {
                HelpText = "Toggle a boss enabled state."
            });
        }

        private void OnPlayerUpdate(object? sender, GetDataHandlers.PlayerUpdateEventArgs args)
        {
            TSPlayer plr = args.Player;
            if (args.Control.IsUsingItem && IsSpawnerItem(plr.SelectedItem))
            {
                if (!IsBossSpawnable(GetBossNetIDFromSpawner(plr.SelectedItem.netID)))
                {
                    plr.SendErrorMessage("This boss is disabled!");

                    NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, NetworkText.FromLiteral(plr.SelectedItem.Name), plr.Index, plr.TPlayer.selectedItem);
                    NetMessage.SendData((int)PacketTypes.PlayerSlot, plr.Index, -1, NetworkText.FromLiteral(plr.SelectedItem.Name), plr.Index, plr.TPlayer.selectedItem);
                }
            }
        }

        private void OnServerBroadcast(ServerBroadcastEventArgs args)
        {
            string text = args.Message.ToString();

            if (text.EndsWith(" has awoken!"))
            {
                args.Message._mode = NetworkText.Mode.LocalizationKey;
                text = args.Message.ToString();

                string bossName = text[..text.IndexOf(" has awoken!")];

                foreach (NPC npc in Main.npc)
                {
                    if (npc.FullName.StartsWith(bossName) && npc.type == 0 && !npc.active)
                    {
                        args.Handled = true;
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.NpcSpawn.Deregister(this, OnNpcSpawn);
                ServerApi.Hooks.ServerJoin.Deregister(this, OnJoin);
                ServerApi.Hooks.ServerBroadcast.Deregister(this, OnServerBroadcast);

                GetDataHandlers.PlayerUpdate -= OnPlayerUpdate;
            }

            base.Dispose(disposing);
        }

        private void ReloadCommand(CommandArgs args)
        {
            Config = Config.Read();
            args.Player.SendInfoMessage("BossManager has been reloaded.");
        }

        public void OnJoin(JoinEventArgs args)
        {
            if (Config.AllowJoinDuringBoss) return;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].boss)
                    TShock.Players[args.Who].Disconnect("The in-game players must defeat the current boss\nbefore you can join.");
            }
        }

        private void ListBossCommand(CommandArgs args)
        {

            var BossList = new List<string>();
            {
                if (NPC.downedSlimeKing)
                    BossList.Add("King Slime");

                if (NPC.downedBoss1)
                    BossList.Add("Eye of Cthulhu");

                if (NPC.downedBoss2)
                {
                    if (WorldGen.crimson)
                        BossList.Add("Brain of Cthulhu");
                    else
                        BossList.Add("Eater of Worlds");
                }

                if (NPC.downedDeerclops)
                    BossList.Add("Deerclops");

                if (NPC.downedBoss3)
                    BossList.Add("Skeletron");

                if (NPC.downedQueenBee)
                    BossList.Add("Queen Bee");

                if (Main.hardMode)
                    BossList.Add("Wall of Flesh");

                if (NPC.downedQueenSlime)
                    BossList.Add("Queen Slime");

                if (NPC.downedMechBoss1)
                    BossList.Add("The Destroyer");

                if (NPC.downedMechBoss2)
                    BossList.Add("The Twins");

                if (NPC.downedMechBoss3)
                    BossList.Add("Skeletron Prime");

                if (NPC.downedPlantBoss)
                    BossList.Add("Plantera");

                if (NPC.downedGolemBoss)
                    BossList.Add("Golem");

                if (NPC.downedFishron)
                    BossList.Add("Duke Fishron");

                if (NPC.downedEmpressOfLight)
                    BossList.Add("Empress of Light");

                if (NPC.downedAncientCultist)
                    BossList.Add("Lunatic Cultist");

                if (NPC.downedMoonlord)
                    BossList.Add("Moon Lord");
            }
            var EventList = new List<string>();
            {
                if (NPC.downedGoblins)
                    EventList.Add("Goblin Army");

                if (NPC.downedPirates)
                    EventList.Add("Pirate Invasion");

                if (NPC.downedClown)
                    EventList.Add("Blood Moon");

                if (NPC.downedFrost)
                    EventList.Add("Frost Legion");

                if (NPC.downedMartians)
                    EventList.Add("Martian Invasion");

                if (NPC.downedHalloweenTree)
                    EventList.Add("Pumpkin Moon");

                if (NPC.downedChristmasTree)
                    EventList.Add("Frost Moon");

                if (NPC.downedTowerNebula && NPC.downedTowerSolar && NPC.downedTowerStardust && NPC.downedTowerVortex)
                    EventList.Add("The Pillars");
            }

            if (String.IsNullOrEmpty(String.Join(", ", BossList)))
                args.Player.SendInfoMessage("No bosses have been defeated so far...");
            else
                args.Player.SendInfoMessage($"[c/ffc500:Defeated Bosses:] {String.Join(", ", BossList)}");


            if (String.IsNullOrEmpty(String.Join(", ", EventList)))
                args.Player.SendInfoMessage("No events have been defeated so far...");
            else
                args.Player.SendInfoMessage($"[c/ffc500:Defeated Events:] {String.Join(", ", EventList)}");
        }

        private void UndoBossCommand(CommandArgs args)
        {
            string subcommand = args.Parameters.Count == 0 ? "undoboss" : args.Parameters[0].ToLower();

            switch (subcommand)
            {
                case "kingslime":
                case "king":
                case "ks":
                    {
                        NPC.downedSlimeKing = !NPC.downedSlimeKing;
                        args.Player.SendInfoMessage($"Set King Slime as {(NPC.downedSlimeKing ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                case "eyeofcthulhu":
                case "eye":
                case "eoc":
                    {
                        NPC.downedBoss1 = !NPC.downedBoss1;
                        args.Player.SendInfoMessage($"Set Eye of Cthulhu as {(NPC.downedBoss1 ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                case "evilboss":
                case "boc":
                case "eow":
                case "eaterofworlds":
                case "brainofcthulhu":
                case "brain":
                case "eater":
                    {
                        NPC.downedBoss2 = !NPC.downedBoss2;
                        args.Player.SendInfoMessage($"Set {(WorldGen.crimson ? "Brain of Cthulhu" : "Eater of Worlds")} as {(NPC.downedBoss2 ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                case "skeletron":
                case "sans":
                    {
                        NPC.downedBoss3 = !NPC.downedBoss3;
                        args.Player.SendInfoMessage($"Set Skeletron as {(NPC.downedBoss3 ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                case "queenbee":
                case "qb":
                    {
                        NPC.downedQueenBee = !NPC.downedQueenBee;
                        args.Player.SendInfoMessage($"Set Queen Bee as {(NPC.downedQueenBee ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                case "hardmode":
                case "wallofflesh":
                case "wof":
                    {
                        Main.hardMode = !Main.hardMode;
                        args.Player.SendInfoMessage($"Set Wall of Flesh (Hardmode) as {(Main.hardMode ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        args.Player.SendInfoMessage("Note: This is the same as the '/hardmode' command.");
                        return;
                    }
                case "queenslime":
                case "qs":
                    {
                        NPC.downedQueenSlime = !NPC.downedQueenSlime;
                        args.Player.SendInfoMessage($"Set Queen Slime as {(NPC.downedQueenSlime ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                case "mech1":
                case "thedestroyer":
                case "destroyer":
                    {
                        NPC.downedMechBoss1 = !NPC.downedMechBoss1;
                        args.Player.SendInfoMessage($"Set The Destroyer as {(NPC.downedMechBoss1 ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                case "mech2":
                case "thetwins":
                case "twins":
                    {
                        NPC.downedMechBoss2 = !NPC.downedMechBoss2;
                        args.Player.SendInfoMessage($"Set The Twins as {(NPC.downedMechBoss2 ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                case "mech3":
                case "skeletronprime":
                case "prime":
                    {
                        NPC.downedMechBoss3 = !NPC.downedMechBoss3;
                        args.Player.SendInfoMessage($"Set Skeletron Prime as {(NPC.downedMechBoss3 ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                case "plantera":
                    {
                        NPC.downedPlantBoss = !NPC.downedPlantBoss;
                        args.Player.SendInfoMessage($"Set Plantera as {(NPC.downedPlantBoss ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                case "golem":
                    {
                        NPC.downedGolemBoss = !NPC.downedGolemBoss;
                        args.Player.SendInfoMessage($"Set Golem as {(NPC.downedGolemBoss ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                case "duke":
                case "fishron":
                case "dukefishron":
                    {
                        NPC.downedFishron = !NPC.downedFishron;
                        args.Player.SendInfoMessage($"Set Duke Fishron as {(NPC.downedFishron ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                case "cultist":
                case "lunatic":
                case "lunaticcultist":
                    {
                        NPC.downedAncientCultist = !NPC.downedAncientCultist;
                        args.Player.SendInfoMessage($"Set Lunatic Cultist as {(NPC.downedAncientCultist ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }

                case "empress":
                case "eol":
                case "empressoflight":
                    {
                        NPC.downedEmpressOfLight = !NPC.downedEmpressOfLight;
                        args.Player.SendInfoMessage($"Set Empress of Light as {(NPC.downedEmpressOfLight ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                case "moonlord":
                case "ml":
                    {
                        NPC.downedMoonlord = !NPC.downedMoonlord;
                        args.Player.SendInfoMessage($"Set Moonlord as {(NPC.downedMoonlord ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
                        return;
                    }
                default:
                    {
                        args.Player.SendErrorMessage("Please specify which boss to toggle!");
                        args.Player.SendInfoMessage("eg. /undoboss king - toggle king slime");
                        return;
                    }
            }

        }

        private void EnableBossCommand(CommandArgs args)
        {
            string subcommand = args.Parameters.Count == 0 ? "undoboss" : args.Parameters[0].ToLower();

            switch (subcommand)
            {
                case "kingslime":
                case "king":
                case "ks":
                    ToggleBoss(ref Config.AllowKingSlime, args, "King Slime");
                    break;

                case "eyeofcthulhu":
                case "eye":
                case "eoc":
                    ToggleBoss(ref Config.AllowEyeOfCthulhu, args, "Eye of Cthulhu");
                    break;

                case "evilboss":
                case "boc":
                case "eow":
                case "eaterofworlds":
                case "brainofcthulhu":
                case "brain":
                case "eater":
                    ToggleBoss(ref Config.AllowEaterOfWorlds, args, WorldGen.crimson ? "Brain of Cthulhu" : "Eater of Worlds");
                    ToggleBoss(ref Config.AllowBrainOfCthulhu, args, "Brain of Cthulhu");
                    break;

                case "deerclops":
                case "deer":
                case "dc":
                    ToggleBoss(ref Config.AllowDeerclops, args, "Deerclops");
                    break;

                case "skeletron":
                case "sans":
                    ToggleBoss(ref Config.AllowSkeletron, args, "Skeletron");
                    break;

                case "queenbee":
                case "qb":
                    ToggleBoss(ref Config.AllowQueenBee, args, "Queen Bee");
                    break;

                case "hardmode":
                case "wallofflesh":
                case "wof":
                    ToggleBoss(ref Config.AllowWallOfFlesh, args, "Wall of Flesh/Hardmode");
                    break;

                case "queenslime":
                    ToggleBoss(ref Config.AllowQueenSlime, args, "Queen Slime");
                    break;

                case "twins":
                case "thetwins":
                case "ret":
                case "spaz":
                    ToggleBoss(ref Config.AllowTheTwins, args, "The Twins");
                    break;

                case "destroyer":
                case "thedestroyer":
                    ToggleBoss(ref Config.AllowTheDestroyer, args, "The Destroyer");
                    break;

                case "skeletronprime":
                case "prime":
                    ToggleBoss(ref Config.AllowSkeletronPrime, args, "Skeletron Prime");
                    break;
                case "plantera":
                    ToggleBoss(ref Config.AllowPlantera, args, "Plantera");
                    break;
                case "golem":
                    ToggleBoss(ref Config.AllowGolem, args, "Golem");
                    break;

                case "duke":
                case "fishron":
                case "dukefishron":
                    ToggleBoss(ref Config.AllowDukeFishron, args, "Duke Fishron");
                    break;

                case "eol":
                case "empress":
                case "empressoflight":
                    ToggleBoss(ref Config.AllowEmpressOfLight, args, "Empress of Light");
                    break;

                case "cultist":
                case "lunatic":
                case "lunaticcultist":
                    ToggleBoss(ref Config.AllowLunaticCultist, args, "Lunatic Cultist");
                    break;

                case "moonlord":
                case "ml":
                case "squid":
                    ToggleBoss(ref Config.AllowMoonLord, args, "Moonlord");
                    break;

                // Add other cases here...

                default:
                    args.Player.SendErrorMessage("Please specify boss to enable:");
                    args.Player.SendInfoMessage("Bosses have pre-set identifiers: eg, /enblb king, /enblb eoc, OR /enblb wof");
                    break;
            }

            void ToggleBoss(ref bool configField, CommandArgs args, string bossName)
            {
                configField = !configField;
                SaveConfigToFile(Config, Path.Combine(TShock.SavePath, "BossManager.json"));
                args.Player.SendInfoMessage($"[BossMGR] Identifier '{bossName}' is now set to {(configField ? "enabled" : "disabled")}");
            }

            void SaveConfigToFile(Config config, string filePath)
            {
                try
                {
                    // Serialize the configuration object to JSON
                    string json = JsonConvert.SerializeObject(config, Formatting.Indented);

                    // Write the JSON data to the specified file
                    File.WriteAllText(filePath, json);

                    // Optionally, you can provide feedback that the save was successful
                    args.Player.SendInfoMessage("BossMGR Configuration saved successfully.");
                    Console.WriteLine("BossMGR Configuration saved successfully.");
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that might occur during the save process
                    args.Player.SendInfoMessage("Error saving BossMGR configuration. Check logs for details");
                    Console.WriteLine($"Error saving BossMGR configuration: {ex.Message}");
                }
            }

        }

        public void OnNpcSpawn(NpcSpawnEventArgs args)
        {
            NPC npc = Main.npc[args.NpcId];

            if (!IsBossSpawnable(npc.netID))
            {
                if (Main.zenithWorld && npc.netID == NPCID.SkeletronPrime)
                {
                    npc.position = new Vector2(int.MinValue, int.MinValue);
                    npc.velocity = new Vector2(int.MinValue, int.MinValue);
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, number: args.NpcId);
                }
                else
                {
                    DespawnNpc(args.NpcId);
                }
            }
        }

        private void DespawnNpc(int index)
        {
            Main.npc[index].active = false;
            Main.npc[index].type = 0;
            TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", index);
        }

        private bool IsSpawnerItem(Item item)
        {
            int[] spawnerIds = {
                ItemID.SlimeCrown,
                ItemID.SuspiciousLookingEye,
                ItemID.WormFood,
                ItemID.BloodySpine,
                ItemID.Abeemination,
                ItemID.DeerThing,
                ItemID.QueenSlimeCrystal,
                ItemID.MechanicalWorm,
                ItemID.MechanicalEye,
                ItemID.MechanicalSkull,
                ItemID.MechdusaSummon,
                ItemID.LihzahrdPowerCell,
                ItemID.TruffleWorm,
                ItemID.CelestialSigil
            };

            return spawnerIds.Contains(item.netID);
        }

        private bool IsBossSpawnable(int netID)
        {
            if (Config.PreventIllegalBoss)
            {
                if (!Main.hardMode &&
                    (netID == NPCID.QueenSlimeBoss ||
                    netID == NPCID.TheDestroyer ||
                    netID == NPCID.Retinazer ||
                    netID == NPCID.Spazmatism ||
                    netID == NPCID.SkeletronPrime ||
                    netID == NPCID.DukeFishron))
                {
                    return false;
                }

                if (!NPC.downedMechBoss1 && !NPC.downedMechBoss2 && !NPC.downedMechBoss3 &&
                    netID == NPCID.Plantera)
                {
                    return false;
                }

                if (!NPC.downedPlantBoss &&
                    (netID == NPCID.HallowBoss || netID == NPCID.EmpressButterfly || netID == NPCID.Golem))
                {
                    return false;
                }

                if (!NPC.downedGolemBoss &&
                    (netID == NPCID.CultistBoss || netID == NPCID.MoonLordCore))
                {
                    return false;
                }
            }

            if (TShock.Utils.GetActivePlayerCount() < Config.RequiredPlayersforBoss)
            {
                if (!NPC.downedSlimeKing && netID == NPCID.KingSlime ||
                    !NPC.downedBoss1 && netID == NPCID.EyeofCthulhu ||
                    (!NPC.downedBoss2 && !WorldGen.crimson && netID == NPCID.EaterofWorldsHead) ||
                    (!NPC.downedBoss2 && WorldGen.crimson && netID == NPCID.BrainofCthulhu) ||
                    !NPC.downedDeerclops && netID == NPCID.Deerclops ||
                    !NPC.downedBoss3 && netID == NPCID.SkeletronHead ||
                    !NPC.downedQueenBee && netID == NPCID.QueenBee ||
                    (!Main.hardMode && netID == NPCID.WallofFlesh) ||
                    !NPC.downedQueenSlime && netID == NPCID.QueenSlimeBoss ||
                    !NPC.downedMechBoss1 && netID == NPCID.TheDestroyer ||
                    !NPC.downedMechBoss2 && netID == NPCID.Retinazer ||
                    !NPC.downedMechBoss2 && netID == NPCID.Spazmatism ||
                    !NPC.downedMechBoss3 && netID == NPCID.SkeletronPrime ||
                    !NPC.downedPlantBoss && netID == NPCID.Plantera ||
                    !NPC.downedGolemBoss && netID == NPCID.Golem ||
                    !NPC.downedFishron && netID == NPCID.DukeFishron ||
                    !NPC.downedMoonlord && netID == NPCID.MoonLordCore ||
                    !NPC.downedAncientCultist && netID == NPCID.CultistBoss ||
                    !NPC.downedEmpressOfLight && netID == NPCID.HallowBoss)
                {
                    return false;
                }
            }

            if (!Config.AllowKingSlime && netID == NPCID.KingSlime) // King Slime
            {
                return false;
            }

            if (!Config.AllowEyeOfCthulhu && netID == NPCID.EyeofCthulhu) // Eye of Cthulhu
            {
                return false;
            }

            if (!Config.AllowEaterOfWorlds && netID == NPCID.EaterofWorldsHead) // Eater of Worlds
            {
                return false;
            }

            if (!Config.AllowBrainOfCthulhu && netID == NPCID.BrainofCthulhu) // Brain of Cthulhu
            {
                return false;
            }

            if (!Config.AllowQueenBee && netID == NPCID.QueenBee) // Queen Bee
            {
                return false;
            }

            if (!Config.AllowSkeletron && netID == NPCID.SkeletronHead) // Skeletron
            {
                return false;
            }

            if (!Config.AllowDeerclops && netID == NPCID.Deerclops) // Deerclops
            {
                return false;
            }

            if (!Config.AllowWallOfFlesh && netID == NPCID.WallofFlesh) // Wall of Flesh
            {
                return false;
            }

            if (!Config.AllowQueenSlime && netID == NPCID.QueenSlimeBoss) // Queen Slime
            {
                return false;
            }

            if (!Config.AllowTheTwins && (netID == NPCID.Retinazer || netID == NPCID.Spazmatism)) // The Twins
            {
                return false;
            }

            if (!Config.AllowTheDestroyer && netID == NPCID.TheDestroyer) // The Destroyer
            {
                return false;
            }

            if (!Config.AllowSkeletronPrime && netID == NPCID.SkeletronPrime) // Skeletron Prime
            {
                return false;
            }

            if (!Config.AllowPlantera && netID == NPCID.Plantera) // Plantera
            {
                return false;
            }

            if (!Config.AllowGolem && netID == NPCID.Golem) // Golem
            {
                return false;
            }

            if (!Config.AllowDukeFishron && netID == NPCID.DukeFishron) // Duke Fishron
            {
                return false;
            }

            if (!Config.AllowEmpressOfLight && netID == NPCID.HallowBoss) // Empress of Light
            {
                return false;
            }

            if (!Config.AllowLunaticCultist && netID == NPCID.CultistBoss) // Lunatic Cultist
            {
                return false;
            }

            if (!Config.AllowMoonLord && netID == NPCID.MoonLordCore) // Moon Lord
            {
                return false;
            }

            return true;
        }

        private int GetBossNetIDFromSpawner(int itemID)
        {
            return itemID switch
            {
                ItemID.SlimeCrown => NPCID.KingSlime,
                ItemID.SuspiciousLookingEye => NPCID.EyeofCthulhu,
                ItemID.WormFood => NPCID.EaterofWorldsHead,
                ItemID.BloodySpine => NPCID.BrainofCthulhu,
                ItemID.Abeemination => NPCID.QueenBee,
                ItemID.DeerThing => NPCID.Deerclops,
                ItemID.QueenSlimeCrystal => NPCID.QueenSlimeBoss,
                ItemID.MechanicalWorm => NPCID.TheDestroyer,
                ItemID.MechanicalEye => NPCID.Retinazer,
                ItemID.MechanicalSkull => NPCID.SkeletronPrime,
                ItemID.MechdusaSummon => NPCID.SkeletronPrime,
                ItemID.LihzahrdPowerCell => NPCID.Golem,
                ItemID.TruffleWorm => NPCID.DukeFishron,
                ItemID.CelestialSigil => NPCID.MoonLordCore,
                _ => 0,
            };
        }
    }
}
