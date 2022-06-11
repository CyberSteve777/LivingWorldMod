﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using LivingWorldMod.Common.ModTypes;
using LivingWorldMod.Content.TileEntities.Interactables.VillageShrines;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LivingWorldMod.Core.PacketHandlers {
    /// <summary>
    /// PacketHandler that handles all packet functionality in relation to Village Shrines.
    /// </summary>
    public class ShrinePacketHandler : PacketHandler {
        /// <summary>
        /// Sent/Received when a new player/client first enters a server's world. Syncs all Shrine
        /// Tile Entities from the server to said client.
        /// </summary>
        public const byte SyncNewPlayer = 0;

        public override void HandlePacket(BinaryReader reader, int fromWhomst) {
            byte packetType = reader.ReadByte();

            switch (packetType) {
                case SyncNewPlayer:
                    if (Main.netMode == NetmodeID.Server) {
                        List<VillageShrineEntity> shrines = TileEntity.ByID.Values.OfType<VillageShrineEntity>().ToList();

                        foreach (VillageShrineEntity entity in shrines) {
                            NetMessage.SendData(MessageID.TileSection, fromWhomst, number: entity.Position.X - 1, number2: entity.Position.Y - 1, number3: 5, number4: 6);
                        }
                    }
                    break;
                default:
                    ModContent.GetInstance<LivingWorldMod>().Logger.Warn($"Invalid ShrinePacketHandler Packet Type of {packetType}");
                    break;
            }
        }
    }
}