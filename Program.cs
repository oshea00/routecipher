if (args.Count() < 1)
{
    System.Console.WriteLine("Usage:");
    System.Console.WriteLine("routecipher encipher | decipher");
    Environment.Exit(0);
}

try 
{
    var text = "";
    if (args[0] != "decipher") {
        System.Console.WriteLine("ENTER TEXT:");
        text = Console.ReadLine() ?? "";
    }
        
    var fillerwords = new List<string> {
        "GREAT","FILL","TO","SPLIT","MESSAGE","WORDS",
        "SEVEN","NOW","BLANK","RED","ROSES","BUSH",
        "MEN","YOUR","MEADOW","SUN","ROCK","EGG",
        "PLACE","BED","TREE","NORTH","WEST","GREEN",
        "ARM","PISTOL","BOMB","FRY","LIST","WRONG"
    };
    var words_placed = 0;
    var fills_placed = 0;
    var words = text.Split(' ').Select(t=>t.Trim()).ToList();
    (var rows, var cols) = GetKey();
    var slots = rows * cols.Count();

    if (words.Count>slots) {
        System.Console.WriteLine($"Message must be less than {slots} words.\nIt is {words.Count} long.");
        Environment.Exit(1);
    }

    if (slots > words.Count+fillerwords.Count) {
        System.Console.WriteLine($"specified key too large rows*cols={slots} must be <= {words.Count+fillerwords.Count}");
        Environment.Exit(1);
    }

    var matrix = new List<List<string>>(rows);

    for (var i=0; i<rows; i++) {
        matrix.Add(new List<string>(cols.Count));
        for (var j=0; j<cols.Count; j++) {
            matrix[i].Add(String.Empty);
        }
    }

    if (args[0]=="decipher")
    {
        System.Console.WriteLine($"ENTER CIPHER");
        text = Console.ReadLine();
        System.Console.WriteLine();
        if (!string.IsNullOrEmpty(text)) {
        words = text.Split(' ').Select(t=>t.Trim()).ToList();
        var currCol=0;
        words_placed=0;
        for (int j=0; j<cols.Count; j++) {
            if (cols[j]<0) {
                currCol = -cols[j]-1;
                for (int i=matrix.Count-1;i>=0;i--)
                {
                    matrix[i][currCol]=words[words_placed++];
                }
            }
            else
            {
                currCol = cols[j]-1;
                for (int i=0;i<matrix.Count;i++)
                {
                    matrix[i][currCol]=words[words_placed++];
                }
            }
        }
        System.Console.WriteLine("CODE MATRIX:");
        PrintMatrix(matrix);
        System.Console.WriteLine();

        }
    }
    else
    {
        for (int i=0; i<matrix.Count; i++) {
            for (int j=0; j<cols.Count; j++) {
                if (words_placed < words.Count)
                    matrix[i][j] = words[words_placed++].ToUpper();
                else
                    matrix[i][j] = fillerwords[fills_placed++];
            }
        }

        System.Console.WriteLine("CODE MATRIX:");
        PrintMatrix(matrix);
        System.Console.WriteLine();

        // print cipher
        System.Console.WriteLine("CIPHER:");
        var currCol=0;
        for (int j=0; j<cols.Count; j++) {
            if (cols[j]<0) {
                currCol = -cols[j]-1;
                for (int i=matrix.Count-1;i>=0;i--)
                {
                    System.Console.Write($"{matrix[i][currCol]} ");
                }
            }
            else
            {
                currCol = cols[j]-1;
                for (int i=0;i<matrix.Count;i++)
                {
                    System.Console.Write($"{matrix[i][currCol]} ");
                }
            }
        }
        System.Console.WriteLine();
    }


} 
catch (Exception e)
{
    System.Console.WriteLine($"Error: {e.Message}");
    Environment.Exit(1);
}

(int rows, List<int> cols) GetKey()
{
    // return (5, new List<int> {2,-1,3,-4});
    System.Console.WriteLine("ENTER KEY (r,c,-c,...):");
    var keyString = Console.ReadLine();
    if (string.IsNullOrEmpty(keyString))
        throw new Exception("Must enter non-blank key");
    var key = keyString.Split(',').Select(t=>Convert.ToInt32(t)).ToList();
    return (key[0],key.Skip(1).Take(key.Count-1).ToList());
} 

void PrintMatrix(List<List<string>> matrix)
{
    for (int i=0;i<matrix.Count;i++) {
        for (int j=0;j<matrix[i].Count;j++) {
            Console.Write($"{matrix[i][j]} ");
        }
        System.Console.WriteLine();
    }
}

