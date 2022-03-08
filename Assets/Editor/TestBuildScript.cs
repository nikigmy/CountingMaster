using System.IO;

public class TestBuildScript
{
    static void PerformBuild ()
    {
        File.WriteAllText("C:\\Users\\nikig\\Desktop\\test\\test.txt", "Test Complete");
    }
}
