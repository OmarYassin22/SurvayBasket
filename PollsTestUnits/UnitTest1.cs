using FluentAssertions;

namespace PollsTestUnits;

public class UnitTest1
{

    [Fact]
    public void CalcTotal_3_Sum_2_Should_be_5_Without_FluentAssertion()
    {
        // Arrange 
        int x = 2, y = 3;
        //Act
        int z = x + y;
        // Assert
        Assert.Equal(5, z);

    }
    [Fact]
    public void CalcTotal_3_Sum_2_Should_be_5_With_FluentAssertion()
    {
        // Arrange
        int x = 3, y = 2;
        int z = 0;
        //Act
        z = x + y;
        // Assert
        z.Should().Be(5, "3 + 2 = 5");
    }
    [Fact]
    public void String_Should_Start_With_O()
    {
        //arrange
        var world = "Omar";
        //act
        var result = world.StartsWith("O");
        //assert
        result.Should().BeTrue();
        world.Should().StartWith("O");
    }
    [Fact]
    public void String_Should_Start_With_O_And_End_With_R_And_Contain_M()
    {

        var world = "OMAR";
        world.Should().StartWith("O").And.EndWith("R").And.Contain("M");
    }

    [Fact]
    public void String_Shouldnot_Be_Null_Or_Empty_Space()
    {
        var world = "OMAR";
        world.Should().NotBeNullOrEmpty();
    }
}
