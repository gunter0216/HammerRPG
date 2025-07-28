namespace FastFiles;

class Program
{
    private static string m_FeaturePath;
    
    private static string m_FeatureNamespace;
    private static string m_FolderPath;
    
    private static string m_AppDirectory;
    private static string m_SelectedAppDirectory;
    private static string m_FeatureName;

    static void Main(string[] _)
    {
        m_AppDirectory = GetAppDirectory();
        
        PrintLn($"App Directory {m_AppDirectory}");
        
        PrintAppDirectoryFolders();

        SelectDirectory();

        SelectFeature();

        m_FeaturePath = Path.Combine(m_SelectedAppDirectory, m_FeatureName);
        m_FeatureNamespace = CreateNamespace(m_FeaturePath);

        SelectActions();
    }

    private static void SelectActions()
    {
        var actions = new List<(string name, Action func)>();
        actions.Add(("Create Base", CreateBaseAction));
        actions.Add(("Config", CreateConfigAction));
        
        PrintLn("Possible Actions:");
        int i = 0;
        foreach (var action in actions)
        {
            PrintLn($"{++i}. {action.name}");
        }
        
        PrintLn("Select actions (int with whitespace):");
        PrintLn("> ");

        var selectedActions = new List<int>();
        var input = Console.ReadLine();
        var split = input.Split(' ');
        foreach (var str in split)
        {
            var index = Convert.ToInt32(str);
            selectedActions.Add(index - 1);
        }
        
        selectedActions.Sort();

        ExecuteActions(actions, selectedActions);
    }

    private static void ExecuteActions(List<(string name, Action func)> actions, List<int> selectedActions)
    {
        foreach (var selectedAction in selectedActions)
        {
            var action = actions[selectedAction];
            Console.WriteLine($">>> Action \"{action.name}\"");
            action.func?.Invoke();
        }
    }

    private static void CreateBaseAction()
    {
        var service = new BaseService(m_FeaturePath, m_FeatureNamespace);
        service.CreateBase();
    }

    private static void CreateConfigAction()
    {
        var service = new ConfigService(m_FeaturePath, m_FeatureNamespace);
        service.ConfigHandle();
    }

    private static void SelectFeature()
    {
        PrintLn("Print Feature Name (str):");
        Print("> ");
        
        m_FeatureName = Console.ReadLine();
    }

    private static void SelectDirectory()
    {
        PrintLn("Select directory (number):");
        Print("> ");
        
        var appDirectories = Directory.GetDirectories(m_AppDirectory);
        var index = ReadInt();
        m_SelectedAppDirectory = appDirectories[index - 1];
    }

    private static int ReadInt()
    {
        var selectedDirectory = Console.ReadLine();
        return Convert.ToInt32(selectedDirectory);
    }

    private static void PrintAppDirectoryFolders()
    {
        var appDirectories = Directory.GetDirectories(m_AppDirectory);
        int i = 0;
        foreach (var directory in appDirectories)
        {
            PrintLn($"{++i}. {Path.GetFileName(directory)}");
        }
    }

    private static string GetAppDirectory()
    {
        string appDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName;
        appDirectory = Path.Combine(appDirectory, "Assets", "App");
        return appDirectory;
    }

    static string CreateNamespace(string fullPath)
    {
        const string marker = @"Assets\";

        int index = fullPath.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (index == -1)
            return string.Empty; // или бросить исключение

        // Обрезаем всё до "Assets\"
        string relativePath = fullPath.Substring(index + marker.Length);

        // Заменяем \ на .
        string dottedPath = relativePath.Replace("\\", ".");

        return dottedPath;
    }

    private static void PrintLn(string str)
    {
        Console.WriteLine(str);
    }
    
    private static void Print(string str)
    {
        Console.Write(str);
    }
}