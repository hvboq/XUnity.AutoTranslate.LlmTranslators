using XUnity.AutoTranslator.LlmTranslators.Config;

namespace XUnity.AutoTranslator.LlmTranslators.Tests;

public class ConfigTests
{
    const string workingDirectory = "../../../";
    const string sampleDirectory = $"{workingDirectory}/../XUnity.AutoTranslator.LlmTranslators/SampleConfig/";

    [Fact]
    public void TestDefaultConfig()
    {
        var config = Configuration.GetConfiguration($"{sampleDirectory}/OpenAi.yaml");

        Assert.True(config.SystemPrompt!.Split("\n").Length > 1);
    }

    [Fact]
    public void TestLmStudioConfig()
    {
        var config = Configuration.GetConfiguration($"{sampleDirectory}/LmStudio.yaml");

        Assert.Equal("http://localhost:1234/v1/chat/completions", config.Url);
        Assert.Equal("model-identifier", config.Model);
        Assert.False(config.ApiKeyRequired);
        Assert.True(config.SystemPrompt!.Split("\n").Length > 1);
    }

    [Fact]
    public void TestGlossaryPromptOverride()
    {
        var config = new LlmConfig { SystemPrompt = "System prompt", GlossaryPrompt = "Glossary prompt" };
        var file = Path.GetTempFileName();

        try
        {
            File.WriteAllText(file, "Override glossary prompt");

            Configuration.LoadGlossaryPrompt(config, file);

            Assert.Equal("System prompt", config.SystemPrompt);
            Assert.Equal("Override glossary prompt", config.GlossaryPrompt);
        }
        finally
        {
            File.Delete(file);
        }
    }
}
