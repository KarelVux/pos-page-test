using Xunit.Abstractions;

namespace Tests.Helpers;

public class LogTextGenerator
{
    private readonly ITestOutputHelper _outputHelper;

    public LogTextGenerator(ITestOutputHelper outputHelper)
    {
        this._outputHelper = outputHelper;
    }

    public void GenerateHttpRequestResponseLog(HttpResponseMessage response)
    {
        _outputHelper.WriteLine("REQUEST:{1}\n{0}\n", response.RequestMessage!.ToString(),
            response.RequestMessage.Content?.ReadAsStringAsync().Result);
        _outputHelper.WriteLine("RESPONSE:\n{0}\n{1}\n", response, response?.Content.ReadAsStringAsync().Result);
    }


    public void GenerateTestStep(String step)
    {
        _outputHelper.WriteLine("\n" + step);
    }
}