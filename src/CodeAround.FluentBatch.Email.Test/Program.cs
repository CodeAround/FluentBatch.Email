using CodeAround.FluentBatch.Email.Test.TaskTest;
using System;
using System.Reflection;
using System.Linq;

namespace CodeAround.FluentBatch.Email.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            EmailDestinationTaskTest test = new EmailDestinationTaskTest();

            TestUtility.RunTest(test);
        }

        
    }
}
