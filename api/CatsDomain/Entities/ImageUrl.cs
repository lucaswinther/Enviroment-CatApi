using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCatsDomain.Entities
{
	public class ImageUrl
	{
		protected ImageUrl()
		{
		}

		public ImageUrl(string imageUrlId, string url)
		{
			ImageUrlId = imageUrlId;
			Url = url;
		}

		public string ImageUrlId { get; private set; }
		public string Url { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		public Breeds Breeds { get; private set; }
		public Category Category { get; private set; }

		public void SetImageUrlId(string id)
		{
			if (IdIsValid(id))
				ImageUrlId = id;
		}

		public void SetUrl(string url)
		{
			if (UrlIsValid(url))
				Url = url;
		}

		public void SetWidth(int width)
		{
			if (width > 0)
				Width = width;
		}

		public void SetHeight(int height)
		{
			if (height > 0)
				Height = height;
		}

		public void SetBreeds(Breeds breeds)
		{
			if (breeds != null)
				Breeds = breeds;
		}

		public void SetCategory(Category category)
		{
			if (category != null)
				Category = category;
		}

		public bool IsValid() =>
			IdIsValid(ImageUrlId) &&
			UrlIsValid(Url);

		bool IdIsValid(string imageUrlId) => (!string.IsNullOrEmpty(imageUrlId) && imageUrlId.Length <= 80);
		bool UrlIsValid(string url) => (!string.IsNullOrEmpty(url) && url.Length <= 512);
	}
}
