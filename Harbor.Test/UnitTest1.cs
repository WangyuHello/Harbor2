using System;
using System.Threading.Tasks;
using Xunit;

namespace Harbor.Test
{
    public class UnitTest1
    {
        [Fact]
        public void TestInit()
        {
            var actions = new Action[]
            {
                () =>
                {

                },
                () =>
                {

                },
                () =>
                {

                }
            };

            Parallel.Invoke(actions);
        }
    }
}
