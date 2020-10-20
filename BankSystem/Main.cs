using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Managers;
using Rocket.Core.Plugins;
using SDG.Unturned;

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
        }

        protected override void Unload()
        {
            Instance = null;
            EffectManager.onEffectButtonClicked -= ControlManager.OnButtonClicked;
            EffectManager.onEffectTextCommitted -= ControlManager.OnTextTyped;
        }
    }
}
