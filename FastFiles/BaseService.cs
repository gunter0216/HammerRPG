namespace FastFiles;

public class BaseService
{
    private readonly string m_FeaturePath;
    private readonly string m_FeatureNamespace;

    public BaseService(string featurePath, string featureNamespace)
    {
        m_FeaturePath = featurePath;
        m_FeatureNamespace = featureNamespace;
    }

    public void CreateBase()
    {
        Directory.CreateDirectory(Path.Combine(m_FeaturePath, "External"));
        Directory.CreateDirectory(Path.Combine(m_FeaturePath, "Runtime"));
        Directory.CreateDirectory(Path.Combine(m_FeaturePath, "Content"));
    }
}