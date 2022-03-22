

using System.Collections.Generic;

namespace KBComputing {
	public interface IStack {

		Stack<byte> getStack();
		byte getFlags();
		byte getOp();
	}
}
