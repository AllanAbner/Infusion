﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infusion.LegacyApi.Events;
using Infusion.Packets;
using Infusion.Packets.Both;
using Infusion.Packets.Server;

namespace Infusion.LegacyApi
{
    internal sealed class MapObserver
    {
        private readonly IEventJournalSource eventJournalSource;

        public event Action<int> MapChanged;

        public MapObserver(IServerPacketSubject serverPacketSubject, IEventJournalSource eventJournalSource)
        {
            this.eventJournalSource = eventJournalSource;
            serverPacketSubject.Subscribe(PacketDefinitions.QuestArrow, HandleQuestArrow);
            serverPacketSubject.Subscribe(PacketDefinitions.MapMessage, HandleMapMessage);
            serverPacketSubject.RegisterFilter(FilterServerPackets);
        }

        private Packet? FilterServerPackets(Packet rawPacket)
        {
            if (rawPacket.Id == PacketDefinitions.GeneralInformationPacket.Id && rawPacket.Payload[4] == 8)
            {
                var packet = new SetMapPacket();
                packet.Deserialize(rawPacket);
                MapChanged?.Invoke(packet.MapId);
            }

            return rawPacket;
        }

        private void HandleMapMessage(MapMessagePacket packet)
        {
            var ev = new MapMessageEvent(packet.UpperLeft, packet.LowerRight, packet.Width, packet.Height);
            eventJournalSource.Publish(ev);
        }

        private void HandleQuestArrow(QuestArrowPacket packet)
        {
            var questArrowEvent = new QuestArrowEvent(packet.Active, packet.Location);
            eventJournalSource.Publish(questArrowEvent);
        }
    }
}
