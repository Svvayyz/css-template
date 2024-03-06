Console.Title = "TemplateInstaller";

DateTime dtTimeBenchmark = DateTime.Now;

Console.WriteLine("Downloading!");

HttpClient hcHttpClient = new HttpClient();

if (File.Exists("css-template.zip"))
    File.Delete("css-template.zip");

Stream sStream = await hcHttpClient.GetStreamAsync("https://github.com/Svvayyz/css-template/raw/main/css-template.zip");
FileStream fsStream = new FileStream("css-template.zip", FileMode.CreateNew);

await sStream.CopyToAsync(fsStream);

fsStream.Close();
sStream.Close();

IEnumerable<string> mDirectories = Directory.GetDirectories(
    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
).Where(s => s.Contains("Visual Studio"));

foreach (string szDir in mDirectories)
{
    // Check if path is a valid visual studio path
    List<string> mDirs = Directory.GetDirectories(szDir).ToList(); // Converting to a list yeaaaaaa

    // mDirs.Contains("Template") didnt work for some reason .net core 7.0 is kinda weird
    if (mDirs.Where(s => s.Contains("Templates")).Count() < 1)
    {
        mDirectories = mDirectories.Where(s => s != szDir);

        continue;
    }

    mDirs = Directory.GetDirectories(szDir + "\\Templates").ToList();
    if (mDirs.Where(s => s.Contains("ProjectTemplates")).Count() < 1)
    {
        mDirectories = mDirectories.Where(s => s != szDir);

        continue;
    }

    // Yea after alldat checks we have found a valid visual studio directory (i guess)
    Console.WriteLine($"Found a valid Visual Studio installation in {szDir}");
}

// yea if after alldat we havent found a dir fuck it all and shutdown
if (mDirectories.Count() < 1)
{
    Console.WriteLine("A valid Visual Studio installation not found!");
    Console.ReadLine();

    return;
}

Console.WriteLine($"Found {mDirectories.Count()} Visual Studio installations, installing!");

// Install the fucking shit!
foreach (string szDir in mDirectories)
{
    string szFinalDir = szDir + "\\Templates\\ProjectTemplates";

    if (File.Exists(szFinalDir + "\\css-template.zip"))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error in {szFinalDir}, template is already installed!");
        Console.ForegroundColor = ConsoleColor.Gray;

        continue;
    }  

    File.Copy(
        "css-template.zip",
        szFinalDir + "\\css-template.zip"
    );

    Console.WriteLine($"Successfully installed to {szFinalDir}!");
}

TimeSpan tsDelta = DateTime.Now - dtTimeBenchmark;

// its really a formatting thing because , looks uglier 
string szTotalSeconds = tsDelta.TotalSeconds.ToString().Replace(',', '.');

Console.WriteLine($"\nFinished in {szTotalSeconds} second(s), press any key to exit!");

Console.ReadKey();