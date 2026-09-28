using BookShop.Application.ViewModels.Author;
using BookShop.Application.ViewModels.Book;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.ViewModels.Common
{
    public class HomePageResponseVM
    {
        public ICollection<BookResponseVM> NewArrivalBooks { get; set; } = new List<BookResponseVM>();
        public ICollection<AuthorResponseVM> Authors { get; set; } = new List<AuthorResponseVM>();
    }
}
