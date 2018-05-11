using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Granular.Extensions;
using static Retyped.dom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Granular.UnitTesting
{
    public static class UnitTestRunner
    {
        public static void Run(Assembly testAssembly)
        {
            WriteLog("Test run starting");

            int testsCount;
            int testsPassed;

            if (!window.location.hash.IsNullOrEmpty())
            {
                string testName = window.location.hash.TrimStart('#');
                RunTest(testAssembly, testName, false, out testsCount, out testsPassed);
            }
            else
            {
                RunTests(testAssembly, true, out testsCount, out testsPassed);
            }

            if (testsCount > 0)
            {
                WriteLog("");

                string summary = String.Format("Test run completed - {0}/{1} passed", testsPassed, testsCount);
                if (testsPassed == testsCount)
                {
                    WriteInfo(summary);
                }
                else
                {
                    WriteWarning(summary);
                    WriteLog("");
                    WriteLog("Append a #TestName to the url to run and debug a specific test");
                }
            }
        }

        private static void RunTests(Assembly testAssembly, bool catchExceptions, out int testsCount, out int testsPassed)
        {
            testsCount = 0;
            testsPassed = 0;

            foreach (Type testClass in GetTestClasses(testAssembly))
            {
                int classTestsCount;
                int classTestsPassed;

                RunTestClass(testClass, true, out classTestsCount, out classTestsPassed);

                testsCount += classTestsCount;
                testsPassed += classTestsPassed;
            }
        }

        private static void RunTest(Assembly testAssembly, string testName, bool catchExceptions, out int testsCount, out int testsPassed)
        {
            Type testClass;
            MethodInfo testMethod;
            if (TryFindTestClass(testAssembly, testName, out testClass))
            {
                RunTestClass(testClass, catchExceptions, out testsCount, out testsPassed);
                WriteInfo(String.Format("Test {0} completed", testName));
            }
            else if (TryFindTestMethod(testAssembly, testName, out testMethod))
            {
                testsCount = 1;
                testsPassed = RunTestMethod(testMethod, catchExceptions) ? 1 : 0;
                WriteInfo(String.Format("Test {0} completed", testName));
            }
            else
            {
                testsCount = 0;
                testsPassed = 0;
                WriteError(String.Format("Test {0} was not found in \"{1}\"", testName, testAssembly.GetName().Name));
            }
        }

        private static void RunTestClass(Type testClass, bool catchExceptions, out int testsCount, out int testsPassed)
        {
            WriteLog(String.Format("Running {0}", testClass.Name));

            testsCount = 0;
            testsPassed = 0;

            foreach (MethodInfo testMethod in GetTestMethods(testClass))
            {
                if (RunTestMethod(testMethod, catchExceptions))
                {
                    testsPassed++;
                }

                testsCount++;
            }
        }

        private static bool RunTestMethod(MethodInfo testMethod, bool catchExceptions)
        {
            object testInstance = Activator.CreateInstance(testMethod.DeclaringType);

            if (catchExceptions)
            {
                try
                {
                    testMethod.Invoke(testInstance);
                    WriteInfo(String.Format("    Test {0} passed", testMethod.Name));
                    return true;
                }
                catch (Exception e)
                {
                    WriteError(String.Format("    Test {0} FAILED - reason: {1}", testMethod.Name, e.Message));
                    return false;
                }
            }
            else
            {
                testMethod.Invoke(testInstance);
                return true;
            }
        }

        private static bool TryFindTestClass(Assembly testAssembly, string testClassName, out Type testClass)
        {
            testClass = GetTestClasses(testAssembly).FirstOrDefault(type => type.Name == testClassName);
            return testClass != null;
        }

        private static bool TryFindTestMethod(Assembly testAssembly, string testMethodName, out MethodInfo testMethod)
        {
            testMethod = GetTestClasses(testAssembly).SelectMany(testClass => testClass.GetMethods(BindingFlags.Instance | BindingFlags.Public)).
                FirstOrDefault(methodInfo => methodInfo.Name == testMethodName && methodInfo.GetCustomAttributes(true).OfType<TestMethodAttribute>().Any());

            return testMethod != null;
        }

        private static IEnumerable<Type> GetTestClasses(Assembly testAssembly)
        {
            return testAssembly.GetTypes().Where(type => type.GetCustomAttributes(true).OfType<TestClassAttribute>().Any());
        }

        private static IEnumerable<MethodInfo> GetTestMethods(Type testClass)
        {
            return testClass.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(methodInfo => methodInfo.GetCustomAttributes(true).OfType<TestMethodAttribute>().Any());
        }

        [Bridge.Template("console.error({message})")]
        private static extern void WriteError(string message);

        [Bridge.Template("console.info({message})")]
        private static extern void WriteInfo(string message);

        [Bridge.Template("console.log({message})")]
        private static extern void WriteLog(string message);

        [Bridge.Template("console.warn({message})")]
        private static extern void WriteWarning(string message);
    }
}
