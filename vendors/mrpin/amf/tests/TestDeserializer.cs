using AMF;
using System;
using Debug = UnityEngine.Debug;

public class TestDeserializer : TestBase
{
    /*
     * Fields
     */

    /*
     * Methods
     */

    public void run()
    {
        Debug.Log("test serializer started");

        //simple
        runTest(testNull, "should derialize a null");

        runTest(testFalse, "should derialize a false");
        runTest(testTrue, "should derialize a true");

        runTest(testIntegers, "should deserialize integers");
        runTest(testLargeIntegers, "should deserialize large integers");
        runTest(testBigNum, "should deserialize BigNums");

        runTest(testString, "should deserialize BigNums");

        //complex
        runTest(testDate, "should deserialize dates");

        runTest(testDynamicObject, "should deserialize an unmapped object as a dynamic anonymous object");

        Debug.Log(_testPassed + " tests passed");
        Debug.Log(_testFailed + " tests failed");
    }

    private void testNull()
    {
        UtilsDebug.assertEqual(null, getFirstObject("simple/amf3-null.bin"));
    }

    private void testFalse()
    {
        UtilsDebug.assertEqual(false, getFirstObject("simple/amf3-false.bin"));
    }

    private void testTrue()
    {
        UtilsDebug.assertEqual(true, getFirstObject("simple/amf3-true.bin"));
    }

    private void testIntegers()
    {
        UtilsDebug.assertEqual(AmfConstants.INTEGER_MAX, getFirstObject("simple/amf3-max.bin"));
        UtilsDebug.assertEqual(0, getFirstObject("simple/amf3-0.bin"));
        UtilsDebug.assertEqual(AmfConstants.INTEGER_MIN, getFirstObject("simple/amf3-min.bin"));
    }

    private void testLargeIntegers()
    {
        UtilsDebug.assertEqual((double)AmfConstants.INTEGER_MAX + 1, getFirstObject("simple/amf3-large-max.bin"));
        UtilsDebug.assertEqual((double)AmfConstants.INTEGER_MIN - 1, getFirstObject("simple/amf3-large-min.bin"));
    }


    private void testBigNum()
    {
        UtilsDebug.assertEqual(Math.Pow(2, 1000), getFirstObject("simple/amf3-bignum.bin"));
    }


    private void testString()
    {
        UtilsDebug.assertEqual("String . String", getFirstObject("simple/amf3-string.bin"));
    }

    private void testDate()
    {
        UtilsDebug.assertEqual(new DateTime(0), getFirstObject("complex/amf3-date.bin"));
    }

    private void testDynamicObject()
    {
        //todo:finish
//        UtilsDebug.assertEqual(new DateTime(0), getFirstObject("complex/amf3-dynamic-object.bin"));
    }



}
