#nullable enable

using BizHawk.Client.Common;
using BizHawk.Emulation.Common;

namespace BizHawk.Client.EmuHawk
{
	public class LuaDependencyProvider : ToolFormDependencyProvider
	{
		public IEmulator Emulator => Get<IEmulator>()!;

		public ILuaLogger? Logger => Get<ILuaLogger>();

		public LuaDependencyProvider(
			IMainFormForTools mainForm,
			IMovieSession movieSession,
			IGameInfo gameInfo,
			Config config,
			IEmulator emulator,
			ILuaLogger? logger = null
		) : base(mainForm, movieSession, gameInfo, config)
		{
			Set<IEmulator>(emulator);
			if (logger != null) Set<ILuaLogger>(logger);
		}
	}

	public interface ILuaLogger
	{
		void Log(string message);
	}
}
