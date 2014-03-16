using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject
{
    class TestResult
    {
        public TestResult(long product)
        {
            Product = product;
        }

        private readonly long Product;

        public override string ToString()
        {
            return Product.ToString();
        }
    }
}
