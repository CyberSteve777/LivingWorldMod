using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace LivingWorldMod.NPCs.Villagers
{
    public abstract class Villager : ModNPC
    {
        public static readonly string VILLAGER_SPRITE_PATH = nameof(LivingWorldMod) + "/NPCs/Villagers/Textures/";

        public readonly WeightedRandom<string> dialogueText;

        public readonly WeightedRandom<string> reputationText;

        public bool isNegativeRep;
        public bool isNeutralRep;
        public bool isPositiveRep;
        public bool isMaxRep;

        public VillagerType villagerType;

        public int spriteVariation = 0;

        public Villager()
        {
            dialogueText = GetDialogueText();
            reputationText = GetReputationText();
        }

        public string VillagerName
        {
            get
            {
                return villagerType.ToString();
            }
        }

        public override string Texture => VILLAGER_SPRITE_PATH + VillagerName + "Style1";

        public override string[] AltTextures => new string[] { 
            VILLAGER_SPRITE_PATH + VillagerName + "Style2",
            VILLAGER_SPRITE_PATH + VillagerName + "Style3"
        };

        public override bool CloneNewInstances => true;

        public override ModNPC Clone()
        {
            Villager clonedNPC = (Villager)base.Clone();
            clonedNPC.spriteVariation = Main.rand.Next(0, 3);
            return clonedNPC;
        }

        public override void SetStaticDefaults()
        {
            NPCID.Sets.ExtraTextureCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 40;
            npc.friendly = true;
            npc.lifeMax = 500;
            npc.defense = 15;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 7;
            animationType = NPCID.Guide;
        }

        //TODO: Figure how in the heck to draw NPCs manually properly
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D textureToDraw;
            if (spriteVariation == 0)
            {
                textureToDraw = Main.npcTexture[npc.type];
            }
            else
            {
                textureToDraw = Main.npcAltTextures[npc.type][spriteVariation];
            }
            SpriteEffects spriteDirection = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(textureToDraw, new Rectangle((int)(npc.Right.X - (npc.frame.Width / 1.5) - Main.screenPosition.X), (int)(npc.Bottom.Y - npc.frame.Height - Main.screenPosition.Y + 2f), npc.frame.Width, npc.frame.Height), npc.frame, drawColor, npc.rotation, default(Vector2), spriteDirection, 0);
            return false;
        }

        /*public override void SetChatButtons(ref string button, ref string button2)
        {
            if (isMerchant)
            {
                button = Language.GetTextValue("LegacyInterface.28");
                button2 = "Reputation";
            }
            else
            {
                button = "Reputation";
            }
        }*/

        /*public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton && isMerchant)
            {
                shop = true;
            }
            else if (firstButton && !isMerchant)
            {
                Main.npcChatText = GetReputationChat();
            }
            else if (!firstButton && isMerchant)
            {
                Main.npcChatText = GetReputationChat();
            }
            else
            {
                shop = true;
            }
        }*/

        #region Virtual Methods

        /// <summary>
        /// Method used to fill the dialogueText variable that is used when talking to Villager.
        /// </summary>
        /// <returns>Returns a value telling the player to contact a mod dev by default.</returns>
        public virtual WeightedRandom<string> GetDialogueText()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();
            chat.Add("If someone saw this text... I'd be scared and tell a mod dev immediately!");
            return chat;
        }

        /// <summary>
        /// Method used to fill the reputationText variable that is used wwhen clicking the Reputation chat button.
        /// </summary>
        /// <returns>Returns a value telling the player to contact a mod dev by default.</returns>
        public virtual WeightedRandom<string> GetReputationText()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();
            chat.Add("If someone saw this text... I'd be scared and tell a mod dev immediately!");
            return chat;
        }
        #endregion

        private void UpdateReputationBools()
        {
            float reputation = LWMWorld.villageReputation[(int)villagerType];
            if (reputation < -30f)
            {
                isNegativeRep = true;
                isNeutralRep = false;
                isPositiveRep = false;
                isMaxRep = false;
            }
            else if (reputation >= -30f && reputation <= 30f)
            {
                isNegativeRep = false;
                isNeutralRep = true;
                isPositiveRep = false;
                isMaxRep = false;
            }
            else if (reputation > 30f && reputation < 100f)
            {
                isNegativeRep = false;
                isNeutralRep = false;
                isPositiveRep = true;
                isMaxRep = false;
            }
            else if (reputation >= 100f)
            {
                isNegativeRep = true;
                isNeutralRep = false;
                isPositiveRep = false;
                isMaxRep = true;
            }
        }
    }
}