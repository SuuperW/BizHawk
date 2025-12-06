using System.Collections.Generic;
using System.IO;

using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Emulation.Common;
using BizHawk.Tests.Client.Common;
using BizHawk.Tests.Client.Common.Movie;

namespace BizHawk.Tests.Client.EmuHawk.lua
{
	[DoNotParallelize]
	[TestClass]
	public class LuaEventTests
	{
		private static readonly string BASE_SCRIPT_PATH = Path.Combine(Environment.CurrentDirectory, "lua/scripts");

		private class Context : ILuaLogger
		{
			private LuaConsole luaTool;

			public List<string> loggedMessages = new();

			public Context()
			{
				IEmulator emulator = new FakeEmulator();
				LuaDependencyProvider dependencyProvider = new(
					new FakeMainFormTools(new FakeMainFormForApi()),
					new FakeMovieSession(emulator),
					new GameInfo(),
					new Config(),
					emulator,
					this
				);
				luaTool = new(dependencyProvider);
				luaTool.Restart();
			}

			void ILuaLogger.Log(string message) => loggedMessages.Add(message);

			public void AddScript(string path, bool enable)
			{
				luaTool.Settings.DisableLuaScriptsOnLoad = !enable;
				luaTool.LoadLuaFile(Path.Combine(BASE_SCRIPT_PATH, path));
			}

			public void RunYielding()
			{
				luaTool.ResumeScripts(false);
			}

			public void RunFrameWaiting()
			{
				luaTool.ResumeScripts(true);
			}

			public void AssertLogMatches(params string[] messages)
			{
				Assert.AreEqual(messages.Length, loggedMessages.Count);
				for (int i = 0; i < messages.Length; i++)
				{
					// The Lua console api will append a newline to every print
					Assert.AreEqual(messages[i] + '\n', loggedMessages[i]);
				}
			}
		}

		[TestMethod]
		public void CanPrint()
		{
			// arrange
			Context context = new();

			// act
			context.AddScript("say_foo.lua", true);
			context.RunYielding();

			// assert
			context.AssertLogMatches("foo");
		}
	}
}
