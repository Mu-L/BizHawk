using BizHawk.Client.Common;
using BizHawk.Common;
using BizHawk.Emulation.Common;

namespace BizHawk.Tests.Client.Common.Display
{
	[TestClass]
	public class InputDisplayTests
	{
		private const int MidValue = 100;
		private SimpleController _boolController = null!;
		private SimpleController _axisController = null!;

		[TestInitialize]
		public void Initializer()
		{
			_boolController = new(new ControllerDefinition("Dummy Gamepad") { BoolButtons = { "A" } }.MakeImmutable());
			_boolController.Definition.BuildMnemonicsCache(VSystemID.Raw.NULL);
			_axisController = new(
				new ControllerDefinition("Dummy Gamepad")
					.AddXYPair("Stick{0}", AxisPairOrientation.RightAndUp, 0.RangeTo(200), MidValue)
					.MakeImmutable());
			_axisController.Definition.BuildMnemonicsCache(VSystemID.Raw.NULL);
		}

#pragma warning disable BHI1600 //TODO disambiguate assert calls
		[TestMethod]
		public void Generate_BoolPressed_GeneratesMnemonic()
		{
			_boolController["A"] = true;
			var actual = Bk2InputDisplayGenerator.Generate(_boolController);
			Assert.AreEqual("A", actual);
		}

		[TestMethod]
		public void Generate_BoolUnPressed_GeneratesSpace()
		{
			_boolController["A"] = false;
			var actual = Bk2InputDisplayGenerator.Generate(_boolController);
			Assert.AreEqual(" ", actual);
		}

		[TestMethod]
		public void Generate_Floats()
		{
			_axisController.AcceptNewAxis("StickX", 0);
			_axisController.AcceptNewAxis("StickY", 0);
			var actual = Bk2InputDisplayGenerator.Generate(_axisController);
			Assert.AreEqual("    0,    0,", actual);
		}

		[TestMethod]
		public void Generate_MidRangeDisplaysEmpty()
		{
			var actual = Bk2InputDisplayGenerator.Generate(_axisController);
			Assert.AreEqual("            ", actual);
			_axisController.AcceptNewAxis("StickX", MidValue);
			actual = Bk2InputDisplayGenerator.Generate(_axisController);
			Assert.AreEqual("            ", actual);
		}
#pragma warning restore BHI1600
	}
}
