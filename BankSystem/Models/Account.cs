using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class Account
    {
        public int AccountId;

        public ulong Id;
        public uint Price;
        public List<string> Histories;

        public Account(int accountId, ulong ıd, uint price)
        {
            AccountId = accountId;
            Id = ıd;
            Price = price;
            this.Histories = new List<string>();
        }

        public void AddAction(string str, DateTime time)
        {
            this.Histories.Add($"[{time.ToLongDateString()}] {str}");
            Save();
        }

        public void Save() => Main.Instance.Configuration.Save();

    }
}
