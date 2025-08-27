using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using BizHawk.Emulation.Common;

namespace BizHawk.Client.Common
{
	/// <summary>
	/// Some hotkeys trigger an event that should be repeated while the button is held.
	/// This adapter manages the repeating logic, presenting its buttons as held if it is time for the next event.<br/>
	/// When a button state is read (with <see cref="IsPressed(string)"/>) it is then set to false, until at least the next <see cref="Update()"/>.
	/// Buttons not specified as repeatable will be read from the source.
	/// </summary>
	public class RepeatableEventAdapter : IInputAdapter
	{
		public IController Source { get; set; }

		public ControllerDefinition Definition { get; private set; }

		private Dictionary<string, long> _lastPressTime = new();
		private Dictionary<string, bool> _isFastRepeating = new();
		private Dictionary<string, bool> _buttonStates = new();

		private long _initialDelay;
		private long _shortDelay;

		/// <param name="source">The "physical" controller.</param>
		/// <param name="repeatableButtons">Buttons that trigger repeatable events.</param>
		/// <param name="initialDelay">The number of milliseconds between the initial press and the first repeat.</param>
		/// <param name="shortDelay">The number of milliseconds between repeats after the initial delay.</param>
		public RepeatableEventAdapter(IController source, IEnumerable<string> repeatableButtons, long initialDelay, long shortDelay)
		{
			Source = source;
			Definition = new ControllerDefinition($"Repeater for {source.Definition.Name}")
			{
				BoolButtons = repeatableButtons.ToList(),
			}.MakeImmutable();

			foreach (string button in repeatableButtons)
			{
				_buttonStates[button] = false;
				_isFastRepeating[button] = false;
			}

			_initialDelay = (long)(initialDelay * (Stopwatch.Frequency / 1000.0));
			_shortDelay = (long)(shortDelay * (Stopwatch.Frequency / 1000.0));
		}

		/// <summary>
		/// Reads the source controller and updates the state of this adapter's buttons.
		/// </summary>
		public void Update()
		{
			long currentTimestamp = Stopwatch.GetTimestamp();

			foreach (string button in Definition.BoolButtons)
			{
				if (Source.IsPressed(button))
				{
					bool doNewPress = false;
					if (!_lastPressTime.TryGetValue(button, out long lastTimestamp))
					{
						// Initial hotkey should have already been triggered, so not a new press here.
						_lastPressTime[button] = currentTimestamp;
					}
					else if (_isFastRepeating[button] && currentTimestamp - lastTimestamp > _shortDelay)
					{
						doNewPress = true;
					}
					else if (currentTimestamp - lastTimestamp > _initialDelay)
					{
						doNewPress = true;
						_isFastRepeating[button] = true;
					}

					if (doNewPress)
					{
						_lastPressTime[button] = currentTimestamp;
						_buttonStates[button] = true;
					}
				}
				else
				{
					_lastPressTime.Remove(button);
					_buttonStates[button] = false;
					_isFastRepeating[button] = false;
				}
			}
		}

		/// <summary>
		/// Triggers all repeatable hotkeys that are due and clears their press state.
		/// </summary>
		public void ProcessEvents(Action<string> handler)
		{
			List<string> pressedButtons = _buttonStates.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
			foreach (string button in pressedButtons)
			{
				handler(button);
				_buttonStates[button] = false;
			}
		}

		public bool IsPressed(string button)
		{
			if (_buttonStates.TryGetValue(button, out bool ret))
			{
				_buttonStates[button] = false;
				return ret;
			}
			else
			{
				return Source.IsPressed(button);
			}
		}

		public int AxisValue(string name) => Source.AxisValue(name);

		public IReadOnlyCollection<(string Name, int Strength)> GetHapticsSnapshot() => Source.GetHapticsSnapshot();

		public void SetHapticChannelStrength(string name, int strength) => Source.SetHapticChannelStrength(name, strength);
	}
}
