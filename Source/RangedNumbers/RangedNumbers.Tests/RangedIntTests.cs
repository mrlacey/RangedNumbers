using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RangedNumbers;

namespace RangedNumbers.Tests
{
    [TestClass]
    public class RangedIntTests
    {
        [TestMethod]
        public void CanCreateWith_int()
        {
            int creator = 4;

            RangedInt sut = creator;

            Assert.AreEqual(sut.Value, creator);
            Assert.AreEqual(sut.MinValue, int.MinValue);
            Assert.AreEqual(sut.MaxValue, int.MaxValue);
        }

        [TestMethod]
        public void CanSetTo_LowerBoundary()
        {
            var sut = new RangedInt(2, 1, 5);

            sut.SetValue(sut.MinValue);

            Assert.AreEqual(sut.Value, sut.MinValue);
        }

        [TestMethod]
        public void SettingToBelow_LowerBoundary_ReturnsLowerBoundary()
        {
            var sut = new RangedInt(3, 2, 5);

            sut.SetValue(sut.MinValue - 1);

            Assert.AreEqual(sut.Value, sut.MinValue);
        }

        [TestMethod]
        public void CanSetTo_UpperBoundary()
        {
            var sut = new RangedInt(3, 1, 5);

            sut.SetValue(sut.MaxValue);

            Assert.AreEqual(sut.Value, sut.MaxValue);
        }

        [TestMethod]
        public void SettingToAbove_UpperBoundary_ReturnsUpperBoundary()
        {
            var sut = new RangedInt(3, 2, 5);

            sut.SetValue(sut.MaxValue + 1);

            Assert.AreEqual(sut.Value, sut.MaxValue);
        }

        [TestMethod]
        public void AddingToAbove_UpperBoundary_ReturnsUpperBoundary()
        {
            var sut = new RangedInt(3, 2, 5);

            sut += 9;

            Assert.AreEqual(sut.Value, sut.MaxValue);
        }

        [TestMethod]
        public void SubtractingToBelow_LowerBoundary_ReturnsLowerBoundary()
        {
            var sut = new RangedInt(4, 2, 5);

            sut -= 9;

            Assert.AreEqual(sut.Value, sut.MinValue);
        }

        [TestMethod]
        public void Adding_WorksWith_Int()
        {
            var sut = new RangedInt(3, 2, 5);

            sut += 9;

            Assert.AreEqual(sut.Value, sut.MaxValue);
        }

        [TestMethod]
        public void Adding_WorksWith_RangedInt()
        {
            var sut = new RangedInt(3, 2, 5);

            var newValue = new RangedInt(4, 3, 6);

            sut += newValue;  // ~3+4

            Assert.AreEqual(sut.Value, sut.MaxValue);
        }

        [TestMethod]
        public void Subtraction_WorksWith_Int()
        {
            var sut = new RangedInt(3, 2, 5);

            sut -= 9;

            Assert.AreEqual(sut.Value, sut.MinValue);
        }

        [TestMethod]
        public void Subtraction_WorksWith_RangedInt()
        {
            var sut = new RangedInt(3, 2, 5);

            var newValue = new RangedInt(4, 3, 6);

            sut -= newValue;  // ~3-4

            Assert.AreEqual(sut.Value, sut.MinValue);
        }

        [TestMethod]
        public void EventRaisedWhen_LowerBoundaryExceeded()
        {
            BoundaryExceededArgs raisedArgs = null;

            Action<BoundaryExceededArgs> onErr = (args) => raisedArgs = args;

            var sut = new RangedInt(3, 2, 5, onErr);

            sut -= 4;

            Assert.AreEqual(sut.Value, sut.MinValue);
            Assert.IsNotNull(raisedArgs.ImpossibleValue);
            Assert.AreEqual(-1, raisedArgs?.ImpossibleValue ?? 0);
        }

        [TestMethod]
        public void HandleGoing_Above_IntMaxValue()
        {
            BoundaryExceededArgs raisedArgs = null;

            Action<BoundaryExceededArgs> onErr = (args) => raisedArgs = args;

            var sut = new RangedInt(int.MaxValue - 1, 0, int.MaxValue, onErr);

            sut += 4;

            // Assert.IsTrue(false, int.MaxValue.ToString()); // 2147483647

            Assert.AreEqual(int.MaxValue, (int)sut.Value);
            Assert.IsNotNull(raisedArgs.ImpossibleValue);
            Assert.AreEqual("2147483646 + 4 > int.MaxValue", raisedArgs?.ImpossibleValue ?? 0);
        }

        [TestMethod]
        public void HandleGoing_Above_IntMaxValue_WithRangedInt()
        {
            BoundaryExceededArgs raisedArgs = null;

            Action<BoundaryExceededArgs> onErr = (args) => raisedArgs = args;

            var sut = new RangedInt(int.MaxValue - 1, 0, int.MaxValue, onErr);

            var addFive = new RangedInt(5);

            sut += addFive;

            Assert.AreEqual(int.MaxValue, (int)sut.Value);
            Assert.IsNotNull(raisedArgs.ImpossibleValue);
            Assert.AreEqual("2147483646 + 5 > int.MaxValue", raisedArgs?.ImpossibleValue ?? 0);
        }

        [TestMethod]
        public void HandleGoing_Below_IntMinValue()
        {
            BoundaryExceededArgs raisedArgs = null;

            Action<BoundaryExceededArgs> onErr = (args) => raisedArgs = args;

            var sut = new RangedInt(int.MinValue + 1, int.MinValue, int.MaxValue, onErr);

            sut -= 4;

            Assert.AreEqual(int.MinValue, (int)sut.Value);
            Assert.IsNotNull(raisedArgs.ImpossibleValue);
            Assert.AreEqual("-2147483647 - 4 < int.MinValue", raisedArgs?.ImpossibleValue ?? 0);
        }

        [TestMethod]
        public void HandleGoing_Below_IntMinValue_WithRangedInt()
        {
            BoundaryExceededArgs raisedArgs = null;

            Action<BoundaryExceededArgs> onErr = (args) => raisedArgs = args;

            var sut = new RangedInt(int.MinValue + 1, int.MinValue, int.MaxValue, onErr);

            var four = new RangedInt(4);

            sut -= four;

            Assert.AreEqual(int.MinValue, (int)sut.Value);
            Assert.IsNotNull(raisedArgs.ImpossibleValue);
            Assert.AreEqual("-2147483647 - 4 < int.MinValue", raisedArgs?.ImpossibleValue ?? 0);
        }
    }
}
