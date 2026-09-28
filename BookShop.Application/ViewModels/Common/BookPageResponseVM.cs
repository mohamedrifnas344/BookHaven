using BookShop.Application.ViewModels.Author;
using BookShop.Application.ViewModels.Book;
using BookShop.Application.ViewModels.Category;
using BookShop.Application.ViewModels.Rating;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.ViewModels.Common
{
    public class BookPageResponseVM
    {
        public ICollection<BookResponseVM> Books { get; set; } = new List<BookResponseVM>();
        public ICollection<AuthorResponseVM> Authors { get; set; } = new List<AuthorResponseVM>();
        public ICollection<CategoryResponseVM> Categories { get; set; } = new List<CategoryResponseVM>();
    }
}
