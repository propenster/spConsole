using SpConsole;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

internal class Program
{

    private static string EncryptString(string input)
    {
        var output = string.Empty;
        byte[] array;
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        var clearBytes = Encoding.UTF8.GetBytes(input);
        using (Aes aes = Aes.Create())
        {
            aes.BlockSize = 128;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.None;
            aes.Key = Encoding.UTF8.GetBytes("abcd1245678901f4");
            var encryptor = aes.CreateEncryptor(aes.Key, new byte[16]);
            using (var memStream = new MemoryStream())
            {
                using (var cs = new CryptoStream((Stream)memStream, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                output = Convert.ToBase64String(memStream.ToArray());

            }
        }

        return output;
    }

    /// <summary>
    /// Encrypts a given string using AES encryption with a predefined key.
    /// </summary>
    /// <param name="input">The string to encrypt.</param>
    /// <returns>The encrypted string in Base64 format.</returns>
    private static string Encrypt(string input)
    {
        // Return an empty string if the input is null, empty, or consists only of white-space characters.
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Convert the input string to a byte array.
        byte[] clearBytes = Encoding.UTF8.GetBytes(input);

        // Initialize AES encryption.
        using (Aes aes = Aes.Create())
        {
            aes.BlockSize = 128;       // Set the block size to 128 bits.
            aes.Mode = CipherMode.ECB; // Use Electronic Codebook mode.
            aes.Padding = PaddingMode.PKCS7; // Use PKCS7 padding to ensure complete blocks.

            // Set a predefined key (16 bytes = 128 bits).
            aes.Key = Encoding.UTF8.GetBytes("abcd1245678901f4");

            // Create an encryptor object using the current AES object settings.
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, new byte[16]);

            // Create a memory stream to hold the encrypted data.
            using (MemoryStream memStream = new MemoryStream())
            {
                // Create a CryptoStream, write the data, and encrypt it.
                using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(clearBytes, 0, clearBytes.Length);
                    cryptoStream.FlushFinalBlock();
                }

                // Convert the encrypted data from the memory stream to a Base64 string.
                return Convert.ToBase64String(memStream.ToArray());
            }
        }
    }

    /// <summary>
    /// Encrypts a given string using AES encryption with a predefined key.
    /// </summary>
    /// <param name="input">The string to encrypt.</param>
    /// <returns>The encrypted string in Base64 format.</returns>
    private static string Decrypt(string input)
    {
        // Return an empty string if the input is null, empty, or consists only of white-space characters.
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }
        var output = string.Empty;
        // Convert the input string to a byte array.
        byte[] clearBytes = Encoding.UTF8.GetBytes(input);

