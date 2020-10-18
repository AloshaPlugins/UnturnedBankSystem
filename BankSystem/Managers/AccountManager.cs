using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Models;

namespace BankSystem.Managers
{
    public static class AccountManager
    {
        public static List<Account> GetPlayerToAccounts(ulong id) =>
            Main.Instance.Configuration.Instance.Accounts.Where(account => account.Id == id).ToList();

        public static Account GetAccount(int id) =>
            Main.Instance.Configuration.Instance.Accounts.FirstOrDefault(accont => accont.AccountId == id);

        public static List<string> GetHistoriesPage(List<string> histories, int page, int count) =>
            histories.Skip(page * count - count).Take(page * count).ToList();


        public static Account CreateAccount(Account account)
        {
            Main.Instance.Configuration.Instance.Accounts.Add(account);
            account.Save();
            return account;
        }

        public static int NewAccountId()
        {
            int id;
            while (true)
            {
                id = new Random().Next(1000, 9999);
                if (Main.Instance.Configuration.Instance.Accounts.Any(account => account.AccountId == id)) continue;
                else break;
            }

            return id;
        }
    }
}
