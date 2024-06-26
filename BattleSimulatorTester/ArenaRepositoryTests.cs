using BattleSimulator.Data;
using BattleSimulator.Interfaces;
using BattleSimulator.Models;
using BattleSimulator.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BattleSimulatorTester
{
    public class ArenaRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDBContext> _options;

        public ArenaRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "BattleSimulatorTestDb")
                .Options;
        }

        [Fact]
        public async Task CreateAsync_AddsArenaToDatabase()
        {
            // Arrange
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new ArenaRepository(context);
                var arena = new Arena();
                int numbOfArenas = context.Arena.Count();

                // Act
                await repository.CreateAsync();
                var savedArena = await context.Arena.LastAsync();

                // Assert
                Assert.NotNull(savedArena);
                Assert.Equal(numbOfArenas, context.Arena.Count());
            }
        }

        [Fact]
        public async Task DoBattleAsync_ReturnsUpdatedArena()
        {

        }
    }
}