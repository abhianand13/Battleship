using BattleshipGame.Models;
using BattleshipGame.Models.Enums;
using BattleshipGame.Requests;
using BattleshipGame.Services;
using BattleshipGame.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace BattleshipGame.Tests
{
    [TestClass]
    public class BattleshipServiceTest
    {
        Mock<ILogService> logServiceMock;
        Mock<IGameService> gameServiceMock;

        [TestMethod]
        public void TestInitializeNewGame()
        {
            InitializeMocks();
            gameServiceMock.Setup(x => x.GetCurrentGame()).Returns<Game>(null);

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            var response = service.InitializeNewGame(10, false);
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public void TestInitializeNewGameWithGameInProgress()
        {
            InitializeMocks();

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            var response = service.InitializeNewGame(5, false);
            Assert.IsFalse(response.IsSuccess);
        }

        [TestMethod]
        public void TestInitializeNewGameForceCreateWithGameInProgress()
        {
            InitializeMocks();

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            var response = service.InitializeNewGame(5, true);
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public void TestAddBattleship()
        {
            InitializeMocks();

            var request = new AddBattleshipRequest
            {
                Player = Player.Player1,
                PositionX = 5,
                PositionY = 3,
                ShipSize = 4,
                IsHorizontal = true
            };

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            var response = service.AddNewBattleship(request);
            Assert.IsTrue(response.IsSuccess);
        }

        [TestMethod]
        public void TestAddBattleshipGameNotInProgress()
        {
            InitializeMocks();
            gameServiceMock.Setup(x => x.GetCurrentGame()).Returns<Game>(null);

            var request = new AddBattleshipRequest
            {
                Player = Player.Player1,
                PositionX = 5,
                PositionY = 3,
                ShipSize = 4,
                IsHorizontal = true
            };

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            var response = service.AddNewBattleship(request);
            Assert.IsFalse(response.IsSuccess);
        }

        [TestMethod]
        public void TestAddBattleshipInvalidPosition()
        {
            InitializeMocks();

            var request = new AddBattleshipRequest
            {
                Player = Player.Player1,
                PositionX = 11,
                PositionY = 3,
                ShipSize = 4,
                IsHorizontal = true
            };

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            var response = service.AddNewBattleship(request);
            Assert.IsFalse(response.IsSuccess);
        }

        [TestMethod]
        public void TestAddBattleshipInvalidSize()
        {
            InitializeMocks();

            var request = new AddBattleshipRequest
            {
                Player = Player.Player1,
                PositionX = 5,
                PositionY = 3,
                ShipSize = 8,
                IsHorizontal = true
            };

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            var response = service.AddNewBattleship(request);
            Assert.IsFalse(response.IsSuccess);
        }

        [TestMethod]
        public void TestAddBattleshipOverlap()
        {
            InitializeMocks();
            Battleship battleship = new Battleship(Player.Player1, 4);

            gameServiceMock.Setup(x => x.GetCurrentGame()).Returns(new Game(10)
            {
                Id = 1,
                GameBoard = new List<BoardPosition>
                {
                    new BoardPosition(5, 3, battleship),
                    new BoardPosition(6, 3, battleship),
                    new BoardPosition(7, 3, battleship),
                    new BoardPosition(8, 3, battleship),
                },
                IsInProgress = true
            });

            var request = new AddBattleshipRequest
            {
                Player = Player.Player1,
                PositionX = 6,
                PositionY = 2,
                ShipSize = 3,
                IsHorizontal = false
            };

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            var response = service.AddNewBattleship(request);
            Assert.IsFalse(response.IsSuccess);
        }

        [TestMethod]
        public void TestAddBattleshipOverlapOnDifferentPlayer()
        {
            InitializeMocks();
            Battleship battleship = new Battleship(Player.Player1, 4);

            gameServiceMock.Setup(x => x.GetCurrentGame()).Returns(new Game(10)
            {
                Id = 1,
                GameBoard = new List<BoardPosition>
                {
                    new BoardPosition(5, 3, battleship),
                    new BoardPosition(6, 3, battleship),
                    new BoardPosition(7, 3, battleship),
                    new BoardPosition(8, 3, battleship),
                },
                IsInProgress = true
            });

            var request = new AddBattleshipRequest
            {
                Player = Player.Player2,
                PositionX = 6,
                PositionY = 2,
                ShipSize = 3,
                IsHorizontal = false
            };

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            var response = service.AddNewBattleship(request);
            Assert.IsFalse(response.IsSuccess);
        }

        [TestMethod]
        public void TestAttackBattleshipHit()
        {
            InitializeMocks();
            Battleship battleship = new Battleship(Player.Player1, 4);

            gameServiceMock.Setup(x => x.GetCurrentGame()).Returns(new Game(10)
            {
                Id = 1,
                GameBoard = new List<BoardPosition>
                {
                    new BoardPosition(5, 3, battleship),
                    new BoardPosition(6, 3, battleship),
                    new BoardPosition(7, 3, battleship),
                    new BoardPosition(8, 3, battleship),
                },
                IsInProgress = true
            });

            var request = new AttackBattleshipRequest
            {
                SourcePlayer = Player.Player2,
                TargetPlayer = Player.Player1,
                PositionX = 6,
                PositionY = 3
            };

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            var response = service.AttackBattleship(request);
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(AttackResult.Hit, response.Result);
        }

        [TestMethod]
        public void TestAttackBattleshipMiss()
        {
            InitializeMocks();
            Battleship battleship = new Battleship(Player.Player1, 4);
            
            gameServiceMock.Setup(x => x.GetCurrentGame()).Returns(new Game(10)
            {
                Id = 1,
                GameBoard = new List<BoardPosition>
                {
                    new BoardPosition(5, 3, battleship),
                    new BoardPosition(6, 3, battleship),
                    new BoardPosition(7, 3, battleship),
                    new BoardPosition(8, 3, battleship),
                },
                IsInProgress = true
            });

            var request = new AttackBattleshipRequest
            {
                SourcePlayer = Player.Player2,
                TargetPlayer = Player.Player1,
                PositionX = 4,
                PositionY = 3
            };

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            var response = service.AttackBattleship(request);
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(AttackResult.Miss, response.Result);
        }

        [TestMethod]
        public void TestAttackBattleshipSunk()
        {
            InitializeMocks();
            Battleship battleship = new Battleship(Player.Player1, 4);
            
            gameServiceMock.Setup(x => x.GetCurrentGame()).Returns(new Game(10)
            {
                Id = 1,
                GameBoard = new List<BoardPosition>
                {
                    new BoardPosition(5, 3, battleship) { IsHit = true },
                    new BoardPosition(6, 3, battleship),
                    new BoardPosition(7, 3, battleship) { IsHit = true },
                    new BoardPosition(8, 3, battleship) { IsHit = true },
                },
                IsInProgress = true
            });

            var request = new AttackBattleshipRequest
            {
                SourcePlayer = Player.Player2,
                TargetPlayer = Player.Player1,
                PositionX = 6,
                PositionY = 3
            };

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            var response = service.AttackBattleship(request);
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(AttackResult.Sunk, response.Result);
        }

        [TestMethod]
        public void TestAttackBattleshipSamePlayer()
        {
            InitializeMocks();
            Battleship battleship = new Battleship(Player.Player1, 4);
            
            gameServiceMock.Setup(x => x.GetCurrentGame()).Returns(new Game(10)
            {
                Id = 1,
                GameBoard = new List<BoardPosition>
                {
                    new BoardPosition(5, 3, battleship),
                    new BoardPosition(6, 3, battleship),
                    new BoardPosition(7, 3, battleship),
                    new BoardPosition(8, 3, battleship),
                },
                IsInProgress = true
            });

            var request = new AttackBattleshipRequest
            {
                SourcePlayer = Player.Player1,
                TargetPlayer = Player.Player2,
                PositionX = 6,
                PositionY = 3
            };

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            var response = service.AttackBattleship(request);
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(AttackResult.Miss, response.Result);
        }

        [TestMethod]
        public void TestGetGameBoardAfterAdd()
        {
            InitializeMocks();

            var request = new AddBattleshipRequest
            {
                Player = Player.Player1,
                PositionX = 5,
                PositionY = 3,
                ShipSize = 4,
                IsHorizontal = true
            };

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            service.AddNewBattleship(request);

            var response = service.GetGameBoard();
            Assert.AreEqual(4, response.BoardPositions.Count());
            Assert.AreEqual(5, response.BoardPositions.ElementAt(0).PositionX);
            Assert.AreEqual(3, response.BoardPositions.ElementAt(0).PositionY);
            Assert.AreEqual(8, response.BoardPositions.ElementAt(3).PositionX);
            Assert.AreEqual(3, response.BoardPositions.ElementAt(3).PositionY);
        }

        [TestMethod]
        public void TestGameBoardAfterAttack()
        {
            InitializeMocks();
            Battleship battleship = new Battleship(Player.Player1, 4);
            
            gameServiceMock.Setup(x => x.GetCurrentGame()).Returns(new Game(10)
            {
                Id = 1,
                GameBoard = new List<BoardPosition>
                {
                    new BoardPosition(5, 3, battleship),
                    new BoardPosition(6, 3, battleship),
                    new BoardPosition(7, 3, battleship),
                    new BoardPosition(8, 3, battleship),
                },
                IsInProgress = true
            });

            var request = new AttackBattleshipRequest
            {
                SourcePlayer = Player.Player2,
                TargetPlayer = Player.Player1,
                PositionX = 6,
                PositionY = 3
            };

            BattleshipService service = new BattleshipService(logServiceMock.Object, gameServiceMock.Object);
            service.AttackBattleship(request);

            var response = service.GetGameBoard();
            Assert.AreEqual(4, response.BoardPositions.Count());
            Assert.AreEqual(1, response.BoardPositions.Count(x => x.IsHit));
        }

        private void InitializeMocks()
        {
            logServiceMock = new Mock<ILogService>();
            logServiceMock.Setup(x => x.Log(It.IsAny<LogType>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            gameServiceMock = new Mock<IGameService>();
            gameServiceMock.Setup(x => x.InitializeNewGame(It.IsAny<Game>())).Verifiable();
            gameServiceMock.Setup(x => x.GetCurrentGame()).Returns(new Game(10)
            {
                Id = 1,
                GameBoard = new List<BoardPosition>(),
                IsInProgress = true
            });
        }
    }
}