namespace Peachyys {
	class Program {
		static void Main() {
			Bot peachyys = new();
			peachyys.RunAsync().GetAwaiter().GetResult();
		}
	}
}
