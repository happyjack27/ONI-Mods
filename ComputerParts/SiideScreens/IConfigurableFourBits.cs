/*
 * Copyright 2020 Dense Logic Team
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

namespace ONI_DenseLogic {
	/// <summary>
	/// Tagged on configurable buildings that can have mulitple bits set to
	/// provide hooks for the bit select side screen.
	/// </summary>
	public interface IConfigurableFourBits {
		/// <summary>
		/// Sets the value of a bit at a specific position.
		/// </summary>
		/// <param name="value">The value to set.</param>
		/// <param name="pos">The position at which to set the value.</param>
		void SetBit(bool value, int pos);

		/// <summary>
		/// Gets the value of a bit at a specific position.
		/// </summary>
		/// <param name="pos">The position at which to get the value.</param>
		/// <returns>The value of the bit at that position.</returns>
		bool GetBit(int pos);
	}
}
