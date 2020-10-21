using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Models;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Screen = BankSystem.Models.Screen;

namespace BankSystem.Managers
{
    public static class ControlManager
    {
        private const string Prefix = "BNK_";
        private const short Key = 457;
        public static List<Screen> Screens = new List<Screen>();

        public static void OnButtonClicked(Player player, string buttonName)
        {
            if (!buttonName.StartsWith(Prefix)) return;
            buttonName = buttonName.Substring(Prefix.Length);

            var Id = player.channel.owner.playerID.steamID;
            var screen = Screens.FirstOrDefault(s => s.Id == Id);
            if (screen == null)
            {
                CloseUI(player);
                return;
            }

            var untPlayer = UnturnedPlayer.FromPlayer(player);
            if (buttonName == "Yatir_100")
            {
                if (untPlayer.Experience < 100) return;
                untPlayer.Experience -= 100;
                screen.Account.Price += 100;
                AccountManager.Save();
                ShowMainUI(player, screen.Account);
                return;
            }
            if (buttonName == "Cek_100")
            {
                if (screen.Account.Price < 100) return;
                untPlayer.Experience += 100;
                screen.Account.Price -= 100;
                AccountManager.Save();
                ShowMainUI(player, screen.Account);
                return;
            }
            if (buttonName == "Havale")
            {
                EffectManager.sendUIEffectVisibility(Key, Id, true, "Transfer_Money", true);
                MultipleVisibility(Id, new string[]
                {
                    "Select_Card",
                    "Select_Card_Add",
                    "Deposit_Money",
                    "Withdraw_Money",
                    "Main"
                });
                return;
            }

            if (buttonName == "Cek")
            {
                EffectManager.sendUIEffectVisibility(Key, Id, true, "Withdraw_Money", true);
                MultipleVisibility(Id, new string[]
                {
                    "Select_Card",
                    "Select_Card_Add",
                    "Deposit_Money",
                    "Transfer_Money",
                    "Main"
                });
                return;
            }
            if (buttonName == "Yatir")
            {
                EffectManager.sendUIEffectVisibility(Key, Id, true, "Deposit_Money", true);
                MultipleVisibility(Id, new string[]
                {
                    "Select_Card",
                    "Select_Card_Add",
                    "Withdraw_Money",
                    "Transfer_Money",
                    "Main"
                });
                return;
            }

            if (buttonName == "CardAdd")
            {
                if (screen.Accounts.Count + 1 > Main.Instance.Configuration.Instance.AccountLimit) return;
                var account = new Account()
                {
                    Id = Id.m_SteamID,
                    AccountId = AccountManager.NewAccountId(),
                    Histories = new List<string>(),
                    Price = 0
                };
                AccountManager.CreateAccount(account);
                screen.Account = account;
                screen.Accounts.Add(account);
                ShowMainUI(player, screen.Account);
                return;
            }

            if (buttonName == "Card")
            {
                var account = GetNext(screen.Accounts, screen.Page);
                if (account == null)
                {
                    ShowAddCardUI(player);
                    return;
                }
                screen.Account = account;
                ShowMainUI(player, screen.Account);
                return;
            }

            if (buttonName == "Transfer_Success")
            {
                if (string.IsNullOrEmpty(screen.Response1))
                {
                    SendError(Id, "HATA: Bu alan boş.", "Transfer_ErrorMessage_0", 10);
                    return;
                }
                if (string.IsNullOrEmpty(screen.Response2))
                {
                    SendError(Id, "HATA: Bu alan boş.", "Transfer_ErrorMessage_1", 10);
                    return;
                }

                var price = uint.Parse(screen.Response1);
                var id = int.Parse(screen.Response2);

                var account = AccountManager.GetAccount(id);
                if (account == null)
                {
                    ShowMainUI(player, screen.Account);
                    return;
                }

                screen.Account.Price -= price;
                account.Price += price;

                AccountManager.Save();
                screen.Response2 = string.Empty;
                screen.Response1 = string.Empty;
                ShowMainUI(player, screen.Account);
                return;
            }

            if (buttonName == "Cek_Success")
            {
                if (string.IsNullOrEmpty(screen.Response1))
                {
                    SendError(Id, "HATA: Buraya bir değer girmelisin.", "Cek_ErrorMessage_0", 10);
                    return;
                }

                var price = uint.Parse(screen.Response1);
                screen.Account.Price -= price;
                UnturnedPlayer.FromPlayer(player).Experience += price;

                AccountManager.Save();
                screen.Response2 = string.Empty;
                screen.Response1 = string.Empty;

                ShowMainUI(player, screen.Account);
                return;
            }

            if (buttonName == "Yatir_Success")
            {
                if (string.IsNullOrEmpty(screen.Response1))
                {
                    SendError(Id, "HATA: Buraya bir değer girmelisin.", "Yatir_ErrorMessage_0", 10);
                    return;
                }

                var price = uint.Parse(screen.Response1);
                
                UnturnedPlayer.FromPlayer(player).Experience -= price;
                screen.Account.Price += price;

                AccountManager.Save();
                screen.Response2 = string.Empty;
                screen.Response1 = string.Empty;

                ShowMainUI(player, screen.Account);
                return;
            }

            if (buttonName == "Back")
            {
                if (screen.Page - 1 <= 0) return;
                screen.Page -= 1;
                ShowCardsUI(player);
                return;
            }

            if (buttonName == "Next")
            {
                var account = GetNext(screen.Accounts, screen.Page + 1);
                if (account == null)
                {
                    ShowAddCardUI(player);
                    return;
                }
                screen.Page += 1;
                ShowCardsUI(player);
                return;
            }

            if (buttonName == "Menu")
            {
                ShowMainUI(player, screen.Account);
                return;
            }

            if (buttonName == "Close")
            {
                CloseUI(player);
                return;
            }

            if (buttonName == "Cards")
            {
                ShowCardsUI(player);
                return;
            }
        }

