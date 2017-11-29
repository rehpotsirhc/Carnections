using Amazon.Lambda.TestUtilities;
using ScrapeCentralDispatch;
using System;

namespace ScrapeCDRunnerAndDbMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            LambdaFunc function = new LambdaFunc();
            function.ScrapeCDAndSave(true, new TestLambdaContext()).Wait();
        }
    }
}
