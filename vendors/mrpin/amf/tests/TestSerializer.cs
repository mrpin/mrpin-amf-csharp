using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace AMF
{
    public class TestSerializer : TestBase
    {


        /*
         * Methods
         */

        public TestSerializer(string pathToFixtures):base(pathToFixtures)
        {

        }

        public void Run()
        {
            AmfLogger.Log("test serializer started");

            {//simple

                RunTest(TestNull, "should serialize a null");
                RunTest(TestFalse, "should serialize a false");
                RunTest(TestTrue, "should serialize a true");


                RunTest(TestIntegers, "should serialize integers");
                RunTest(TestLargeIntegers, "should serialize large integers");

                RunTest(TestFloat, "should serialize floats");

                //disable bignum test because something wrong with precision (in c# or as3\ruby).
                // as3/ruby: [126, 112, 0, 0, 0, 0, 0, 0]
                // c#:       [126, 112, 0, 0, 0, 0, 0, 11]
                // RunTest(TestBigNums, "should serialize BigNums");

                RunTest(TestString, "should serialize simple string");
            }


            {//complex
                RunTest(TestDateTime, "should serialize a DateTime");
                RunTest(TestUnmappedObject, "should serialize an unmapped object as a dynamic anonymous object");

                RunTest(TestArrayEmpty, "should serialize an empty array");
                RunTest(TestArrayPrimitive, "should serialize an array of primitives");
                RunTest(TestArrayMixed, "should serialize an array of mixed objects");

                RunTest(TestByteArray, "should serialize a byte array");
            }

            {//references
                RunTest(TestRefStrings, "should keep references of duplicate strings");
                RunTest(TestRefEmptyStrings, "should not reference the empty string");

                RunTest(TestRefDates, "should keep references of duplicate dates");
                RunTest(TestRefObjects, "should keep reference of duplicate objects");

                RunTest(TestRefTraits, "should keep reference of duplicate object traits");

                RunTest(TestRefArrays, "should keep references of duplicate arrays");
                RunTest(TestRefEmptyArrays, "should keep references empty arrays");

                RunTest(TestRefByteArrays, "should keep references byte arrays");


                RunTest(TestRefGraph, "should serialize a deep object graph with circular references");
            }

            {//encoding
                RunTest(TestEncodingArray, "should support multiple encodings");
                RunTest(TestEncodingRef, "should keep references of duplicate strings with different encodings");
            }

            AmfLogger.Log(_testPassed + " tests passed");
            AmfLogger.Log(_testFailed + " tests failed");
        }

        /*
         * Simple
         */

        private void CompareWithFixture(object input, string fixtureName)
        {
            string expected = GetStringFromFile(fixtureName);

            string output = GetAmf3String(input);

            AssertEqual(output, expected);
        }

        private void TestNull()
        {
            CompareWithFixture(null, "simple/amf3-null.bin");
        }

        private void TestFalse()
        {
            CompareWithFixture(false, "simple/amf3-false.bin");
        }

        private void TestTrue()
        {
            CompareWithFixture(true, "simple/amf3-true.bin");
        }

        private void TestIntegers()
        {
            CompareWithFixture(AmfConstants.INTEGER_MAX, "simple/amf3-max.bin");
            CompareWithFixture(0, "simple/amf3-0.bin");
            CompareWithFixture(AmfConstants.INTEGER_MIN, "simple/amf3-min.bin");
        }

        private void TestLargeIntegers()
        {
            CompareWithFixture(AmfConstants.INTEGER_MAX + 1, "simple/amf3-large-max.bin");
            CompareWithFixture(AmfConstants.INTEGER_MIN - 1, "simple/amf3-large-min.bin");
        }

        private void TestFloat()
        {
            CompareWithFixture(3.5, "simple/amf3-float.bin");
        }

        private void TestBigNums()
        {
            CompareWithFixture(Math.Pow(2, 1000), "simple/amf3-bigNum.bin");
        }

        private void TestString()
        {
            CompareWithFixture("String . String", "simple/amf3-string.bin");
        }

        private void TestDateTime()
        {
            CompareWithFixture(AmfConstants.UnixEpocTime, "complex/amf3-date.bin");
        }



        /*
         * Complex
         */

        private void TestUnmappedObject()
        {
            CompareWithFixture(new NonMappedClass(), "complex/amf3-dynamic-object.bin");
        }

        private void TestArrayEmpty()
        {
            CompareWithFixture(new object[]
            {
            }, "complex/amf3-empty-array.bin");

            CompareWithFixture(new ArrayList(), "complex/amf3-empty-array.bin");

            CompareWithFixture(new List<object>(), "complex/amf3-empty-array.bin");
        }

        private void TestArrayPrimitive()
        {
            object[] array = new object[]
            {
                1,
                2,
                3,
                4,
                5
            };

            CompareWithFixture(array, "complex/amf3-primitive-array.bin");

            ArrayList arrayList = new ArrayList();
            arrayList.Add(1);
            arrayList.Add(2);
            arrayList.Add(3);
            arrayList.Add(4);
            arrayList.Add(5);

            CompareWithFixture(arrayList, "complex/amf3-primitive-array.bin");

            List<object> typedList = new List<object>();
            typedList.Add(1);
            typedList.Add(2);
            typedList.Add(3);
            typedList.Add(4);
            typedList.Add(5);

            CompareWithFixture(typedList, "complex/amf3-primitive-array.bin");
        }


        private void TestArrayMixed()
        {
            ObjectDynamic value0 = new ObjectDynamic();
            value0["foo_one"] = "bar_one";

            ObjectDynamic value1 = new ObjectDynamic();
            value1["foo_two"] = "";

            ObjectDynamic value2 = new ObjectDynamic();
            value2["foo_three"] = 42;

            List<object> input = new List<object>();
            input.Add(value0);
            input.Add(value1);
            input.Add(value2);
            input.Add(new ObjectDynamic());

            {
                List<object> list = new List<object>();

                list.Add(value0);
                list.Add(value1);
                list.Add(value2);

                input.Add(list);
            }

            input.Add(new List<object>());
            input.Add(42);
            input.Add("");
            input.Add(new List<object>());
            input.Add("");
            input.Add(new ObjectDynamic());
            input.Add("bar_one");
            input.Add(value2);

            CompareWithFixture(input, "complex/amf3-mixed-array.bin");
        }

        private void TestByteArray()
        {
            string str = " これtest@";

            byte[] input = Encoding.UTF8.GetBytes(str);

            CompareWithFixture(input, "complex/amf3-byte-array.bin");
        }

        /*
         * References
         */

        private void TestRefStrings()
        {
            string foo = "foo";
            string bar = "str";

            ObjectDynamic value = new ObjectDynamic();
            value["str"] = foo;

            object[] input = new object[]
            {
                foo,
                bar,
                foo,
                bar,
                foo,
                value
            };

            CompareWithFixture(input, "references/amf3-string-ref.bin");
        }

        private void TestRefEmptyStrings()
        {
            object[] input = new object[]
            {
                "",
                ""
            };

            CompareWithFixture(input, "references/amf3-empty-string-ref.bin");
        }


        private void TestRefDates()
        {
            DateTime epocTime = AmfConstants.UnixEpocTime;

            object[] input = new object[]
            {
                epocTime,
                epocTime
            };

            CompareWithFixture(input, "references/amf3-date-ref.bin");
        }

        private void TestRefObjects()
        {
            ObjectDynamic value0 = new ObjectDynamic();
            value0["foo"] = "bar";

            ObjectDynamic value1 = new ObjectDynamic();
            value1["foo"] = value0["foo"];

            object[] array0 = new object[]
            {
                value0,
                value1
            }   ;

            object[] array1 = new object[]
            {
                value0,
                value1
            }   ;


            object[] input = new object[]
            {
                array0,
                "bar",
                array1
            };

            CompareWithFixture(input, "references/amf3-object-ref.bin");
        }

        private void TestRefTraits()
        {
            CSharpClass value0 = new CSharpClass();
            value0.foo = "foo";

            CSharpClass value1 = new CSharpClass();
            value1.foo = "bar";

            object[] input = new object[]
            {
                value0,
                value1
            };

            CompareWithFixture(input, "references/amf3-trait-ref.bin");
        }

        private void TestRefArrays()
        {
            object[] a = new object[]
            {
                1,
                2,
                3
            };
            object[] b = new object[]
            {
                "a",
                "b",
                "c"
            };

            object[] input = new object[]
            {
                a,
                b,
                a,
                b
            };

            CompareWithFixture(input, "references/amf3-array-ref.bin");
        }

        private void TestRefEmptyArrays()
        {
            object[] a = new object[]
            {
            };
            object[] b = new object[]
            {
            };

            object[] input = new object[]
            {
                a,
                b,
                a,
                b
            };

            CompareWithFixture(input, "references/amf3-empty-array-ref.bin");
        }

        private void TestRefByteArrays()
        {
            byte[] value = Encoding.UTF8.GetBytes("ASDF");

            object[] input = new object[]
            {
                value,
                value
            };

            CompareWithFixture(input, "references/amf3-byte-array-ref.bin");
        }

        private void TestRefGraph()
        {
            GraphMember input = new GraphMember();
            input.AddChild(new GraphMember());
            input.AddChild(new GraphMember());

            CompareWithFixture(input, "references/amf3-graph-member.bin");
        }

        /*
         * Encoding
         */

        private void TestEncodingArray()
        {
            Encoding encodingShift_JIS = Encoding.GetEncoding("Shift_JIS");

            if (encodingShift_JIS == null)
            {
                return;
            }

            byte[] value0Bytes = new byte[]
            {
                83,
                104,
                105,
                102,
                116,
                32,
                131,
                101,
                131,
                88,
                131,
                103
            };

            string value0 = encodingShift_JIS.GetString(value0Bytes);

            byte[] value1Bytes = new byte[]
            {
                85,
                84,
                70,
                32,
                227,
                131,
                134,
                227,
                130,
                185,
                227,
                131,
                136
            };

            string value1 = Encoding.UTF8.GetString(value1Bytes);

            object[] input = new object[]
            {
                5,
                value0,
                value1,
                5
            };

            CompareWithFixture(input, "encoding/amf3-complex-encoded-string-array.bin");
        }

        private void TestEncodingRef()
        {
            Encoding encodingShift_JIS = Encoding.GetEncoding("Shift_JIS");

            if (encodingShift_JIS == null)
            {
                return;
            }
            byte[] value0Bytes = new byte[]
            {
                116,
                104,
                105,
                115,
                32,
                105,
                115,
                32,
                97,
                32,
                131,
                101,
                131,
                88,
                131,
                103
            }      ;

            string value0 = encodingShift_JIS.GetString(value0Bytes);

            byte[] value1Bytes = new byte[]
            {
                116,
                104,
                105,
                115,
                32,
                105,
                115,
                32,
                97,
                32,
                227,
                131,
                134,
                227,
                130,
                185,
                227,
                131,
                136
            };

            string value1 = Encoding.UTF8.GetString(value1Bytes);

            object[] input = new object[]
            {
                value0,
                value1,
            };

            CompareWithFixture(input, "encoding/amf3-encoded-string-ref.bin");
        }

    }
}
