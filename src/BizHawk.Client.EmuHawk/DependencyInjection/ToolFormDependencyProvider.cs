using BizHawk.Client.Common;
using BizHawk.Emulation.Common;

namespace BizHawk.Client.EmuHawk
{
	public class ToolFormDependencyProvider : DependencyProvider
	{
		public IMainFormForTools MainForm => Get<IMainFormForTools>()!;

		public IMovieSession MovieSession => Get<IMovieSession>()!;

		public IGameInfo GameInfo => Get<IGameInfo>()!;

		public Config Config => Get<Config>()!;

		// DisplayManager DisplayManager { get; } // not needed right now, no interface

		// InputManager InputManager { get; } // not needed right now, no interface

		// ToolManager toolManager { get; } // not needed right now, no interface

		public ToolFormDependencyProvider(IMainFormForTools mainForm, IMovieSession movieSession, IGameInfo game, Config config)
		{
			Set<IMainFormForTools>(mainForm);
			Set<IMovieSession>(movieSession);
			Set<IGameInfo>(game);
			Set<Config>(config);
		}
	}
}
