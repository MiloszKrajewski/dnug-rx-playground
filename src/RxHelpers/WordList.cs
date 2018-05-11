using System.Linq;

namespace RxHelpers
{
	public class WordList
	{
		public readonly string[] Words =
			Resources.Words.Split('\n').Select(w => w.Trim()).ToArray();
	}
}
