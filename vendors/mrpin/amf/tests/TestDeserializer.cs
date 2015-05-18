using System;
using System.Collections;
using System.Collections.Generic;

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

            {//simple

                //                RunTest(TestNull, "should derialize a null");

                //                RunTest(TestFalse, "should derialize a false");
                //                RunTest(TestTrue, "should derialize a true");

                //                RunTest(TestIntegers, "should deserialize integers");
                //                RunTest(TestLargeIntegers, "should deserialize large integers");
                //                RunTest(TestBigNum, "should deserialize BigNums");

                //                RunTest(TestString, "should deserialize BigNums");
            }

            {//complex

                //                RunTest(TestDate, "should deserialize dates");

                //                RunTest(TestDynamicObject, "should deserialize an unmapped object as a dynamic anonymous object");
                //                RunTest(TestMappedObject, "should deserialize a mapped object as a mapped ruby class instance");

//                RunTest(TestArrayEmpty, "should deserialize an empty array");
//                RunTest(TestArrayPrimitives, "should deserialize an array of primitives");
//                RunTest(TestArrayAssociative, "should deserialize an associative array");
                                RunTest(TestArrayMixed, "should deserialize an array of mixed objects");





            }

            AmfLogger.Log(_testPassed + " tests passed");
            AmfLogger.Log(_testFailed + " tests failed");
        }

        /*
         * Simple
         */

        private void TestNull()
        {
            AssertEqual(null, GetFirstObject("simple/amf3-null.bin"));
        }

        private void TestFalse()
        {
            AssertEqual(false, GetFirstObject("simple/amf3-false.bin"));
        }

        private void TestTrue()
        {
            UtilsDebug.assertEqual(true, GetFirstObject("simple/amf3-true.bin"));
        }

        private void TestIntegers()
        {
            AssertEqual(AmfConstants.INTEGER_MAX, GetFirstObject("simple/amf3-max.bin"));
            AssertEqual(0, GetFirstObject("simple/amf3-0.bin"));
            AssertEqual(AmfConstants.INTEGER_MIN, GetFirstObject("simple/amf3-min.bin"));
        }

        private void TestLargeIntegers()
        {
            AssertEqual(Convert.ToDouble(AmfConstants.INTEGER_MAX + 1), GetFirstObject("simple/amf3-large-max.bin"));
            AssertEqual(Convert.ToDouble(AmfConstants.INTEGER_MIN - 1), GetFirstObject("simple/amf3-large-min.bin"));
        }


        private void TestBigNum()
        {
            AssertEqual(Math.Pow(2, 1000), GetFirstObject("simple/amf3-bignum.bin"));
        }


        private void TestString()
        {
            AssertEqual("String . String", GetFirstObject("simple/amf3-string.bin"));
        }

        /*
         * Complex
         */

        private void TestDate()
        {
            AssertEqual(AmfConstants.UnixEpocTime, GetFirstObject("complex/amf3-date.bin"));
        }

        private void TestDynamicObject()
        {
            ObjectDynamic input = new ObjectDynamic();
            input["property_one"] = "foo";
            input["nil_property"] = null;
            input["another_public_property"] = "a_public_value";

            AssertEqual(input, GetFirstObject("complex/amf3-dynamic-object.bin"));
        }

        private void TestMappedObject()
        {
            AMF.Root.RegisterClassAlias("AMF.CSharpClass", "org.amf.ASClass");

            CSharpClass output = GetFirstObject("complex/amf3-typed-object.bin") as CSharpClass;

            Assert(output != null);
            AssertEqual(output.foo, "bar");
            AssertEqual(null, output.baz);
        }

        private void TestArrayEmpty()
        {
            IList output = GetFirstObject("complex/amf3-empty-array.bin") as IList;

            IList expected = new List<object>();

            Assert(IsMatch(output, expected));
        }

        private void TestArrayPrimitives()
        {
            object output = GetFirstObject("complex/amf3-primitive-array.bin");

            object[] expected = new object[]
            {
                1,
                2,
                3,
                4,
                5
            };


            Assert(IsMatch(output, expected));
        }

        private void TestArrayAssociative()
        {
            object output = GetFirstObject("complex/amf3-associative-array.bin");

            IDictionary expected = new Dictionary<object, object>();

            expected[0] = "bar1";
            expected[1] = "bar2";
            expected[2] = "bar3";
            expected["asdf"] = "fdsa";
            expected["foo"] = "bar";
            expected["42"] = "bar";

            Assert(IsMatch(output, expected));
        }

        private void TestArrayMixed()
        {
            object output = GetFirstObject("complex/amf3-mixed-array.bin");

            IList l = output as IList;

            ObjectDynamic value0 = new ObjectDynamic();
            value0["foo_one"] = "bar_one";

            ObjectDynamic value1 = new ObjectDynamic();
            value1["foo_two"] = "";

            ObjectDynamic value2 = new ObjectDynamic();
            value2["foo_three"] = 42;

            List<object> list = new List<object>();

            list.Add(value0);
            list.Add(value1);
            list.Add(value2);

            object[] expected = new object[]
            {
                value0,
                value1,
                value2,
                new ObjectDynamic(),
                list,
                new List<object>(),
                42,
                "",
                new object[]
                {
                },
                "",
                new ObjectDynamic(),
                "bar_one",
                value2
            };

            Assert(IsMatch(output, expected));
        }
    }

}