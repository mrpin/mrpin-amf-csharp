using System;
using System.IO;
using System.Text;
using Debug = UnityEngine.Debug;

public class TestSerializer : TestBase
{

    /*
     * Fields
     */


    /*
     * Helpers
     */

    /*
     * Methods
     */

    public void run()
    {
        Debug.Log("test serializer started");

        runTest(testNull, "should serialize a null");
        runTest(testFalse, "should serialize a false");
        runTest(testTrue, "should serialize a true");


        Debug.Log(_testPassed + " tests passed");
        Debug.Log(_testFailed + " tests failed");
    }



    private void testNull()
    {
        string amfString = getAmfString(null);

        string amfStringFromFile = getDataFromFile("simple/amf3-null.bin");

        UtilsDebug.assertEqual(amfString, amfStringFromFile);
    }

    private void testFalse()
    {
        string amfString = getAmfString(false);

        string amfStringFromFile = getDataFromFile("simple/amf3-false.bin");

        UtilsDebug.assertEqual(amfString, amfStringFromFile);
    }

    private void testTrue()
    {
        string amfString = getAmfString(true);

        string amfStringFromFile = getDataFromFile("simple/amf3-true.bin");

        UtilsDebug.assertEqual(amfString, amfStringFromFile);
    }

}
