using System.Collections.Generic;

namespace eden.Game.Gameplay.Items
{
    public class Inventory
    {
        /// <summary>
        /// All the Items in this inventory
        /// </summary>
        public readonly List<Item> Items = new List<Item>();

        /// <summary>
        /// Has single items with their StackCount set to the total from all mathcing items in Items
        /// </summary>
        // ReSharper disable once CollectionNeverUpdated.Global todo
        public readonly List<Item> CombinedItemList = new List<Item>();

        /// <summary>
        /// Adds and Item to this inventory, can ignore max item count
        /// </summary>
        public bool Add(Item item, bool ignoreMaxItemCount = false)
        {
            int count = item.StackSize;

            foreach (Item i in Items)
                if (i.ItemID == item.ItemID)
                    count += i.StackSize;

            if (count < item.MaxStackSize || ignoreMaxItemCount)
            {
                Items.Add(item);
                //TODO: remove from world here
                return true;
            }
            else if (count - item.StackSize < item.MaxStackSize)
            {
                item.StackSize -= count - item.MaxStackSize;
                Items.Add(item);
                //TODO: add item with less contents to world here
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes an item from this inventory
        /// </summary>
        /// <param name="item"></param>
        public void Remove(Item item)
        {
            Items.Remove(item);
            //TODO: add to world here
        }
    }
}
