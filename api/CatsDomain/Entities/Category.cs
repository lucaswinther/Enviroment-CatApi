using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCatsDomain.Entities
{
	public class Category
	{
		protected Category()
		{
		}

		public Category(int categoryId, string name)
		{
			CategoryId = categoryId;
			Name = name;
		}

		public int CategoryId { get; private set; }
		public string Name { get; private set; }

		public ICollection<ImageUrl> Images { get; set; } = new List<ImageUrl>();

		public void SetCategoryId(int id)
		{
			if (IdIsValid(id))
				CategoryId = id;
		}

		public void SetName(string name)
		{
			if (NameIsValid(name))
				Name = name;
		}

		public bool IsValid() =>
			IdIsValid(CategoryId) &&
			NameIsValid(Name);

		bool IdIsValid(int categoryId) => (categoryId > 0);
		bool NameIsValid(string name) => (!string.IsNullOrEmpty(name) && name.Length <= 255);
	}
}
