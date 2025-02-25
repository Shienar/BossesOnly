using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace BOConfig
{
    class BossesOnlyConfig: ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(true)]
        public bool toggleBossesOnly = true;

        [DefaultValue(false)]
        public bool onlyOneBoss = false;
    }
}