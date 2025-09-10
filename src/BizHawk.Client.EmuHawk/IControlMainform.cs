namespace BizHawk.Client.EmuHawk
{
	public interface IControlMainform
	{
		bool WantsToControlReboot { get; }
		void RebootCore();

		bool WantsToControlSavestates { get; }

		void SaveState();

		bool LoadState();

		void SaveStateAs();

		bool LoadStateAs();

		void SaveQuickSave(int slot);

		bool LoadQuickSave(int slot);

		/// <summary>
		/// Overrides the select slot method
		/// </summary>
		/// <returns>Returns whether the function is handled.
		/// If false, the mainform should continue with its logic</returns>
		bool SelectSlot(int slot);
		bool PreviousSlot();
		bool NextSlot();

		bool WantsToControlReadOnly { get; }

		/// <summary>
		/// Function that is called by Mainform instead of using its own code
		/// when a Tool sets WantsToControlReadOnly.
		/// Should not be called directly.
		/// </summary>
		void ToggleReadOnly();

		bool WantsToControlStopMovie { get; }

		/// <summary>
		/// Function that is called by Mainform instead of using its own code
		/// when a Tool sets WantsToControlStopMovie.
		/// Should not be called directly.
		/// <remarks>Like MainForm's StopMovie(), saving the movie is part of this function's responsibility.</remarks>
		/// </summary>
		void StopMovie(bool suppressSave);

		bool WantsToControlRewind { get; }

		void CaptureRewind();

		/// <summary>
		/// Function that is called by Mainform instead of using its own code
		/// when a Tool sets WantsToControlRewind
		/// One or both of the parameters will be true.
		/// The return value is ignored if no rewind occured (the current emulator frame did not change).
		/// </summary>
		/// <param name="byHotkeyEvent">True if the rewind is happening because of a hotkey event.</param>
		/// <param name="byFrameProgress">True if the rewind is happening because it is time for the next frame and rewind hotkey is currently held.</param>
		/// <returns>Returns true if a frame advance is required.</returns>
		bool Rewind(bool byHotkeyEvent, bool byFrameProgress);

		bool WantsToControlRestartMovie { get; }

		bool RestartMovie();

		bool WantsToBypassMovieEndAction { get; }
	}
}
