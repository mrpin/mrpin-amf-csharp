using System;
using System.Collections;
using System.IO;
using System.Text;
namespace AMF
{
    public class TestBase
    {
        /*
         * Fields
         */
        protected int _testPassed;
        protected int _testFailed;

        private string _pathToFixtures;

        /*
         * Methods
         */

        public TestBase(string pathToFixtures = null)
        {
            if (pathToFixtures == null)
            {
                _pathToFixtures = Path.Combine(Directory.GetCurrentDirectory(), "fixtures");
            }
            else
            {
                _pathToFixtures = pathToFixtures;
            }
        }

        /*
         * Helpers
         */

        protected string GetStringFromFile(string fileName)
        {
            string result = null;

            string path = Path.Combine(_pathToFixtures, fileName);

            byte[] data = File.ReadAllBytes(path);

            result = Encoding.UTF8.GetString(data);

            return result;
        }

        protected object GetFirstObject(string fileName)
        {
            object result = null;

            string path = Path.Combine(_pathToFixtures, fileName);

            byte[] data = File.ReadAllBytes(path);

            AmfResponse response = AMF.Root.Deserialize(data);

            result = response.Objects[0];

            return result;
        }

        protected string GetAmf3String(object value)
        {
            string result = null;

            byte[] data = AMF.Root.Serialize(value);

            result = Encoding.UTF8.GetString(data);

            return result;
        }

        protected void RunTest(UtilsDelegate.CallbackWithoutParams test, string message)
        {
            AMF.Root.ClearClassAliases();

            try
            {
                test();

                _testPassed++;
            }
            catch (Exception e)
            {
                AmfLogger.LogError(message + " failed");
                AmfLogger.LogError(e.Message);
                AmfLogger.LogError(e.StackTrace);

                _testFailed++;
            }
        }

        /*
         * Assertion
         */

        protected static void Assert(bool condition)
        {
            if (!condition)
            {
                throw new Exception();
            }
        }

        protected static T AssertEqual<T>(Object target, T value)
        {
            Assert(Object.Equals(target, value));

            return value;
        }

        protected static T AssertNotEqual<T>(Object target, T value)
        {
            Assert(!Object.Equals(target, value));

            return value;
        }

        protected static bool IsMatch(object v0, object v1)
        {
            bool result = false;

            if (v0 is IDictionary && v1 is IDictionary)
            {
                result = IsDictionariesMatch(v0 as IDictionary, v1 as IDictionary);
            }
            else if (v0 is IList && v1 is IList)
            {
                result = IsArraysMatch(v0 as IList, v1 as IList);
            }
            else
            {
                result = Object.Equals(v0, v1);

                //todo:remove
                if (!result)
                {
                    AmfLogger.Log(v0);
                    AmfLogger.Log(v1);

                    AmfLogger.Log(v0.GetType().FullName);
                    AmfLogger.Log(v0.ToString());

                    AmfLogger.Log(v1.GetType().FullName);
                    AmfLogger.Log(v1.ToString());
                }
            }

            return result;
        }

        private static bool IsArraysMatch(IList v0, IList v1)
        {
            bool result = true;

            if ((v0 == null && v1 != null) || (v0 != null && v1 == null))
            {
                result = false;
            }
            else if (v0.Count != v1.Count)
            {
                result = false;
            }
            else
            {
                for (int j = 0; j < v0.Count; j++)
                {
                    object item0 = v0[j];
                    object item1 = v1[j];

                    if (!IsMatch(item0, item1))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private static bool IsDictionariesMatch(IDictionary v0, IDictionary v1)
        {
            bool result = true;

            if ((v0 == null && v1 != null) || (v0 != null && v1 == null))
            {
                result = false;
            }
            else if (v0.Keys.Count != v0.Keys.Count)
            {
                result = false;
            }
            else
            {
                foreach (object key in v0.Keys)
                {
                    object value0 = v0[key] ;
                    object value1 = v1[key] ;

                    if (!IsMatch(value0, value1))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

    }
}
