using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.ID;
using BOConfig;


namespace BossesOnly
{
	public class BossesOnly : GlobalNPC
	{
		public override void OnSpawn(NPC npc, IEntitySource source)
		{
			//ModConfig toggle
			if (ModContent.GetInstance<BossesOnlyConfig>().toggleBossesOnly)
			{
				//Dont do anything if OnlyOneBoss mod config == true and a boss is alive
				if (!(ModContent.GetInstance<BossesOnlyConfig>().onlyOneBoss && Main.CurrentFrameFlags.AnyActiveBossNPC))
				{
					//Don't affect boss ads
					//Special cases for Eater of Worlds (id 13-15)
					//Eater of worls' vile spit == id 666 and can cause infinite duplication if not accounted for.
					//Special case for Wall of Flesh leeches/hungry (115-119)
					//Special case for Destroyer probes (139)
					//Special case for plantera's projectiles/hooks (263-265
					//Special case for golem (id 246-249)
					if (!(source is EntitySource_Parent parent && parent.Entity is NPC n && n.boss)
						&& npc.type != 13 && npc.type != 14 && npc.type != 15 && npc.type != 666 &&
						npc.type != 115 && npc.type != 116 && npc.type != 117 && npc.type != 118 && npc.type != 119 &&
						npc.type != 139 &&
						npc.type != 263 && npc.type != 264 && npc.type != 265 &&
						npc.type != 246 && npc.type != 247 && npc.type != 248 && npc.type != 249)
					{

						//Don't infinitely spawn bosses after using this method to spawn one boss.
						//Dont do anything if boss checklist isnt installed
						if (!npc.boss && ModLoader.TryGetMod("BossChecklist", out Mod BossList))
						{
							Dictionary<string, Dictionary<string, object>> bosses = (Dictionary<string, Dictionary<string, object>>)BossList.Call("GetBossInfoDictionary", Mod);

							foreach (Dictionary<string, object> bossInfo in bosses.Values)
							{
								Func<bool> isDowned = (Func<bool>)bossInfo["downed"];
								bool isBoss = (bool)bossInfo["isBoss"];
								List<int> IDs = (List<int>)bossInfo["npcIDs"];

								if (IDs.Contains(npc.type)) return;

								//Finds the next boss in progression that hasnt been killed.
								//Betsy (ID: 551) isn't marked as killed/completed when spawned in manually, so she is skipped over.
								//
								//This mod doesn't do anything when all bosses are defeated.
								if (!isDowned() && isBoss && IDs[0] != 551)
								{
									//Brain of Cthulu special case
									//Eater of worlds is preferred by default. Both have the same progression value
									if ((float)bossInfo["progression"] == 3 && WorldGen.crimson == true)
									{
										NPC.SpawnBoss((int)npc.position.X, (int)npc.position.Y, 266, Main.myPlayer);
									}
									else if (IDs[0] == 125) //Twins special case
									{
										NPC.SpawnBoss((int)npc.position.X, (int)npc.position.Y, 125, Main.myPlayer);
										NPC.SpawnBoss((int)npc.position.X, (int)npc.position.Y, 126, Main.myPlayer);
									}
									else
									{
										NPC.SpawnBoss((int)npc.position.X, (int)npc.position.Y, IDs[0], Main.myPlayer);
									}
									return;
								}
							}
						}
					}
				}
			}
        }

	}
}
