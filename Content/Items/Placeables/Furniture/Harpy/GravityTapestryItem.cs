﻿using LivingWorldMod.Content.Tiles.Furniture.Harpy;
using Terraria.ID;
using Terraria.ModLoader;

namespace LivingWorldMod.Content.Items.Placeables.Furniture.Harpy {
    public class GravityTapestryItem : BaseItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.BlueBanner);
            Item.rare = ItemRarityID.Blue;
            Item.placeStyle = 0;
            Item.value = Terraria.Item.buyPrice(silver: 20);
            Item.createTile = ModContent.TileType<GravityTapestryTile>();
        }
    }
}