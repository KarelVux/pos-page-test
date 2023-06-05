using BLL.Base;
using DAL.Contracts.Base;
using Moq;

namespace Tests.Services
{
    public class BaseBLLTests
    {
        private readonly Mock<IBaseUOW> _baseUOWMock;
        private readonly MockedBaseBLL _mockedBaseBLL;


        public BaseBLLTests()
        {
            _baseUOWMock = new Mock<IBaseUOW>();
            _mockedBaseBLL = new MockedBaseBLL(_baseUOWMock.Object);
        }

        [Fact]
        public async Task BaseBll_Verify_That_Save_Changes_Returns_Count()
        {
            // Arrange
            _baseUOWMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(2);

            // Act
            var res = await _mockedBaseBLL.SaveChangesAsync();

            // Assert
            Assert.Equal(2, res);
        }


        private class MockedBaseBLL : BaseBLL<IBaseUOW>
        {
            public MockedBaseBLL(IBaseUOW uow) : base(uow)
            {
            }
        }
    }
}