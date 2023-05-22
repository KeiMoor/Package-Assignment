using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string filePath = @"C:\Packages\Input.txt"; // The path to the input file

        string[] lines = File.ReadAllLines(filePath); // Read the input file

        foreach (string line in lines)
        {
            string[] parts = line.Split(':');// Splits the line into weight limit and item descriptions
            int weightLimit = int.Parse(parts[0].Trim());

            List<Item> items = ParseItems(parts[1]);// Parse the item descriptions

            List<Item> selectedItems = Knapsack(items, weightLimit);// Solve the knapsack problem

            if (selectedItems.Count > 0)// Display the output
            {
                Console.WriteLine(string.Join(", ", selectedItems.Select(item => item.Index)));
            }
            else
            {
                Console.WriteLine("-");
            }
        }
    }
    static List<Item> ParseItems(string itemsString)
    {
        List<Item> items = new List<Item>();
       
        string[] itemStrings = itemsString.Split(new[] { '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries); // Splits the string into individual item descriptions

        foreach (string itemString in itemStrings)
        {           
            string[] parts = itemString.Split(','); // Splits the item description into index, weight, and amount

            int index = int.Parse(parts[0]);
            double weight = double.Parse(parts[1].Replace('.', ','));
            int amount = int.Parse(parts[2].Substring(1)); // Removes the currency symbol

            items.Add(new Item(index, weight, amount));
        }

        return items;
    }
    static List<Item> Knapsack(List<Item> items, int weightLimit)
    {
        int numOfItems = items.Count;
        int[,] table = new int[numOfItems + 1, weightLimit + 1];

        for (int i = 1; i <= numOfItems; i++)
        {
            Item currentItem = items[i - 1];

            for (int j = 1; j <= weightLimit; j++)
            {
                if (currentItem.Weight > j)
                {
                    table[i, j] = table[i - 1, j];
                }
                else
                {
                    table[i, j] = Math.Max(
                        table[i - 1, j],
                        table[i - 1, j - (int)currentItem.Weight] + currentItem.Amount
                    );
                }
            }
        }

        List<Item> selectedItems = new List<Item>();

        int remainingWeight = weightLimit;
        for (int i = numOfItems; i > 0; i--)
        {
            if (table[i, remainingWeight] != table[i - 1, remainingWeight])
            {
                Item selectedItem = items[i - 1];
                selectedItems.Add(selectedItem);
                remainingWeight -= (int)selectedItem.Weight;
            }
        }

        selectedItems.Reverse();
        return selectedItems;
    }
}
class Item
{
    public int Index { get; }
    public double Weight { get; }
    public int Amount { get; }

    public Item(int index, double weight, int amount)
    {
        Index = index;
        Weight = weight;
        Amount = amount;
    }
}