using System;

namespace AMF
{
    public class TestDeserializer : TestBase
    {

        /*
         * Methods
         */

        public TestDeserializer(string pathToFixtures):base(pathToFixtures)
        {

        }

        public void Run()
        {
            AmfLogger.Log("test serializer started");

            //simple
            RunTest(TestNull, "should derialize a null");

            RunTest(TestFalse, "should derialize a false");
            RunTest(testTrue, "should derialize a true");

            RunTest(testIntegers, "should deserialize integers");
            RunTest(testLargeIntegers, "should deserialize large integers");
            RunTest(testBigNum, "should deserialize BigNums");

            RunTest(testString, "should deserialize BigNums");

            //complex
            RunTest(testDate, "should deserialize dates");

            RunTest(testDynamicObject, "should deserialize an unmapped object as a dynamic anonymous object");

            AmfLogger.Log(_testPassed + " tests passed");
            AmfLogger.Log(_testFailed + " tests failed");
        }

        private void TestNull()
        {
            AssertEqual(null, GetFirstObject("simple/amf3-null.bin"));
        }

        private void TestFalse()
        {
            AssertEqual(false, GetFirstObject("simple/amf3-false.bin"));
        }

        private void testTrue()
        {
            UtilsDebug.assertEqual(true, GetFirstObject("simple/amf3-true.bin"));
        }

        private void testIntegers()
        {
            AssertEqual(AmfConstants.INTEGER_MAX, GetFirstObject("simple/amf3-max.bin"));
            AssertEqual(0, GetFirstObject("simple/amf3-0.bin"));
            AssertEqual(AmfConstants.INTEGER_MIN, GetFirstObject("simple/amf3-min.bin"));
        }

        private void testLargeIntegers()
        {
            AssertEqual(Convert.ToDouble(AmfConstants.INTEGER_MAX + 1), GetFirstObject("simple/amf3-large-max.bin"));
            AssertEqual(Convert.ToDouble(AmfConstants.INTEGER_MIN - 1), GetFirstObject("simple/amf3-large-min.bin"));
        }


        private void testBigNum()
        {
            AssertEqual(Math.Pow(2, 1000), GetFirstObject("simple/amf3-bignum.bin"));
        }


        private void testString()
        {
            UtilsDebug.assertEqual("String . String", GetFirstObject("simple/amf3-string.bin"));
        }

        private void testDate()
        {
            UtilsDebug.assertEqual(new DateTime(0), GetFirstObject("complex/amf3-date.bin"));
        }

        private void testDynamicObject()
        {
            //todo:finish
            //        UtilsDebug.assertEqual(new DateTime(0), getFirstObject("complex/amf3-dynamic-object.bin"));
        }



    }

}