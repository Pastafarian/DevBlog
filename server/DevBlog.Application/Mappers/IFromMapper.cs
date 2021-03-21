using System.Threading.Tasks;

namespace DevBlog.Application.Mappers
{
	public interface IFromMapper<in TFrom, out TTo>
	{
		TTo Map(TFrom model);
	}

	public interface IFromAsyncMapper<in TFrom, TTo>
	{
		Task<TTo> Map(TFrom model);
	}
}
