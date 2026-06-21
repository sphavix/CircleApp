using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleApp.Data.Services
{
    public interface IHashtagsService
    {
        Task CreateHashtagForNewPostAsync(string content);
        Task DeleteHashtagForPostAsync(string content);
    }
}
