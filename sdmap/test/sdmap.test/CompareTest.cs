﻿using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.IntegratedTest
{
    public class CompareTest
    {
        [Fact]
        public void IsEqualOk()
        {
            var code = "sql v1{#isEqual<A, true, 'Yes'>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = true });
            Assert.Equal("Yes", result);
        }

        [Fact]
        public void IsEqualIntOk()
        {
            var code = "sql v1{#isEqual<A, 3, 'Yes'>}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new { A = 3 });
            Assert.Equal("Yes", result);
        }
    }
}
