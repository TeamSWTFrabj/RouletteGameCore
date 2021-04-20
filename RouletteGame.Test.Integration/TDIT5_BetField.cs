using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using RouletteGame.Bets;
using RouletteGame.Fields;
using RouletteGame.Output;
using RouletteGame.Randomizing;

namespace RouletteGame.Tests.Integration
{
    [TestFixture]
    class TDIT5_BetField
    {
        private Game.Game _rouletteGame;
        private Roulette.Roulette _roulette;

        private StandardFieldFactory _fieldFactory;

        private FieldBet _fieldBet;
        private ColorBet _colorBet;
        private EvenOddBet _evenOddBet;

        private IOutput _fakeOutput;
        private IRandomizer _fakeRandomizer;

        [SetUp]
        public void Setup()
        {
            _fakeOutput = Substitute.For<IOutput>();
            _fakeRandomizer = Substitute.For<IRandomizer>();

            _fieldFactory = new StandardFieldFactory();
            _roulette = new Roulette.Roulette(_fieldFactory, _fakeRandomizer);

            _rouletteGame = new Game.Game(_roulette, _fakeOutput);

            _fieldBet = new FieldBet("Bente", 100, 2);
            _colorBet = new ColorBet("Bjarne", 100, FieldColor.Black);
            _evenOddBet = new EvenOddBet("Berit", 100, Parity.Even);
        }

        [Test]
        public void FieldBet_CorrectNumber_AmountCorrect()
        {
            _fakeRandomizer.Next().Returns(2U);

            _rouletteGame.OpenBets();
            _rouletteGame.PlaceBet(_fieldBet);
            _rouletteGame.CloseBets();
            _rouletteGame.SpinRoulette();
            _rouletteGame.PayUp();

            _fakeOutput.Received(1).Report(Arg.Is<string>(s => 
                s.ToLower().Contains("3600")
                &&
                s.ToLower().Contains("bente")));
        }

        [Test]
        public void FieldBet_WrongNumber_NoPayment()
        {
            _fakeRandomizer.Next().Returns(3U);

            _rouletteGame.OpenBets();
            _rouletteGame.PlaceBet(_fieldBet);
            _rouletteGame.CloseBets();
            _rouletteGame.SpinRoulette();
            _rouletteGame.PayUp();

            _fakeOutput.DidNotReceive().Report(Arg.Is<string>(s => s.Contains("Bente")));
        }

        [Test]
        public void ColorBet_CorrectColor_AmountCorrect()
        {
            _fakeRandomizer.Next().Returns(2U);

            _rouletteGame.OpenBets();
            _rouletteGame.PlaceBet(_colorBet);
            _rouletteGame.CloseBets();
            _rouletteGame.SpinRoulette();
            _rouletteGame.PayUp();

            _fakeOutput.Received(1).Report(Arg.Is<string>(s =>
                s.ToLower().Contains("200")
                &&
                s.ToLower().Contains("bjarne")));
        }

        [Test]
        public void ColorBet_WrongColor_AmountCorrect()
        {
            _fakeRandomizer.Next().Returns(3U);

            _rouletteGame.OpenBets();
            _rouletteGame.PlaceBet(_colorBet);
            _rouletteGame.CloseBets();
            _rouletteGame.SpinRoulette();
            _rouletteGame.PayUp();

            _fakeOutput.DidNotReceive().Report(Arg.Is<string>(s => s.Contains("Bjarne")));
        }

        [Test]
        public void EvenOddBet_CorrectParity_AmountCorrect()
        {
            _fakeRandomizer.Next().Returns(2U);

            _rouletteGame.OpenBets();
            _rouletteGame.PlaceBet(_evenOddBet);
            _rouletteGame.CloseBets();
            _rouletteGame.SpinRoulette();
            _rouletteGame.PayUp();

            _fakeOutput.Received(1).Report(Arg.Is<string>(s =>
                s.ToLower().Contains("200")
                &&
                s.ToLower().Contains("berit")));
        }

        [Test]
        public void EvenOddBet_WrongParity_AmountCorrect()
        {
            _fakeRandomizer.Next().Returns(3U);

            _rouletteGame.OpenBets();
            _rouletteGame.PlaceBet(_evenOddBet);
            _rouletteGame.CloseBets();
            _rouletteGame.SpinRoulette();
            _rouletteGame.PayUp();

            _fakeOutput.DidNotReceive().Report(Arg.Is<string>(s => s.Contains("Berit")));
        }

    }
}
