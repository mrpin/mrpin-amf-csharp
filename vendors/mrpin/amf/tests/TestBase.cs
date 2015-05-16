using System;
using System.IO;
using System.Text;
public class TestBase
{
    /*
     * Fields
     */
    protected int _testPassed;
    protected int _testFailed;

    /*
     * Helpers
     */

    private string getFullPathFor(string fileName)
    {
        string result = Directory.GetCurrentDirectory();

        result = Path.Combine(result, "Assets/fixtures/");
        result = Path.Combine(result, fileName);

        return result;
    }

    protected string getDataFromFile(string fileName)
    {
        string result = null;

        string path = getFullPathFor(fileName);

        byte[] data = File.ReadAllBytes(path);

        result = Encoding.UTF8.GetString(data);

        return result;
    }

    protected object getFirstObject(string fileName)
    {
        object result = null;

        string path = getFullPathFor(fileName);

        byte[] data = File.ReadAllBytes(path);

        AmfResponse response = AMF.Root.deserialize(data);

        result = response.objects[0];

        return result;
    }

    protected string getAmfString(object value)
    {
        string result = null;

        byte[] data = AMF.Root.serialize(value);

        result = Encoding.UTF8.GetString(data);

        return result;
    }

    protected void runTest(UtilsDelegate.CallbackWithoutParams test, string message)
    {
        try
        {
            test();

            _testPassed++;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(message + " failed");
            UnityEngine.Debug.LogError(e.Message);
            UnityEngine.Debug.LogError(e.StackTrace);

            _testFailed++;
        }
    }
}