        // Initialize AES encryption.
        using (Aes aes = Aes.Create())
        {
            aes.BlockSize = 128;       // Set the block size to 128 bits.
            aes.Mode = CipherMode.ECB; // Use Electronic Codebook mode.
            aes.Padding = PaddingMode.PKCS7; // Use PKCS7 padding to ensure complete blocks.

            // Set a predefined key (16 bytes = 128 bits).
            aes.Key = Encoding.UTF8.GetBytes("abcd1245678901f4");

            // Create an encryptor object using the current AES object settings.
            ICryptoTransform encryptor = aes.CreateDecryptor(aes.Key, new byte[16]);

            // Create a memory stream to hold the encrypted data.
            using (MemoryStream memStream = new MemoryStream(clearBytes))
            {
                // Create a CryptoStream, write the data, and encrypt it.
                using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cryptoStream)) output = sr.ReadToEnd();

                }

                // Convert the encrypted data from the memory stream to a Base64 string.
            }
        }

        return output;
    }

    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var text = File.ReadAllText("scores.txt");
        var transformed = text.ReplaceLineEndings("\",\"");


        //kcX3X7HHfmQV9fNXHIVCLQ==
        //var encryptEmike = Encrypt("Emike@95");
        //Console.WriteLine(encryptEmike);
        //CreateDataCsv();

        var engine = new Engine(0);
        //Console.WriteLine("Current seed: {0}", engine);
        var input = string.Empty;

        while (true)
        {
            Console.Write("Enter lines separated by a space e.g 1.20 3.50 4.25: ");
            input = Console.ReadLine();
            if (input == "cls") { Console.Clear(); continue; }

            RunHighestScoringHalf(engine, input);

            //RunHighestScoringHalfQuadratic(input);

            //RunDrawCatcher(input);

            //Run2HalfHighestScoring(input);

            //RunQuadraticFormulaMethod(input);

        }

        static void RunQuadraticFormulaMethod(string? input)
        {
            var equation = SolveQuadraticEquation(input);
            Console.WriteLine("Result >>> {0}", equation);

        }

        static void RunHighestScoringHalf(Engine engine, string? input)
        {
            var result = engine.GetScore(input ?? string.Empty);
            Console.WriteLine("Result of {0} => {1} with confidence percent ({2}/{3}): {4:N2}%", input, result.score, result.numerator, result.denominator, result.percentage);
        }


        //var list = new List<Tuple<float, float, float, int>>()
        //{
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),
        //    new Tuple<float, float, float, int>(3.30f, 2.10f, 3.00f, 2),
        //    new Tuple<float, float, float, int>(3.10f, 1.89f, 4.25f, 2),
        //    new Tuple<float, float, float, int>(3.30f, 2.10f, 3.25f, 1),

        //    new Tuple<float, float, float, int>(3.40f, 2.15f, 3.10f, 1),
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),
        //    new Tuple<float, float, float, int>(3.10f, 1.99f, 3.50f, 2),

        //};

    }

    private static void RunHighestScoringHalfQuadratic(string? input)
    {
        var sum = input.Split(" ").Sum(x => float.Parse(x.Trim()));

        Console.WriteLine("Sum of real numbers >>> {0}", sum);

    }
    //14, 50, 52, 52, 2, 0, 7, 36, 77, 52, 48, 71, 44, 44, 72, 60, 44, 49, 88, 39, 44, 57*, 60, 17, 17, 50, 52, 52, 32, 
    //34, 7, 88*, 
    private static int[] _2Half = new int[] { 6, 42, 51, 19, 59, 66, 24, 80, 18, 61, 88, 82, 67, 73, 61, 55, 34, 42, 57, 47, 67, 55, 89, 42,  };
    private static void Run2HalfHighestScoring(string? input)
    {
        var nums = input.Split(" ");

        var sum = nums.Sum(x => float.Parse(x));
        var res = GetFloatRemainder(sum);
        var condition = _2Half.Contains(res);
        Console.WriteLine("Sum = {0} 2Half >>> {1}", res, condition ? "YES" : "NO");
    }

    private static string SolveQuadraticEquation(string input)
    {
        // Split the input string and convert to float
        string[] parts = input.Split(' ');
        if (parts.Length != 3)
        {
            throw new ArgumentException("Input must contain exactly three space-separated values for a, b, and c.");
        }

        float a = float.Parse(parts[0], CultureInfo.InvariantCulture);
        float b = float.Parse(parts[1], CultureInfo.InvariantCulture);
        float c = float.Parse(parts[2], CultureInfo.InvariantCulture);

        // Calculate the discriminant
        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            // Calculate the complex roots
            float realPart = -b / (2 * a);
            float imaginaryPart = MathF.Sqrt(-discriminant) / (2 * a);
            return string.Format(CultureInfo.InvariantCulture,
                "{0:0.000000} + {1:0.000000}i, {0:0.000000} - {1:0.000000}i",
                realPart, imaginaryPart);
        }
        else
        {
            // Calculate the real roots
            float root1 = (-b + MathF.Sqrt(discriminant)) / (2 * a);
            float root2 = (-b - MathF.Sqrt(discriminant)) / (2 * a);
            return string.Format(CultureInfo.InvariantCulture,
                "{0:0.000000}, {1:0.000000}",
                root1, root2);
        }
    }

    private static int GetFloatRemainder(float number)
    {
        float fractionalPart = number - (int)number;
        int rem = (int)(fractionalPart * 100);

        return rem;
    }
    private static void RunDrawCatcher(string? input)
    {
        var pool = input.Split("_");
        var first = pool[0].Split(",");
        var second = pool[1].Split(" ");

        var condition1 = first.Select(x => float.Parse(x)).Min() == float.Parse(first[0]);
        var rem = int.Parse(second.Select(x => float.Parse(x)).Sum().ToString().Split(".")[1]);
        var condition2 = rem % 3 == 0;

        var last = int.Parse(first[1].Split(".")[1]);

        var condition3 = rem % 3 == 0 || last % 3 == 0;

        var isDrawPossible = IsDrawPossible(condition1, condition2, condition3);

        if (!isDrawPossible) { Console.WriteLine(">>> Draw NO"); return; }

        Console.WriteLine(">>> Draw YES");


    }
    private static bool IsDrawPossible(bool condition1, bool condition2, bool condition3)
    {
        return (condition1 && condition2 && condition3);
    }

    private static void CreateDataCsv()
    {
        var engine = new Engine();
        var list = new List<string>();
        list.AddRange(engine.GetSeedV1());
        list.AddRange(engine.GetSeedV2());

        var csv = new CsvWriter(list);
        csv.CreateCsv();
    }
}
