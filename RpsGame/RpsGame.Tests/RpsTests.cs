using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RpsGame.Model;

namespace RpsGame.Tests
{
    class RpsTests
    {
        [Test]
        public void Should_calculcate_winner()
        {
            Assert.That(Move.Scissors.IsWinner(Move.Paper));
            Assert.That(Move.Scissors.IsWinner(Move.Rock),Is.False);
            Assert.That(Move.Scissors.IsWinner(Move.Scissors),Is.False);

            Assert.That(Move.Rock.IsWinner(Move.Scissors));
            Assert.That(Move.Rock.IsWinner(Move.Paper),Is.False);
            Assert.That(Move.Rock.IsWinner(Move.Rock),Is.False);

            Assert.That(Move.Paper.IsWinner(Move.Rock));
            Assert.That(Move.Paper.IsWinner(Move.Scissors),Is.False);
            Assert.That(Move.Paper.IsWinner(Move.Paper),Is.False);
        }
    }
}
