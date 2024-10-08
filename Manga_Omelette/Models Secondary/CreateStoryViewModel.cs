﻿using MangaASP.Models;

namespace Manga_Omelette.Models_Secondary
{
    public class CreateStoryViewModel
    {
        public Story story {  get; set; }
        public IFormFile? imageFile { get; set; }
        public string GenreIds { get; set; }
		public List<Genre> AllGenre { get; set; }
        public List<Author> AllAuthor { get; set; }
	}
}
