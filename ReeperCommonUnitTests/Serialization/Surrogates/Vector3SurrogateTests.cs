using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FinePrint;
using ReeperCommon.Serialization;
using ReeperCommon.Serialization.Surrogates;
using ReeperCommonUnitTests.Fixtures;
using UnityEngine;
using Xunit;
using Xunit.Extensions;
using XunitShould;

// ReSharper disable once CheckNamespace
namespace ReeperCommon.Serialization.Tests.Surrogates
{
    public class Vector3SurrogateTests
    {
        [Theory, AutoDomainData]
        public void Serialize_Test(Vector3Surrogate sut, Vector3 data, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var obj = (object)data;
            sut.Serialize(typeof(Vector3), ref obj, key, config, serializer);

            Assert.True(config.HasData);
            Assert.True(config.HasValue(key));
            Assert.True(config.GetValue(key) == KSPUtil.WriteVector(data));
        }


        [Theory, AutoDomainData]
        public void Deserialize_Test(Vector3Surrogate sut, Vector3 data, string key, ConfigNode config,
            IConfigNodeSerializer serializer)
        {
            var deserializedVector = default(Vector3);
            var deserializedObject = (object)deserializedVector;

            config.AddValue(key, KSPUtil.WriteVector(data));
            sut.Deserialize(typeof(Vector3), ref deserializedObject, key, config, serializer);

            deserializedVector = (Vector3)deserializedObject;
            Assert.Equal(data, deserializedVector);
        }
    }
}
