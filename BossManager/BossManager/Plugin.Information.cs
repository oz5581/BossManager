using Terraria;
using TerrariaApi.Server;

namespace BossManager
{
    public partial class Plugin: TerrariaPlugin
    {
        public override string Name => "BossManager";
        public override Version Version => new(1, 3, 1);
        public override string Author => "Ozz5581";
        public override string Description => "Controls boss spawning requirements.";

        public Plugin(Main game) : base(game)
        {
        }

        private static Config Config = Config.Read();
    }
}
