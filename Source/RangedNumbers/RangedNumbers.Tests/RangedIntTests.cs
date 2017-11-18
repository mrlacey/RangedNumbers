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
            var sut = new RangedInt(1, 5);

            sut.SetValue(sut.MinValue);

            Assert.AreEqual(sut.Value, sut.MinValue);
        }

        [TestMethod]
        public void SettingToBelow_LowerBoundary_ReturnsLowerBoundary()
        {
            var sut = new RangedInt(2, 5);

            sut.SetValue(sut.MinValue - 1);

            Assert.AreEqual(sut.Value, sut.MinValue);
        }

        [TestMethod]
        public void CanSetTo_UpperBoundary()
        {
            var sut = new RangedInt(1, 5);

            sut.SetValue(sut.MaxValue);

            Assert.AreEqual(sut.Value, sut.MaxValue);
        }

        [TestMethod]
        public void SettingToAbove_UpperBoundary_ReturnsUpperBoundary()
        {
            var sut = new RangedInt(2, 5);

            sut.SetValue(sut.MaxValue + 1);

            Assert.AreEqual(sut.Value, sut.MaxValue);
        }

        [TestMethod]
        public void AddingToAbove_UpperBoundary_ReturnsUpperBoundary()
        {
            var sut = new RangedInt(2, 5);

            sut.SetValue(3);

            sut += 9;

            Assert.AreEqual(sut.Value, sut.MaxValue);
        }

        [TestMethod]
        public void SubtractingToBelow_LowerBoundary_ReturnsLowerBoundary()
        {
            var sut = new RangedInt(2, 5);

            sut.SetValue(3);

            sut -= 9;

            Assert.AreEqual(sut.Value, sut.MinValue);
        }

        [TestMethod]
        public void Adding_WorksWith_Int()
        {
            var sut = new RangedInt(2, 5);

            sut.SetValue(3);

            sut += 9;

            Assert.AreEqual(sut.Value, sut.MaxValue);
        }

        [TestMethod]
        public void Adding_WorksWith_RangedInt()
        {
            var sut = new RangedInt(2, 5);

            sut.SetValue(3);

            var newValue = new RangedInt(3, 6);
            newValue.SetValue(4);

            sut += newValue;  // ~3+4

            Assert.AreEqual(sut.Value, sut.MaxValue);
        }

        [TestMethod]
        public void Subtraction_WorksWith_Int()
        {
            var sut = new RangedInt(2, 5);

            sut.SetValue(3);

            sut -= 9;

            Assert.AreEqual(sut.Value, sut.MinValue);
        }

        [TestMethod]
        public void Subtraction_WorksWith_RangedInt()
        {
            var sut = new RangedInt(2, 5);

            sut.SetValue(3);

            var newValue = new RangedInt(3, 6);
            newValue.SetValue(4);

            sut -= newValue;  // ~3-4

            Assert.AreEqual(sut.Value, sut.MinValue);
        }

        [TestMethod]
        public void EventRaisedWhen_LowerBoundaryExceeded()
        {
            BoundaryExceededEventHandlerArgs raisedArgs = null;

            BoundaryExceededEventHandler onErr = (object sender, BoundaryExceededEventHandlerArgs args) => raisedArgs = args;

            var sut = new RangedInt(2, 5, 3, onErr);

            sut -= 4;

            Assert.AreEqual(sut.Value, sut.MinValue);
            Assert.IsNotNull(raisedArgs.ImpossibleValue);
            Assert.AreEqual(-1, raisedArgs?.ImpossibleValue ?? 0);
        }

        [TestMethod]
        public void HandleGoing_Above_IntMaxValue()
        {
            BoundaryExceededEventHandlerArgs raisedArgs = null;

            BoundaryExceededEventHandler onErr = (object sender, BoundaryExceededEventHandlerArgs args) => raisedArgs = args;

            var sut = new RangedInt(0, int.MaxValue, int.MaxValue - 1, onErr);

            sut += 4;

            // Assert.IsTrue(false, int.MaxValue.ToString()); // 2147483647

            Assert.AreEqual(int.MaxValue, (int)sut.Value);
            Assert.IsNotNull(raisedArgs.ImpossibleValue);
            Assert.AreEqual("2147483646 + 4 > int.MaxValue", raisedArgs?.ImpossibleValue ?? 0);
        }

        //[TestMethod]
        //public void HandleGoing_Above_IntMaxValue_WithRangedInt()
        //{
        //    BoundaryExceededEventHandlerArgs raisedArgs = null;

        //    BoundaryExceededEventHandler onErr = (object sender, BoundaryExceededEventHandlerArgs args) => raisedArgs = args;

        //    var sut = new RangedInt(0, int.MaxValue, int.MaxValue - 1, onErr);

        //    var addFive = new RangedInt(5);

        //    sut += addFive;

        //    Assert.AreEqual(int.MaxValue, (int)sut.Value);
        //    Assert.IsNotNull(raisedArgs.ImpossibleValue);
        //    Assert.AreEqual("2147483646 + 5 > int.MaxValue", raisedArgs?.ImpossibleValue ?? 0);
        //}

        [TestMethod]
        public void HandleGoing_Below_IntMinValue()
        {
            BoundaryExceededEventHandlerArgs raisedArgs = null;

            BoundaryExceededEventHandler onErr = (object sender, BoundaryExceededEventHandlerArgs args) => raisedArgs = args;

            var sut = new RangedInt(int.MinValue, int.MaxValue, int.MinValue + 1, onErr);

            sut -= 4;

            Assert.AreEqual(int.MinValue, (int)sut.Value);
            Assert.IsNotNull(raisedArgs.ImpossibleValue);
            Assert.AreEqual("-2147483647 - 4 < int.MinValue", raisedArgs?.ImpossibleValue ?? 0);
        }

        //[TestMethod]
        //public void HandleGoing_Below_IntMinValue_WithRangedItn()
        //{
        //    BoundaryExceededEventHandlerArgs raisedArgs = null;

        //    BoundaryExceededEventHandler onErr = (object sender, BoundaryExceededEventHandlerArgs args) => raisedArgs = args;

        //    var sut = new RangedInt(int.MinValue, int.MaxValue, int.MinValue + 1, onErr);


        //    var four = new RangedInt(4);

        //    sut -= four;

        //    Assert.AreEqual(int.MinValue, (int)sut.Value);
        //    Assert.IsNotNull(raisedArgs.ImpossibleValue);
        //    Assert.AreEqual("-2147483647 - 4 < int.MinValue", raisedArgs?.ImpossibleValue ?? 0);
        //}
    }
}
