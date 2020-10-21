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
        public List<Account> Accounts;
        public bool ISended = false;
        public string Response1 = string.Empty, Response2 = string.Empty;
        public Screen(CSteamID id, int page, Account account)
        {
            Id = id;
            Page = page;
            this.Account = account;
        }
    }
}
