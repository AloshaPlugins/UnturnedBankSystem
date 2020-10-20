using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Models;
using SDG.Unturned;

namespace BankSystem.Managers
{
    public static class ControlManager
    {
        private const string Prefix = "ABS";
        private const short Key = 457;
        public static List<Screen> Screens = new List<Screen>();

        public static void OnButtonClicked(Player player, string buttonName)
        {
            if (!buttonName.StartsWith(Prefix)) return;
            buttonName = buttonName.Substring(Prefix.Length);



        }

        public static void OnTextTyped(Player player, string buttonName, string text)
        {
            if (!buttonName.StartsWith(Prefix)) return;
            buttonName = buttonName.Substring(Prefix.Length);



        }

        public static void ShowMainUI(Player player)
        {
            
        }

        public static void ReturnMainUI(Player player)
        {

        }   

        public static void CloseUI(Player player)
        {

        }
    }
}
