using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Versioning
{
	public interface IVersioningService<T> where T : IContentItem
	{
		T CreateNewVersion(T entity);
		T SetToVersion(T entity, string version);
		void DeleteVersion(T entity, string version);

	}
}
