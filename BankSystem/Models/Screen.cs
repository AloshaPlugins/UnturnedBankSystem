using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;

namespace BankSystem.Models
{
    public class Screen
    {
        public CSteamID Id;
        public Account Account;
        public int Page;
        public Screen(CSteamID id, int page, Account account)
        {
            Id = id;
            Page = page;
            this.Account = account;
        }
    }
}
