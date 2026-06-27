namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var a = 2;
            var b = 3;

            var resultado = a + b;

            Assert.Equal(5, resultado);
        }
    }
}
