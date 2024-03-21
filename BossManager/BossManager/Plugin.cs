using TerrariaApi.Server;
using TShockAPI;
using Terraria;
using Terraria.ID;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using Microsoft.Xna.Framework;

namespace BossManager
{
    [ApiVersion(2, 1)]
    public partial class Plugin : TerrariaPlugin
    {
        public override void Initialize()
        {
            ServerApi.Hooks.NpcSpawn.Register(this, OnNpcSpawn);

            ServerApi.Hooks.ServerJoin.Register(this, OnJoin);

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose here
            }
            base.Dispose(disposing);
        }

        private void ReloadCommand(CommandArgs args)
        {
            Config = Config.Read();
            args.Player.SendInfoMessage("BossMGR File Reloaded");
        }

        public void OnJoin(JoinEventArgs args)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (!Config.AllowJoinDuringBoss && Main.npc[i].active && Main.npc[i].boss)
                    TShock.Players[args.Who].Disconnect("The current in-game players must defeat the current boss\nBefore you can join.");
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
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (Config.PreventIllegalBoss)
                {
                    if (!Main.hardMode &&
                        (npc.type == NPCID.QueenSlimeBoss ||
                        npc.type == NPCID.TheDestroyer ||
                        npc.type == NPCID.Retinazer ||
                        npc.type == NPCID.Spazmatism ||
                        npc.type == NPCID.SkeletronPrime ||
                        npc.type == NPCID.DukeFishron))
                    {
                        UpdateNpc(i);
                    }

                    if (!NPC.downedMechBoss1 && !NPC.downedMechBoss2 && !NPC.downedMechBoss3 &&
                        npc.type == NPCID.Plantera)
                    {
                        UpdateNpc(i);
                    }

                    if (!NPC.downedPlantBoss &&
                        (npc.type == NPCID.HallowBoss || npc.type == NPCID.EmpressButterfly || npc.type == NPCID.Golem))
                    {
                        UpdateNpc(i);
                    }

                    if (!NPC.downedGolemBoss &&
                        (npc.type == NPCID.CultistBoss || npc.type == NPCID.MoonLordCore))
                    {
                        UpdateNpc(i);
                    }
                }

                if (TShock.Utils.GetActivePlayerCount() < Config.RequiredPlayersforBoss)
                {
                    if (!NPC.downedSlimeKing && npc.type == NPCID.KingSlime ||
                        !NPC.downedBoss1 && npc.type == NPCID.EyeofCthulhu ||
                        (!NPC.downedBoss2 && !WorldGen.crimson && npc.type == NPCID.EaterofWorldsHead) ||
                        (!NPC.downedBoss2 && WorldGen.crimson && npc.type == NPCID.BrainofCthulhu) ||
                        !NPC.downedDeerclops && npc.type == NPCID.Deerclops ||
                        !NPC.downedBoss3 && npc.type == NPCID.SkeletronHead ||
                        !NPC.downedQueenBee && npc.type == NPCID.QueenBee ||
                        (!Main.hardMode && npc.type == NPCID.WallofFlesh) ||
                        !NPC.downedQueenSlime && npc.type == NPCID.QueenSlimeBoss ||
                        !NPC.downedMechBoss1 && npc.type == NPCID.TheDestroyer ||
                        !NPC.downedMechBoss2 && npc.type == NPCID.Retinazer ||
                        !NPC.downedMechBoss2 && npc.type == NPCID.Spazmatism ||
                        !NPC.downedMechBoss3 && npc.type == NPCID.SkeletronPrime ||
                        !NPC.downedPlantBoss && npc.type == NPCID.Plantera ||
                        !NPC.downedGolemBoss && npc.type == NPCID.Golem ||
                        !NPC.downedFishron && npc.type == NPCID.DukeFishron ||
                        !NPC.downedMoonlord && npc.type == NPCID.MoonLordCore ||
                        !NPC.downedAncientCultist && npc.type == NPCID.CultistBoss ||
                        !NPC.downedEmpressOfLight && npc.type == NPCID.HallowBoss)
                    {
                        UpdateNpc(i);
                    }
                }

                if (!Config.AllowKingSlime && npc.type == NPCID.KingSlime) // King Slime
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowEyeOfCthulhu && npc.type == NPCID.EyeofCthulhu) // Eye of Cthulhu
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowEaterOfWorlds && npc.type == NPCID.EaterofWorldsHead) // Eater of Worlds
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowBrainOfCthulhu && npc.type == NPCID.BrainofCthulhu) // Brain of Cthulhu
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowQueenBee && npc.type == NPCID.QueenBee) // Queen Bee
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowSkeletron && npc.type == NPCID.SkeletronHead) // Skeletron
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowDeerclops && npc.type == NPCID.Deerclops) // Deerclops
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowWallOfFlesh && npc.type == NPCID.WallofFlesh) // Wall of Flesh
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowQueenSlime && npc.type == NPCID.QueenSlimeBoss) // Queen Slime
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowTheTwins && (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)) // The Twins
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowTheDestroyer && npc.type == NPCID.TheDestroyer) // The Destroyer
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowSkeletronPrime && npc.type == NPCID.SkeletronPrime) // Skeletron Prime
                {
                    if (Main.zenithWorld)
                    {
                        npc.position = new Vector2(int.MinValue, int.MinValue);
                        npc.velocity = new Vector2(int.MinValue, int.MinValue);
                        TSPlayer.All.SendData(PacketTypes.NpcUpdate, number: i);
                    }
                    else
                    {
                        UpdateNpc(i);
                    }
                }

                if (!Config.AllowPlantera && npc.type == NPCID.Plantera) // Plantera
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowGolem && npc.type == NPCID.Golem) // Golem
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowDukeFishron && npc.type == NPCID.DukeFishron) // Duke Fishron
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowEmpressOfLight && npc.type == NPCID.HallowBoss) // Empress of Light
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowLunaticCultist && npc.type == NPCID.CultistBoss) // Lunatic Cultist
                {
                    UpdateNpc(i);
                }

                if (!Config.AllowMoonLord && npc.type == NPCID.MoonLordCore) // Moon Lord
                {
                    UpdateNpc(i);
                }
            }
        }

        private void UpdateNpc(int index)
        {
            Main.npc[index].active = false;
            Main.npc[index].type = 0;
            TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", index);
        }
    }
}
