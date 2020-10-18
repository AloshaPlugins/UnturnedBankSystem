using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Models;
using SDG.Unturned;

namespace BankSystem.Managers
{
    public class ControlManager
    {
        private const string Prefix = "ABS";
        private const short Key = 457;
        public List<Screen> Screens = new List<Screen>();

        public void OnButtonClicked(Player player, string buttonName)
        {
            if (!buttonName.StartsWith(Prefix)) return;
            buttonName = buttonName.Substring(Prefix.Length);



        }

        public void OnTextTyped(Player player, string buttonName, string text)
        {
            if (!buttonName.StartsWith(Prefix)) return;
            buttonName = buttonName.Substring(Prefix.Length);



        }

        public void ShowMainUI(Player player)
        {
            
        }

        public void ReturnMainUI(Player player)
        {

        }

        public void CloseUI(Player player)
        {

        }
    }
}