        public static void OnTextTyped(Player player, string buttonName, string text)
        {
            if (!buttonName.StartsWith(Prefix)) return;
            buttonName = buttonName.Substring(Prefix.Length);
            var untPlayer = UnturnedPlayer.FromPlayer(player);

            var Id = player.channel.owner.playerID.steamID;
            var screen = Screens.FirstOrDefault(s => s.Id == Id);
            if (screen == null)
            {
                CloseUI(player);
                return;
            }


            if (buttonName == "Transfer_Player")
            {
                if (int.TryParse(text, out var result))
                {
                    SendError(Id, "HATA: Bu alana sadece bir hesap numarası girebilirsin.", "Transfer_ErrorMessage_1", 5);
                    return;
                }

                var account = AccountManager.GetAccount(result);
                if (account == null)
                {
                    SendError(Id, $"HATA: Belirtmiş olduğun {result} numaralı bir hesap bulamadım. Geçerli bir hesap gir.", "Transfer_ErrorMessage_1", 10);
                    return;
                }

                screen.Response2 = text;
                SendError(Id, $"<color=green> Hesap numarası geçerli.</color>", "Transfer_ErrorMessage_1", 10);
                return;
            }

            if (buttonName == "Transfer_Price")
            {
                if (string.IsNullOrEmpty(text))
                {
                    SendError(Id, "HATA: Buraya boş bir değer giremezsin. Geçerli bir miktar gir.", "Transfer_ErrorMessage_0", 5);
                    return;
                }

                if (!uint.TryParse(text, out var result))
                {
                    SendError(Id, "HATA: Bu alana sadece geçerli bir miktar girebilirsin.", "Transfer_ErrorMessage_0", 5);
                    return;
                }

                if (screen.Account.Price < result)
                {
                    SendError(Id, $"HATA: Üzgünüm ancak hesabında bu kadar para yok. Mevcut bakiyen: ${screen.Account.Price}", "Transfer_ErrorMessage_0", 5);
                    return;
                }

                screen.Response1 = text;
                SendError(Id, $"<color=green> Fiyat geçerli geçerli.</color>", "Transfer_ErrorMessage_0", 10);
                return;
            }

            if (buttonName == "Cek_Input")
            {
                if (!uint.TryParse(text, out var result))
                {
                    SendError(Id, "HATA: Bu alana sadece geçerli bir miktar girebilirsin.", "Cek_ErrorMessage_0", 5);
                    return;
                }

                if (screen.Account.Price < result)
                {
                    SendError(Id, $"HATA: Üzgünüm ancak hesabında bu kadar para yok. Mevcut bakiyen: ${screen.Account.Price}", "Cek_ErrorMessage_0", 5);
                    return;
                }

                screen.Response1 = text;
                SendError(Id, $"<color=green> Miktar geçerli.</color>", "Cek_ErrorMessage_0", 10);
                return;
            }

            if (buttonName == "Yatir_Input")
            {
                if (!uint.TryParse(text, out var result))
                {
                    SendError(Id, "HATA: Bu alana sadece sayı girebilirsin.", "Yatir_ErrorMessage_0", 5);
                    return;
                }

                if (untPlayer.Experience < result)
                {
                    SendError(Id, $"HATA: Üzgünüm ancak üzerinde bu kadar para yok. Mevcut bakiyen: ${untPlayer.Experience}", "Yatir_ErrorMessage_0", 5);
                    return;
                }

                screen.Response1 = text;
                SendError(Id, $"<color=green> Miktar geçerli.</color>", "Yatir_ErrorMessage_0", 10);
                return;
            }
        }

