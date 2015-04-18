﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using ReeperCommon.Serialization;
using ReeperCommonUnitTests.TestData;

namespace ReeperCommonUnitTests.Fixtures
{
    namespace AssemblyReloaderTests.FixtureCustomizations
    {
        public class AutoDomainDataAttribute : AutoDataAttribute
        {
            public AutoDomainDataAttribute()
                : base(new Fixture().Customize(new DomainCustomization()))
            {
                //var filename = Assembly.GetExecutingAssembly().Location;

                //if (!File.Exists(filename))
                //    throw new FileNotFoundException(filename);

                //var assemblyDefinition = AssemblyDefinition.ReadAssembly(filename);

                //// AssemblyDefinition
                //Fixture.Register(() => assemblyDefinition);

                //// TypeDefintiion
                //var typesWithAtLeastOneMethod =
                //    assemblyDefinition.Modules.SelectMany(md => md.Types).Where(td => td.Methods.Count > 0);

                //Fixture.Register(() => typesWithAtLeastOneMethod.First());

                //// Assembly => ExecutingAssembly

                //Fixture.Register(() => Assembly.GetExecutingAssembly());

                //// IAddonAttributesFromAssembly
                //Fixture.Register(() => new AddonAttributesFromTypeQuery());


                //// MethodDefinition => TestPartModule.OnSave
                //Fixture.Register(
                //    () =>
                //        assemblyDefinition.MainModule.Types.Single(td => td.FullName == "AssemblyReloaderTests.TestData.PartModules.TestPartModule").Methods.Single(
                //            md => md.Name == "OnSave"));

                //Fixture.Register(() => global::ConfigNode);

                Fixture.Register(() => new ConfigNode());
                Fixture.Register(() => new SimplePersistentObject());
                Fixture.Register(() => new SerializableFieldQuery());
            }
        }
    }
}
