using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using TShockAPI;

namespace BossManager
{
    class Config
    {
        public bool AllowKingSlime;
        public bool AllowEyeOfCthulhu;
        public bool AllowEaterOfWorlds;
        public bool AllowBrainOfCthulhu;
        public bool AllowQueenBee;
        public bool AllowSkeletron;
        public bool AllowDeerclops;
        public bool AllowWallOfFlesh;
        public bool AllowQueenSlime;
        public bool AllowTheTwins;
        public bool AllowTheDestroyer;
        public bool AllowSkeletronPrime;
        public bool AllowPlantera;
        public bool AllowGolem;
        public bool AllowDukeFishron;
        public bool AllowEmpressOfLight;
        public bool AllowLunaticCultist;
        public bool AllowMoonLord;
        public bool AllowJoinDuringBoss;

        public bool PreventIllegalBoss;
        public int RequiredPlayersforBoss;

        public static Config Read()
        {

            string path = Path.Combine(TShock.SavePath, "BossManager.json");
            if (!File.Exists(path))
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(Default(), Formatting.Indented));
                return Default();
            }
            try
            {
                var args = JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
                return args;
            }
            catch
            {
                return Default();
            }
        }

        private static Config Default()
        {
            return new Config()
            {
                AllowJoinDuringBoss = true,
                PreventIllegalBoss = false,
                RequiredPlayersforBoss = 1,

                AllowKingSlime = true,
                AllowEyeOfCthulhu = true,
                AllowEaterOfWorlds = true,
                AllowBrainOfCthulhu = true,
                AllowQueenBee = true,
                AllowSkeletron = true,
                AllowDeerclops = true,
                AllowWallOfFlesh = true,
                AllowQueenSlime = true,
                AllowTheTwins = true,
                AllowTheDestroyer = true,
                AllowSkeletronPrime = true,
                AllowPlantera = true,
                AllowGolem = true,
                AllowDukeFishron = true,
                AllowEmpressOfLight = true,
                AllowLunaticCultist = true,
                AllowMoonLord = true,
            };
        }
    }
}
