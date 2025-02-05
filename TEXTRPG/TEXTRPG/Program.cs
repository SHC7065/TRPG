using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace TEXTRPG
{
    public class Game
    {
        public static void Main(string[] args)
        {
            ShowStartMenu();
        }

        public static void ShowStartMenu()
        {
            Status playerStatus = new Status("Player1", 1, 10, 5, 100, 200);
            MyInventory playerMyInventory = new MyInventory();
            Shop shop = new Shop();
            
            while (true)
            { 
                Console.Clear();
                Console.WriteLine("게임을 시작합니다");
                Console.WriteLine("이곳에서 정비를 할 수 있습니다");
                Console.WriteLine("1. 스테이터스");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("0. 종료");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            playerStatus.DisplayStatus();
                            break;

                        case 2:
                            playerMyInventory.DisplayMyInventory();
                            break;

                        case 3:
                            shop.BuyItem(playerStatus, playerMyInventory);
                            break;
                        
                        case 0:
                            Console.WriteLine("게임을 종료합니다");
                            return;

                        default:
                            Console.WriteLine("잘못된 입력입니다");
                            continue;
                    }
                }
                else
                {
                    Console.WriteLine("숫자를 입력하세요.");
                    continue;
                }

                Console.WriteLine("계속하려면 엔터를 누르세요");
                Console.ReadLine();
            }
        }
        public static void MyInventory()
        {
            Console.WriteLine("인벤토리");
        }
            

            public class Status
        { 
            public string Name { get; set; }
            public int Level { get; set; }
            public int Attack { get; set; }
            public int Defense { get; set; }
            public int Health { get; set; }
            public int Gold { get; set; }

            public Item EquippedWeapon { get; set; }
            public Item EquippedArmor { get; set; }

            public Status(string name, int level, int attack, int defense, int health, int gold)
            {
                Name = name;
                Level = level;
                Attack = attack;
                Defense = defense;
                Health = health;
                Gold = gold;
            }

            public void DisplayStatus()
            {
                Console.WriteLine($"이름: {Name}");
                Console.WriteLine($"레벨: {Level}");
                Console.WriteLine($"공격력: {Attack}");
                Console.WriteLine($"방어력: {Defense}");
                Console.WriteLine($"체력: {Health}");
                Console.WriteLine($"골드: {Gold}");

                Console.WriteLine($"[장착 중인 무기]: {(EquippedWeapon != null ? EquippedWeapon.Name : "없음")}");
                Console.WriteLine($"[장착 중인 방어구]: {(EquippedArmor != null ? EquippedArmor.Name : "없음")}");
            }

            public int GetTotalAttack()
            {
                return Attack + (EquippedWeapon?.Value ?? 0);
            }

            public int GetTotalDefense()
            {
                return Defense + (EquippedWeapon?.Value ?? 0);
            }
            
            public bool SpendGold(int amount)
            {
                if (Gold >= amount)
                {
                    Gold -= amount;
                    return true;
                }
                else
                {
                    Console.WriteLine("골드가 부족합니다");
                    return false;
                }
            }
        }
    }
    public class Item
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public Item(string name, string type, string value)
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public void DisplayItem()
        {
            Console.WriteLine($"아이템 이름: {Name}, 타입: {Type}, 가치: {Value} 골드");
        }
    }


        public class MyInventory
        {
            public List<Item> Items { get; set; }

            public MyInventory()
            {
                Items = new List<Item>();
            }

            public void AddItem(Item item)
            {
                Items.Add(item);
            }

            public void DisplayMyInventory()
            {
                if (Items.Count == 0)
                {
                    Console.WriteLine("인벤토리가 비어 있습니다");
                    return;
                }
                Console.WriteLine("인벤토리 목록");
                for (int i = 0; i < Items.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {Items[i].Name} ({Items[i].Type}) - 능력치: {Items[i].Value}");
                }
            }


        }
    

        public class ShopItem
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Type { get; set; }
        public bool IsPurchased { get; set; }

        public ShopItem(string name, int price, string type)
        {
            Name = name;
            Price = price;
            Type = type;
            IsPurchased = false;
        }
        public void DisplayItem()
        {
            string status = IsPurchased ? "(구매 완료)" : "";
            Console.WriteLine($"{Name} - {Price} 골드, 유형: {Type}");
        }
    }

        public class Shop
    {
        public List<ShopItem> ItemsForSale { get; set; }

        public Shop()
        {
            ItemsForSale = new List<ShopItem>
            {
                new ShopItem("힐링포션", 50, "소모품"),
                new ShopItem("철검", 200, "무기"),
                new ShopItem("갑옷", 200, "방어구")
            };
        }

        public void DisplayShopItems()
        {
            Console.WriteLine("상점에 있는 아이템 목록");
            for (int i = 0; i < ItemsForSale.Count; i++)
            {
                Console.Write($"{i + 1}.");
                ItemsForSale[i].DisplayItem();
            }
            Console.WriteLine("0. 나가기");
        }

        public void BuyItem(Game.Status playerStatus, MyInventory playerInventory)
        {
            while (true)
            {
                DisplayShopItems();
                Console.Write("구매할 아이템 번호를 입력하세요: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 0)
                    {
                        Console.WriteLine("상점을 종료합니다");
                        break;
                    }
                    
                    if (choice > 0 && choice <= ItemsForSale.Count)
                    {
                        ShopItem selectedItem = ItemsForSale[choice - 1];

                        if (selectedItem.IsPurchased)
                        {
                            Console.WriteLine($"이미 구매한 아이템입니다: {selectedItem.Name}");
                            continue;
                        }

                        if (playerStatus.SpendGold(selectedItem.Price))
                        {
                            playerInventory.AddItem(new Item(selectedItem.Name, selectedItem.Type, selectedItem.Price.ToString()));
                            selectedItem.IsPurchased = true;
                            Console.WriteLine($"{selectedItem.Name}을(를) 구매했습니다!");
                        }
                    }


                    else
                    {
                        Console.WriteLine("잘못된 선택입니다.");
                    }
                }
                else
                {
                    Console.WriteLine("숫자를 입력하세요.");

                }
            }
        }   
   } 
}
