namespace EFCore.Extensions.Test.Tests
{
    public class LanguageTest
    {
        [Fact]
        public void NullAbleIsValue()
        {
            bool? f = false;

            var ret = f is false;

            Assert.True(ret);
        }
    }
}