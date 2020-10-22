using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Models;
using Rocket.API;
using Steamworks;

namespace BankSystem
{
    public class Config : IRocketPluginConfiguration
    {
        public ushort Effect, ATM;
        public int AccountLimit;
        public List<Account> Accounts;
        public void LoadDefaults()
        {
            Effect = 47532;
            ATM = 61356;
            AccountLimit = 5;
            Accounts = new List<Account>();
        }
    }
}
