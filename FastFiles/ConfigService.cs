namespace FastFiles;

public class ConfigService
{
    private readonly string m_FeaturePath;
    private readonly string m_FeatureNamespace;
    
    private readonly string m_ConfigPath;
    private readonly string m_NamespaceString;
    
    private string m_ConfigName;

    private readonly string m_ModelPath;
    private readonly string m_ConverterPath;
    private readonly string m_ServicePath;
    private readonly string m_LoaderPath;
    private readonly string m_DtoPath;
    private readonly string m_ContentPath;
    private readonly string m_ExternalPath;
    private readonly string m_RuntimePath;

    public ConfigService(string featurePath, string featureNamespace)
    {
        m_FeaturePath = featurePath;
        m_FeatureNamespace = featureNamespace;
        
        m_ContentPath = Path.Combine(featurePath, "Content");
        m_ExternalPath = Path.Combine(featurePath, "External");
        m_RuntimePath = Path.Combine(featurePath, "Runtime");
        
        m_ConfigPath = Path.Combine(m_ExternalPath, "Config");
        m_NamespaceString = $"{featureNamespace}";

        m_ModelPath = Path.Combine(m_ConfigPath, "Model");
        m_ConverterPath = Path.Combine(m_ConfigPath, "Converter");
        m_ServicePath = Path.Combine(m_ConfigPath, "Service");
        m_LoaderPath = Path.Combine(m_ConfigPath, "Loader");
        m_DtoPath = Path.Combine(m_ConfigPath, "Dto");
    }

    public void ConfigHandle()
    {
        SelectConfigName();
        
        
        Directory.CreateDirectory(m_ConfigPath);
        Directory.CreateDirectory(m_ModelPath);
        Directory.CreateDirectory(m_ConverterPath);
        Directory.CreateDirectory(m_ServicePath);
        Directory.CreateDirectory(m_LoaderPath);
        Directory.CreateDirectory(m_DtoPath);

        CreateJson();
        CreateConverter();
        CreateConfig();
        CreateService();
        CreateLoader();
        CreateDto();
    }
    
    private void SelectConfigName()
    {
        Console.WriteLine("Print Config Name (str):");
        Console.Write("> ");
        
        m_ConfigName = Console.ReadLine();
    }

    private void CreateJson()
    {
        var directoryPath = Path.Combine(m_ContentPath, "Configs");
        var path = Path.Combine(directoryPath, $"{m_ConfigName}Config.json");
        if (Directory.Exists(directoryPath) && File.Exists(path))
        {
            return;
        }

        Directory.CreateDirectory(m_ContentPath);
        Directory.CreateDirectory(directoryPath);
        
        File.WriteAllText(path, "{\n}");
    }

    private void CreateConverter()
    {
        var className = $"{m_ConfigName}DtoToConfigConverter";
        var path = Path.Combine(m_ConverterPath, $"{className}.cs");
        
        var template = File.ReadAllText(".\\Templates\\Converter.txt");
        var content = template.Replace("{featureNamespace}", m_FeatureNamespace);
        content = content.Replace("{config}", m_ConfigName);
        
        File.WriteAllText(path, content);
    }

    private void CreateConfig()
    {
        var className = $"{m_ConfigName}Config";
        var path = Path.Combine(m_ModelPath, $"{className}.cs");
        
        var template = File.ReadAllText(".\\Templates\\Config.txt");
        var nameSpace = $"{m_NamespaceString}.External.Config.Model";
        var content = string.Format(template, nameSpace, className, className);
        
        File.WriteAllText(path, content);
    }

    private void CreateService()
    {
        var className = $"{m_ConfigName}ConfigService";
        var path = Path.Combine(m_ServicePath, $"{className}.cs");
        
        var template = File.ReadAllText(".\\Templates\\Service.txt");
        var content = template.Replace("{featureNamespace}", m_FeatureNamespace);
        content = content.Replace("{config}", m_ConfigName);
        
        File.WriteAllText(path, content);
    }

    private void CreateLoader()
    {
        var className = $"{m_ConfigName}ConfigLoader";
        var path = Path.Combine(m_LoaderPath, $"{className}.cs");
        
        var template = File.ReadAllText(".\\Templates\\Loader.txt");
        var content = template.Replace("{featureNamespace}", m_FeatureNamespace);
        content = content.Replace("{config}", m_ConfigName);
        
        File.WriteAllText(path, content);
    }

    private void CreateDto()
    {
        var className = $"{m_ConfigName}ConfigDto";
        var path = Path.Combine(m_DtoPath, $"{className}.cs");
        
        var template = File.ReadAllText(".\\Templates\\Dto.txt");
        var nameSpace = $"{m_NamespaceString}.External.Config.Dto";
        var content = string.Format(template, nameSpace, className);
        
        File.WriteAllText(path, content);
    }
}