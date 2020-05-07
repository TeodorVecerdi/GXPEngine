using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GXPEngine.Core;

namespace GXPEngine {
	/// <summary>
	///     The Input class contains functions for reading keys and mouse
	/// </summary>
	public class Input {
		private static readonly Dictionary<string, (List<int>, List<int>)> Axes = new Dictionary<string, (List<int>, List<int>)>();
		private static readonly Dictionary<string, (int, bool)> Buttons = new Dictionary<string, (int, bool)>();
		/// <summary>
		///     Gets the current mouse x position in pixels.
		/// </summary>
		public static int mouseX => GLContext.mouseX;

		/// <summary>
		///     Gets the current mouse y position in pixels.
		/// </summary>
		public static int mouseY => GLContext.mouseY;

		/// <summary>
		/// 	Gets the current mouse position in pixels as a vector
		/// </summary>
		public static Vector2 mousePosition => new Vector2(mouseX, mouseY);

		/// <summary>
		///     Returns 'true' if given key is down, else returns 'false'
		/// </summary>
		/// <param name='key'>
		///     Key number, use Key.KEYNAME or integer value.
		/// </param>
		public static bool GetKey(int key) {
            return GLContext.GetKey(key);
        }

		/// <summary>
		///     Returns 'true' if specified key was pressed down during the current frame
		/// </summary>
		/// <param name='key'>
		///     Key number, use Key.KEYNAME or integer value.
		/// </param>
		public static bool GetKeyDown(int key) {
            return GLContext.GetKeyDown(key);
        }

		/// <summary>
		///     Returns 'true' if specified key was released during the current frame
		/// </summary>
		/// <param name='key'>
		///     Key number, use Key.KEYNAME or integer value.
		/// </param>
		public static bool GetKeyUp(int key) {
            return GLContext.GetKeyUp(key);
        }
		
		/// <summary>
		///     Returns 'true' if any key is down, else returns 'false'
		/// </summary>
		public static bool AnyKey() {
			return GLContext.AnyKey();
		}

		/// <summary>
		///     Returns 'true' if any key was pressed down during the current frame
		/// </summary>
		public static bool AnyKeyDown() {
			return GLContext.AnyKeyDown();
		}

		/// <summary>
		///     Returns 'true' if any key was released during the current frame
		/// </summary>
		public static bool AnyKeyUp() {
			return GLContext.AnyKeyUp();
		}
		
		/// <summary>
		///     Returns the last key
		/// </summary>
		public static int LastKey => GLContext.LastKey();
		

		/// <summary>
		///     Returns the last key down
		/// </summary>
		public static int LastKeyDown => GLContext.LastKeyDown();
		

		/// <summary>
		///     Returns the last key up
		/// </summary>
		public static bool LastKeyUp => GLContext.AnyKeyUp();
		

		/// <summary>
		/// 	Returns the string equivalent of the pressed key
		/// </summary>
		/// <param name="key">The key</param>
		/// <returns>The string equivalent of the pressed key</returns>
		public static string KeyToString(int key) {
			if (!Key.KEY_TO_STRING.ContainsKey(key) || !Key.KEY_TO_STRING_UPPER.ContainsKey(key)) return "";
			
			var upper = GetKey(Key.LEFT_SHIFT) || GetKey(Key.RIGHT_SHIFT);
			return upper ? Key.KEY_TO_STRING_UPPER[key] : Key.KEY_TO_STRING[key];
		}

		/// <summary>
		///     Returns 'true' if mousebutton is down, else returns 'false'
		/// </summary>
		/// <param name='button'>
		///     Number of button:
		///     0 = left button
		///     1 = right button
		///     2 = middle button
		/// </param>
		public static bool GetMouseButton(int button) {
            return GLContext.GetMouseButton(button);
        }

		/// <summary>
		///     Returns 'true' if specified mousebutton was pressed down during the current frame
		/// </summary>
		/// <param name='button'>
		///     Number of button:
		///     0 = left button
		///     1 = right button
		///     2 = middle button
		/// </param>
		public static bool GetMouseButtonDown(int button) {
            return GLContext.GetMouseButtonDown(button);
        }

		/// <summary>
		///     Returns 'true' if specified mousebutton was released during the current frame
		/// </summary>
		/// <param name='button'>
		///     Number of button:
		///     0 = left button
		///     1 = right button
		///     2 = middle button
		/// </param>
		public static bool GetMouseButtonUp(int button) {
            return GLContext.GetMouseButtonUp(button); /*courtesy of LeonB*/
        }

		public static void AddAxis(string axisName, List<int> negativeKeys, List<int> positiveKeys, bool flipped = false) {
			if(flipped)
				Axes.Add(axisName, ValueTuple.Create(positiveKeys, negativeKeys));
			else
				Axes.Add(axisName, ValueTuple.Create(negativeKeys, positiveKeys));
		}

		public static float GetAxis(string axisName) {
			if (!Axes.ContainsKey(axisName)) {
				Console.Error.WriteLine($"Axis {axisName} does not exist.");
				throw new KeyNotFoundException($"Axis {axisName} does not exist.");
			}
			var value = 0f;
			if (Axes[axisName].Item1.Any(GetKey)) value -= 1f;
			if (Axes[axisName].Item2.Any(GetKey)) value += 1f;
			return value;
		}
		public static float GetAxisDown(string axisName) {
			if (!Axes.ContainsKey(axisName)) {
				Console.Error.WriteLine($"Axis {axisName} does not exist.");
				throw new KeyNotFoundException($"Axis {axisName} does not exist.");
			}
			var value = 0f;
			if (Axes[axisName].Item1.Any(GetKeyDown)) value -= 1f;
			if (Axes[axisName].Item2.Any(GetKeyDown)) value += 1f;
			return value;
		}

		public static void AddButton(string button, int key, bool isKey) {
			Buttons.Add(button, (key, isKey));
		}

		public static bool GetButton(string button) {
			if (!Buttons.ContainsKey(button)) {
				throw new ArgumentException($"Button {button} is not defined.");
			}
			var (key, isKey) = Buttons[button];
			return isKey ? GetKey(key) : GetMouseButton(key);
		}
		public static bool GetButtonDown(string button) {
			if (!Buttons.ContainsKey(button)) {
				throw new ArgumentException($"Button {button} is not defined.");
			}
			var (key, isKey) = Buttons[button];
			return isKey ? GetKeyDown(key) : GetMouseButtonDown(key);
		}
		
		public static bool GetButtonUp(string button) {
			if (!Buttons.ContainsKey(button)) {
				throw new ArgumentException($"Button {button} is not defined.");
			}
			var (key, isKey) = Buttons[button];
			return isKey ? GetKeyUp(key) : GetMouseButtonUp(key);
		}
    }
}