using SteamDatabase.ValvePak;
using System.IO.Enumeration;

//Clear the console
Console.Clear();
Console.ResetColor();

//Maximize console window
//Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

//Write a welcome message
Console.WriteLine("Counter-Strike 2 VPK map packer v1.0");
Console.WriteLine("Created by Francesc Pàez (C) 2023");
Console.WriteLine("-------------------------------------------");
Console.WriteLine("Download the latest version on GitHub: https://github.com/fpaezf/cs2-vpk-map-packer");
Console.WriteLine("");


if (args.Length == 0) {         //User has launched the application without arguments, throw a warning message and exit application
    Console.BackgroundColor = ConsoleColor.DarkRed;
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("FAIL:");
    Console.ResetColor();
    Console.Write(" No arguments detected!");
    Console.WriteLine("");
    Console.WriteLine("");
    Console.WriteLine("Launch this application again but using 2 command line arguments with quotes:");
    Console.WriteLine("MapPacker.exe \"c:\\sourcefolder\\mymap\" \"c:\\targetfolder\\mymap.vpK\"");
    Console.WriteLine("");
    Console.WriteLine("Hit a key to exit.");
    Console.ReadKey(true);
    return;
} else if (args.Length == 1) {   //User has launched the application with only one argument, throw a warning message and exit application
    Console.BackgroundColor = ConsoleColor.DarkRed;
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("FAIL:");
    Console.ResetColor();
    Console.Write(" only 1 argument detected!");
    Console.WriteLine("");
    Console.WriteLine("");
    Console.WriteLine("Launch this application again but using 2 command line arguments with quotes:");
    Console.WriteLine("MapPacker.exe \"c:\\sourcefolder\\mymap\" \"c:\\targetfolder\\mymap.vpK\"");
    Console.WriteLine("");
    Console.WriteLine("Hit a key to exit.");
    Console.ReadKey(true);
    return;
} else if (args.Length == 2){     //User has launched the application with 2 arguments, show pass message and continue
    Console.BackgroundColor = ConsoleColor.DarkYellow;
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("Source folder:");
    Console.ResetColor();
    Console.Write(" " + args[0]);
    Console.WriteLine("");
    Console.BackgroundColor = ConsoleColor.DarkYellow;
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("Target file:");
    Console.ResetColor();
    Console.Write(" " + args[1]);
    Console.WriteLine("");
    Console.WriteLine("");

    if (Directory.Exists(args[0])) { //Check if provided directory exists and if so, show a pass message and continue
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("PASS:");
        Console.ResetColor();
        Console.Write(" Source directory is accesible.");
        Console.WriteLine("");
    } else if (!Directory.Exists(args[0])){ //If source folder not exists, show a warning message and exit
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("FAIL:");
        Console.ResetColor();
        Console.Write(" Source directory is not accesible.");
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("Hit a key to exit.");
        Console.ReadKey(true);
        return;
    }

    if (Path.GetExtension(args[1]).Contains(".vpk")) { //If target file contains .vpk file extension, continue.
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("PASS:");
        Console.ResetColor();
        Console.Write(" Target file is a valid .vpk file.");
        Console.WriteLine("");       
    } else if (!Path.GetExtension(args[1]).Contains(".vpk")) { //If target file not contains .vpk file extension, fail and exit
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("FAIL:");
        Console.ResetColor();
        Console.Write(" Target file is not a valid .vpk file.");
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("Hit a key to exit.");
        Console.ReadKey(true);
        return;
    }
    PackVpkFromDirectory(args[0]);  //Call the Pack folder routine
}

void PackVpkFromDirectory(string dirPath)
{
    string vpkPath = args[1];    

    if (File.Exists(vpkPath))
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("WORK:");
        Console.ResetColor();
        Console.Write($" Deleting existing {vpkPath} file...");
        Console.WriteLine("");
        File.Delete(vpkPath);
    }

    Console.BackgroundColor = ConsoleColor.Blue;
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("WORK:");
    Console.ResetColor();
    Console.Write(" Packing all files and folders...");
    Console.WriteLine("");

    Package vpk = new Package();
    var mapFiles = new FileSystemEnumerable<string>(dirPath,(ref FileSystemEntry entry) => entry.ToSpecifiedFullPath(),new EnumerationOptions {RecurseSubdirectories = true,});
    uint fileCount = 0;
    int vpkSize = 0;

    foreach (var file in mapFiles)
    {
        if (!File.Exists(file))
            continue;
        var name = file[(dirPath.Length + 1)..];
        var data = File.ReadAllBytes(file);
        vpk.AddFile(name, data);
        fileCount++;
        vpkSize += data.Length;
    }

    Console.BackgroundColor = ConsoleColor.Blue;
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("WORK:");
    Console.ResetColor();
    Console.Write(" Verifying checksums...");
    Console.WriteLine("");
    vpk.VerifyFileChecksums(); 
    
    vpk.Write(vpkPath);

    Console.BackgroundColor = ConsoleColor.Blue;
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("WORK:");
    Console.ResetColor();
    Console.Write(" Performing cleanup...");
    Console.WriteLine("");

    vpk.Dispose();
   
    Console.BackgroundColor = ConsoleColor.DarkGreen;
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("PASS:");
    Console.ResetColor();
    Console.Write($" Wrote {Path.GetFileName(vpkPath)} with {fileCount} files totalling {vpkSize} bytes.");
    Console.WriteLine("");
    Console.WriteLine("");
    Console.WriteLine("Hit a key to exit.");
    Console.ReadKey(true);
    return;  
}





















