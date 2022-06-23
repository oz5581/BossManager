using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;
using BossManagerConfig;
using Terraria.ID;

namespace BossManager
{
    [ApiVersion(2, 1)]
    public class BossManagerPlugin : TerrariaPlugin
    {
        public override string Author => "Ozz5581";
        public override string Description => "Boss Management Tools";
        public override string Name => "BossManager";
        public override Version Version => new Version(1, 0, 0, 0);

        public BossManagerPlugin(Main game) : base(game)
        {

        }

        private static Config Config = Config.Read();

        public override void Initialize()
        {
            ServerApi.Hooks.NpcSpawn.Register(this, OnNPCSpawn);

            ServerApi.Hooks.ServerJoin.Register(this, OnJoin);

            Commands.ChatCommands.Add(new Command("bossmgr.reload", ReloadCommand, "bossrel")
            {
                HelpText = "Reload the BossManager Plugin."
            });

            Commands.ChatCommands.Add(new Command("bossmgr.undoboss", UndoBossCommand, "undoboss", "uboss")
            {
                HelpText = "Toggle a Bosses Defeated State."
            });

            Commands.ChatCommands.Add(new Command("bossmgr.listboss", ListBossCommand, "listboss", "lboss", "bosses")
            {
                HelpText = "List Defeated Bosses."
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Deregister hooks here
            }
            base.Dispose(disposing);
        }

        private void ReloadCommand(CommandArgs args)
        {
            Config = Config.Read();
            args.Player.SendInfoMessage("DisableBoss config reloaded!");
        }

        public void OnJoin(JoinEventArgs args)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (!Config.AllowJoinDuringBoss && Main.npc[i].active && Main.npc[i].boss)
                    TShock.Players[args.Who].Disconnect("The current boss must be defeated before joining.");
            }
        }

        private void ListBossCommand(CommandArgs args)
        {
            string subCmd = args.Parameters.Count == 0 ? "listboss" : args.Parameters[0].ToLower();

            switch (subCmd)
            {
                case "boss":
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

                        if (String.IsNullOrEmpty(String.Join(", ", BossList)))
                            args.Player.SendInfoMessage("No Bosses have been Defeated so far!");
                        else
                            args.Player.SendInfoMessage($"Defeated Bosses: {String.Join(", ", BossList)}");
                    }
                    return;
                case "event":
                    {
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

                        if (String.IsNullOrEmpty(String.Join(", ", EventList)))
                            args.Player.SendInfoMessage("No Events have been Defeated so far!");
                        else
                            args.Player.SendInfoMessage($"Defeated Events: {String.Join(", ", EventList)}");
                    }
                    return;
                default:
                    args.Player.SendErrorMessage("Invalid Usage! Please specify Bosses or Events:");
                    args.Player.SendInfoMessage(
                        "/listboss boss - Lists Defeated Bosses\n" +
                        "/listboss event - Lists Defeated Invasions");
                    return;
            }
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
                        args.Player.SendInfoMessage($"Set Wall of Flesh (aka Hardmode) as {(Main.hardMode ? "[c/FF0000:Killed]" : "[c/00FF00:Not Killed]")}!");
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
        public void OnNPCSpawn(NpcSpawnEventArgs args)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!Config.AllowKingSlime && npc.type == NPCID.KingSlime) // King Slime
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowEyeOfCthulhu && npc.type == NPCID.EyeofCthulhu) // Eye of Cthulhu
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowEaterOfWorlds && npc.type == NPCID.EaterofWorldsHead) // Eater of Worlds
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowBrainOfCthulhu && npc.type == NPCID.BrainofCthulhu) // Brain of Cthulhu
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowQueenBee && npc.type == NPCID.QueenBee) // Queen Bee
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowSkeletron && npc.type == NPCID.SkeletronHead) // Skeletron
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowDeerclops && npc.type == NPCID.Deerclops) // Deerclops
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowWallOfFlesh && npc.type == NPCID.WallofFlesh) // Wall of Flesh
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowQueenSlime && npc.type == NPCID.QueenSlimeBoss) // Queen Slime
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowTheTwins && (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)) // The Twins
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowTheDestroyer && npc.type == NPCID.TheDestroyer) // The Destroyer
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowSkeletronPrime && npc.type == NPCID.SkeletronPrime) // Skeletron Prime
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowPlantera && npc.type == NPCID.Plantera) // Plantera
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowGolem && npc.type == NPCID.Golem) // Golem
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowDukeFishron && npc.type == NPCID.DukeFishron) // Duke Fishron
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowEmpressOfLight && npc.type == NPCID.HallowBoss) // Empress of Light
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowLunaticCultist && npc.type == NPCID.CultistBoss) // Lunatic Cultist
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }

                if (!Config.AllowMoonLord && npc.type == NPCID.MoonLordCore) // Moon Lord
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                }
            }
        }
    }
}