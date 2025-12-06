#nullable enable

namespace BizHawk.Client.Common
{
	public interface IDependencyProvider
	{
		T? Get<T>() where T : class;
	}
}
