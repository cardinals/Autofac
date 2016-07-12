﻿using System;
using System.Linq;
using System.Reflection;
using Autofac.Core.Activators.Reflection;
using Autofac.Test.Util;
using Xunit;

namespace Autofac.Test.Core.Activators.Reflection
{
    public class DefaultValueParameterTests
    {
        public class HasDefaultValues
        {
            public HasDefaultValues(string s, string t = "Hello")
            {
            }
        }

        private static ParameterInfo GetTestParameter(string name)
        {
            return typeof(HasDefaultValues).GetConstructors().Single()
                .GetParameters().Where(pi => pi.Name == name).Single();
        }

        [Fact]
        public void DoesNotProvideValueWhenNoDefaultAvailable()
        {
            var dvp = new DefaultValueParameter();
            Func<IComponentContext, object> vp;
            var dp = GetTestParameter("s").DefaultValue;
            Assert.False(dvp.CanSupplyValue(GetTestParameter("s"), new ContainerBuilder().Build(), out vp));
        }

        [Fact]
        public void ProvidesValueWhenDefaultInitialiserPresent()
        {
            var dvp = new DefaultValueParameter();
            var u = GetTestParameter("t");
            Func<IComponentContext, object> vp;
            var dp = u.DefaultValue;
            var c = new ContainerBuilder().Build();
            Assert.True(dvp.CanSupplyValue(u, c, out vp));
            Assert.Equal("Hello", vp(c));
        }
    }
}
