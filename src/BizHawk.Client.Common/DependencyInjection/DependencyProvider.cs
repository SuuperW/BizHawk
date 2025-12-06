#nullable enable

using System.Collections.Generic;

namespace BizHawk.Client.Common
{
	public class DependencyProvider : IDependencyProvider
	{
		private Dictionary<Type, object> _stuff = new();

		public T? Get<T>() where T : class
		{
			if (_stuff.TryGetValue(typeof(T), out object thing))
				return thing as T;
			else
				return null;
		}

		public void Set<T>(T thing) where T : class
		{
			if (thing is not T) throw new Exception("Dependency thing has incorrect type.");
			_stuff[typeof(T)] = thing;
		}
	}
}