        public static void SendError(CSteamID Id, string text, string child, float time)
        {
            EffectManager.sendUIEffectText(Key, Id, true, child, text);
            Main.Instance.StartCoroutine(ErrorDuration(Id, child, time));
        }

        private static IEnumerator ErrorDuration(CSteamID Id, string child, float time)
        {
            yield return new WaitForSeconds(time);
            EffectManager.sendUIEffectText(Key, Id, true, child, " ");
        }

        public static void ShowMainUI(Player player, Account account)
        {
            SetModal(player, true);
            var Id = player.channel.owner.playerID.steamID;
            var untPlayer = UnturnedPlayer.FromPlayer(player);

            EffectManager.sendUIEffectText(Key, Id, true, "BNK_Name", untPlayer.DisplayName);
            EffectManager.sendUIEffectText(Key, Id, true, "BNK_Price", account.Price.ToString());
            EffectManager.sendUIEffectText(Key, Id, true, "BNK_X", "<color=orange>CÜZDAN:</color> " + untPlayer.Experience.ToString());
            EffectManager.sendUIEffectVisibility(Key, Id, true, "Main", true);

            MultipleVisibility(Id, new string[]
            {
                "Select_Card",
                "Select_Card_Add",
                "Deposit_Money",
                "Withdraw_Money",
                "Transfer_Money"
            });

        }

        public static void ShowCardsUI(Player player)
        {
            SetModal(player, true);
            var Id = player.channel.owner.playerID.steamID;
            MultipleVisibility(Id, new string[]
            {
                "Main",
                "Select_Card_Add",  
                "Deposit_Money",
                "Withdraw_Money",
                "Transfer_Money"
            });
            var screen = GetScreenOrCreate(Id);
            if (!screen.ISended)
            {
                EffectManager.sendUIEffect(Main.Instance.Configuration.Instance.Effect, Key, Id, true);
                screen.ISended = true;
            }
            var account = GetNext(screen.Accounts, screen.Page);
            if (account == null)
            {
                ShowAddCardUI(player);
                return;
            }
            EffectManager.sendUIEffectText(Key, Id, true, "BNK_Select_Card_Text", $" <color=grey>ID:</color> {account.AccountId} | <color=grey>BAKIYE:</color> <color=orange>$</color>{account.Price}");
            EffectManager.sendUIEffectVisibility(Key, Id, true, "Select_Card", true);
        }

        public static void ShowAddCardUI(Player player)
        {
            SetModal(player, true);
            var Id = player.channel.owner.playerID.steamID;
            // EffectManager.sendUIEffect(Main.Instance.Configuration.Instance.Effect, Key, Id, true);
            EffectManager.sendUIEffectVisibility(Key, Id, true, "Select_Card_Add", true);

            MultipleVisibility(Id, new string[]
            {
                "Select_Card",
                "Main",
                "Deposit_Money",
                "Withdraw_Money",
                "Transfer_Money"
            });
        }

        public static void MultipleVisibility(CSteamID Id, string[] close)
        {
            foreach (var s in close)
            {
                EffectManager.sendUIEffectVisibility(Key, Id, true, s, false);
            }
        }

        public static void CloseUI(Player player)
        {
            SetModal(player, false);
            Screens.RemoveAll(screen => screen.Id == player.channel.owner.playerID.steamID);
            EffectManager.askEffectClearByID(Main.Instance.Configuration.Instance.Effect, player.channel.owner.playerID.steamID);
        }

        public static Screen GetScreenOrCreate(CSteamID Id)
        {
            var screen = Screens.FirstOrDefault(s => s.Id == Id);
            if (screen == null)
            {
                screen = new Screen(Id, 1, null);
                screen.Accounts = Main.Instance.Configuration.Instance.Accounts.Where(ac => ac.Id == Id.m_SteamID)
                    .ToList();
                Screens.Add(screen);
            }
            return screen;
        }

        public static Account GetNext(List<Account> list, int page) => list.Skip(page - 1).Take(1).FirstOrDefault();
        public static void SetModal(Player player, bool activity) =>
            player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, activity);
    }
}