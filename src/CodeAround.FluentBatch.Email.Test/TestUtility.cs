using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace CodeAround.FluentBatch.Email.Test
{
    public static class TestUtility
    {
        public static void RunTest(object test)
        {

            if (test != null)
            {
                Console.WriteLine("Start Test " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));

                var methods = test.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                Console.WriteLine($"-- Founded {methods.Length} Test method");

                if (methods != null & methods.Length > 0)
                {
                    Action<bool> testResult = (x) =>
                    {
                        if (x)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("-- Executed Test Result OK");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("-- Executed Test Result KO");
                        }

                        Console.ForegroundColor = ConsoleColor.Gray;
                    };

                    foreach (var method in methods)
                    {
                        if (method.CustomAttributes.Any(x => x.AttributeType.Name == "FactAttribute"))
                        {
                            Console.WriteLine($"-- Executing Test {method.Name}");
                            method.Invoke(test, new object[] { testResult });
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"-- Skip Execute Test {method.Name}");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    }
                }

                Console.WriteLine("End Test " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                Console.WriteLine("Press Key to Finish");
                Console.ReadLine();
            }
        }
    }
}
