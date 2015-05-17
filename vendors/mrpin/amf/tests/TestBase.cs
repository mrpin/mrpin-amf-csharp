using System;
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
            if(pathToFixtures == null)
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


    }
}
