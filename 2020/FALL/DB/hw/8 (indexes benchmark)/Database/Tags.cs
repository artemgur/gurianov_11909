using System.Threading.Tasks;

namespace Database
{
	public class Tags
	{
		public static async Task<string[]> Get() => await General.GetEnum("TAG");
	}
}