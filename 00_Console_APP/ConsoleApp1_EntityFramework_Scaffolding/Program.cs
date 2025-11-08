
using ConsoleApp1_EntityFramework_Scaffolding;

namespace ConsoleApp1
{
	class Program
	{
		public static void Main(string[] args)
		{
			using (var instance = new PmsContext())
			{
				instance.Database.EnsureCreated();
				
				Console.WriteLine(instance.Projects.Count());
			}
		}
	}
}
