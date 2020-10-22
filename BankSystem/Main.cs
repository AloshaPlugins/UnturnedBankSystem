using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Managers;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace BankSystem
{
    public class Main : RocketPlugin<Config>
    {
        public static Main Instance;
        protected override void Load()
        {
            Instance = this;
            EffectManager.onEffectButtonClicked += ControlManager.OnButtonClicked;
            EffectManager.onEffectTextCommitted += ControlManager.OnTextTyped;
            UnturnedPlayerEvents.OnPlayerUpdateGesture += UnturnedPlayerEventsOnOnPlayerUpdateGesture;
            U.Events.OnPlayerDisconnected += EventsOnOnPlayerDisconnected;
        }

        private void EventsOnOnPlayerDisconnected(UnturnedPlayer player)
        {
            ControlManager.Screens.RemoveAll(screen => screen.Id == player.CSteamID);
        }

        private void UnturnedPlayerEventsOnOnPlayerUpdateGesture(UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
        {
            if (gesture != UnturnedPlayerEvents.PlayerGesture.PunchLeft) return;
            var raycast = Physics.Raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), out var info,
                5f, RayMasks.STRUCTURE | RayMasks.STRUCTURE_INTERACT);
            if (!raycast) return;

            var hit = info;
            if (hit.transform == null) return;

            var flag = StructureManager.tryGetInfo(hit.transform, out var x, out var y, out var index, out var region);
            if (!flag) return;
            var structer = region.structures[index];

            if (structer.structure.id != Configuration.Instance.ATM) return;
            ControlManager.ShowCardsUI(player.Player);
        }

        protected override void Unload()
        {
            Instance = null;
            EffectManager.onEffectButtonClicked -= ControlManager.OnButtonClicked;
            EffectManager.onEffectTextCommitted -= ControlManager.OnTextTyped;
            UnturnedPlayerEvents.OnPlayerUpdateGesture -= UnturnedPlayerEventsOnOnPlayerUpdateGesture;
            U.Events.OnPlayerDisconnected -= EventsOnOnPlayerDisconnected;
        }
    }
}
