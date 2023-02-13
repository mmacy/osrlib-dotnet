using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace osrlib.Tests
{
    // Adapted from https://learn.microsoft.com/dotnet/core/testing/order-unit-tests?pivots=xunit

    /// <summary>
    /// The TestPriorityAttribute is used to specify the priority of a test case.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestPriorityAttribute : Attribute
    {
        public int Priority { get; private set; }

        public TestPriorityAttribute(int priority) => Priority = priority;
    }

    /// <summary>
    /// The PriorityOrderer is used to order test cases based on their priority.
    /// </summary>
    public class PriorityOrderer : ITestCaseOrderer
    {
        /// <summary>
        /// Orders the test cases based on their priority.
        /// </summary>
        /// <typeparam name="TTestCase">The type of the test case.</typeparam>
        /// <param name="testCases">The test cases to be ordered.</param>
        /// <returns>The ordered test cases.</returns>
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
            IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            string assemblyName = typeof(TestPriorityAttribute).AssemblyQualifiedName!;
            var sortedMethods = new SortedDictionary<int, List<TTestCase>>();
            foreach (TTestCase testCase in testCases)
            {
                int priority = testCase.TestMethod.Method
                    .GetCustomAttributes(assemblyName)
                    .FirstOrDefault()
                    ?.GetNamedArgument<int>(nameof(TestPriorityAttribute.Priority)) ?? 0;

                GetOrCreate(sortedMethods, priority).Add(testCase);
            }

            foreach (TTestCase testCase in
                sortedMethods.Keys.SelectMany(
                    priority => sortedMethods[priority].OrderBy(
                        testCase => testCase.TestMethod.Method.Name)))
            {
                yield return testCase;
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key, or creates a new value if the key is not found.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns>The value associated with the specified key.</returns>
        private static TValue GetOrCreate<TKey, TValue>(
            IDictionary<TKey, TValue> dictionary, TKey key)
            where TKey : struct
            where TValue : new() =>
            dictionary.TryGetValue(key, out TValue? result)
                ? result
                : (dictionary[key] = new TValue());
    }
}