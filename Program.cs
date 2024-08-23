using System;

string[][] reelSets =
[
    ["sym2", "sym7", "sym7", "sym1", "sym1", "sym5", "sym1", "sym4", "sym5", "sym3", "sym2", "sym3", "sym8", "sym4", "sym5", "sym2", "sym8", "sym5", "sym7", "sym2"],
    ["sym1", "sym6", "sym7", "sym6", "sym5", "sym5", "sym8", "sym5", "sym5", "sym4", "sym7", "sym2", "sym5", "sym7", "sym1", "sym5", "sym6", "sym8", "sym7", "sym6", "sym3", "sym3", "sym6", "sym7", "sym3"],
    ["sym5", "sym2", "sym7", "sym8", "sym3", "sym2", "sym6", "sym2", "sym2", "sym5", "sym3", "sym5", "sym1", "sym6", "sym3", "sym2", "sym4", "sym1", "sym6", "sym8", "sym6", "sym3", "sym4", "sym4", "sym8", "sym1", "sym7", "sym6", "sym1", "sym6"],
    ["sym2", "sym6", "sym3", "sym6", "sym8", "sym8", "sym3", "sym6", "sym8", "sym1", "sym5", "sym1", "sym6", "sym3", "sym6", "sym7", "sym2", "sym5", "sym3", "sym6", "sym8", "sym4", "sym1", "sym5", "sym7"],
    ["sym7", "sym8", "sym2", "sym3", "sym4", "sym1", "sym3", "sym2", "sym2", "sym4", "sym4", "sym2", "sym6", "sym4", "sym1", "sym6", "sym1", "sym6", "sym4", "sym8"]
];

Dictionary<string, int[]> paytable = new Dictionary<string, int[]>
    {
        { "sym1", [1,2,3] },
        { "sym2", [1,2,3] },
        { "sym3", [1,2,5] },
        { "sym4", [2,5,10] },
        { "sym5", [5,10,15] },
        { "sym6", [5,10,15]},
        { "sym7", [5,10,20]},
        { "sym8", [10,20,50] }
    };

Random random = new();
int[] stopPositions = new int[5];
string[,] screen = new string[3, 5];

// Generate stop positions and screen
for (int i = 0; i < 5; i++)
{
    stopPositions[i] = random.Next(reelSets[i].Length);
    for (int j = 0; j < 3; j++)
    {
        screen[j, i] = reelSets[i][(stopPositions[i] + j) % reelSets[i].Length];
    }
}

// Display stop positions
Console.WriteLine("Stop Positions: " + string.Join(", ", stopPositions));

//  screen = new string[3, 5]
// {
//     { "sym3", "sym3", "sym3", "sym1", "sym1" },
//     { "sym3", "sym3", "sym3", "sym2", "sym2" },
//     { "sym3", "sym3", "sym3", "sym5", "sym5" }
// };

// Display screen
Console.WriteLine("Screen:");
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 5; j++)
    {
        Console.Write(screen[i, j] + " ");
    }
    Console.WriteLine();
}
// Calculate winnings
int totalWins = 0;
List<List<string>> winDetails = [];

// left most column
int leftColumn = 0;
for (int i = 0; i < 3; i++)
   {
    string symbol = screen[i, leftColumn];
    bool isPresentInNextReel = true;
    int count = 1;
    int k = leftColumn + 1;
    List<int> positions = [];
    positions.Add(leftColumn + (i * 5));
    while (isPresentInNextReel) {
        isPresentInNextReel = false;
        if (k == 5) {
            break;
        }
        for (int l = 0; l < 3; l++)
        {
            if (screen[l,k] == symbol)
            {
                isPresentInNextReel = true;
                positions.Add((l * 5) + k);
                k++;
                count++;
                break;
            }
        }
    }
    if (count >= 3)
    {
        int[] pay = paytable[symbol];
        int win = pay[count - 3];
        totalWins += win;
        // check if the win is already added or subset of another win
        bool isWinAdded = false;
        foreach (var detail in winDetails)
        {
            if (detail[0] == symbol && string.Join("", detail[3].Split(", ")).Contains(string.Join("", positions)))
            {
                isWinAdded = true;
                break;
            }
        }
        if(!isWinAdded)
        {
            winDetails.Add([symbol, count.ToString(), win.ToString(), string.Join(", ", positions), win.ToString()]);
        }
    }
   }

// Display winnings
Console.WriteLine("Total wins: " + totalWins);
foreach (var detail in winDetails)
{
    Console.WriteLine($"- Ways win {string.Join("-", detail[3].Split(", "))}, {detail[0]} x{detail[1]}, {detail[4]}");
}
