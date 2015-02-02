using ReeperCommonUnitTests.FileSystem.Framework.Implementations;

namespace ReeperCommonUnitTests.FileSystem.Framework.Tests
{
    public class FakeDirectory_Test
    {
        static class DirectoryFactory
        {
            public static IFakeDirectory Create(string name, IUrlFileMocker fmocker)
            {
                return new FakeDirectory(name, fmocker);
            }

            public static IFakeDirectory Create(string name)
            {
                return new FakeDirectory(name, new UrlFileMocker());
            }
        }



        //[Fact]
        //void Constructor_ThrowsExceptionOnNull_OrEmpty_OrBad_String()
        //{
        //    Assert.Throws<ArgumentNullException>(() => new FakeDirectory(null));
        //    Assert.Throws<ArgumentNullException>(() => new FakeDirectory(""));
 
        //    Assert.Throws<ArgumentNullException>(() => new FakeDirectory("/"));
        //    Assert.Throws<ArgumentNullException>(() => new FakeDirectory("\\"));
        //}



        //[Fact]
        //void Build_CallsAssignedBuilder()
        //{
        //    // arrange
        //    var builder = Substitute.For<IFakeDirectoryBuilder>();

        //    var sut = DirectoryFactory.Create("Test");


        //    // act
        //    sut.


        //    // assert
        //    builder.Build().Received();
        //}


    }
}
