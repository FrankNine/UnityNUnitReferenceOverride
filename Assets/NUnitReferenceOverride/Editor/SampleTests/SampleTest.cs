using NUnit.Framework;

public class SampleTest {
    [Test]
    public static void SampleTest_OneAddTwo_EqualsThree()
    {
        const int a = 1;
        const int b = 2;
        
        Assert.AreEqual(3, a +b);
    }
}
