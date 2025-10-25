namespace MailClient.Toolkit.Utilities
{
    public class FileUtilities
    {
        public static string FileExplorer(string startPath)
        {
            string currentDir = startPath;

            int offset = 0;

            int maxDisplay = 10;

            while (true)
            {
                Console.Clear();

                Console.WriteLine(currentDir);

                string[] dirs = Directory.GetDirectories(currentDir);

                string[] dirFiles = Directory.GetFiles(currentDir);

                List<string> files = new(dirs.Concat(dirFiles));

                string[] fileNames = new string[files.Count];

                int min = Math.Clamp(0 + offset * maxDisplay, 0, files.Count - 1);

                int max = Math.Clamp(min + maxDisplay, 0, files.Count);

                for (int i = min; i < max; i++)
                {
                    fileNames[i] = Path.GetFileName(files[i]);
                    Console.WriteLine("\t" + (i - min) + ": " + fileNames[i]);
                }

                //Gets pressed key

                ConsoleKeyInfo key = Console.ReadKey();

                if (char.IsDigit(key.KeyChar))
                {
                    int digit = key.KeyChar - '0' + min;

                    if (Path.HasExtension(fileNames[digit]))
                    {
                        return files[digit];
                    }

                    currentDir = files[digit];
                    offset = 0;
                    continue;
                }

                //Moves page or goes back one directory

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        offset = Math.Clamp(offset - 1, 0, (int)Math.Ceiling((decimal)(files.Count / 9)));
                        break;
                    case ConsoleKey.RightArrow:
                        offset = Math.Clamp(offset + 1, 0, (int)Math.Ceiling((decimal)(files.Count / 9)));
                        break;
                    case ConsoleKey.UpArrow:
                        DirectoryInfo? parent = Directory.GetParent(currentDir);
                        if (parent != null)
                        {
                            offset = 0;
                            currentDir = parent.FullName;
                        }
                        break;
                }
            }
        }
    }
}
