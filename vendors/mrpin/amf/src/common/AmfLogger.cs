using System;
using System.Reflection;
using UnityEngine;
namespace AMF
{
    public static class AmfLogger
    {
        /*
         * Fields
         */

        private static MethodInfo _log;
        private static MethodInfo _logError;

        /*
         * Static methods
         */

        //! Satic constructor
        static AmfLogger()
        {
            bool isInitialized = false;

            isInitialized = isInitialized || initAsUnityLogger();

            isInitialized = isInitialized || initAsConsoleLogger();


        }

        private static bool initAsUnityLogger()
        {
            bool result = false;

            string assemblyQualifiedName = "UnityEngine.Debug, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

            Type unityDebug = Type.GetType(assemblyQualifiedName);

            if (unityDebug != null)
            {
                MethodInfo logMethod = unityDebug.GetMethod("Log", new []
                {
                    typeof(string)
                });
                MethodInfo logErrorMethod = unityDebug.GetMethod("LogError", new []
                {
                    typeof(string)
                });

                if (logMethod != null && logErrorMethod != null)
                {
                    _log = logMethod;
                    _logError = logErrorMethod;
                    result = true;
                }
            }

            return result;
        }

        private static bool initAsConsoleLogger()
        {
            bool result = false;

            Type console = Type.GetType("Console");

            if (console != null)
            {
                MethodInfo writeLineMethod = console.GetMethod("WriteLine");

                if (writeLineMethod != null)
                {
                    _log = writeLineMethod;
                    _logError = writeLineMethod;
                    result = true;
                }
            }

            return result;
        }

        /*
         * Log methods
         */
        public static void Log(object message)
        {
            if (_log == null || message == null)
            {
                return;
            }

            _log.Invoke(null, new object[]
            {
                message.ToString()
            });
        }

        public static void LogError(object message)
        {
            if (_logError == null || message == null)
            {
                return;
            }

            _logError.Invoke(null, new object[]
            {
                message.ToString()
            });
        }


    }
}