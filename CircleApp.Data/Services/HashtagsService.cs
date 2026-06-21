using CircleApp.Data.Helpers;
using CircleApp.Data.Persistence.Entities;
using CircleApp.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleApp.Data.Services
{
    public class HashtagsService(CircleAppDbContext context) : IHashtagsService
    {
        private readonly CircleAppDbContext _context = context; 
        public async Task CreateHashtagForNewPostAsync(string content)
        {
            var postHastags = HashtagHelper.ExtractHashtags(content);
            foreach (var hashtag in postHastags)
            {
                var existingHashtag = await _context.Hashtags.FirstOrDefaultAsync(h => h.Name == hashtag);
                if (existingHashtag != null)
                {
                    existingHashtag.Count++;
                    existingHashtag.DateUpdated = DateTime.UtcNow;

                    _context.Hashtags.Update(existingHashtag);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var newHashtag = new Hashtag
                    {
                        Name = hashtag,
                        Count = 1,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow
                    };

                    await _context.Hashtags.AddAsync(newHashtag);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteHashtagForPostAsync(string content)
        {
            var hashTag = HashtagHelper.ExtractHashtags(content);
            foreach (var hashtag in hashTag)
            {
                var existingHashtag = await _context.Hashtags.FirstOrDefaultAsync(h => h.Name == hashtag);
                if (existingHashtag != null)
                {
                    existingHashtag.Count--;
                    existingHashtag.DateUpdated = DateTime.UtcNow;

                    if(existingHashtag.Count == 0)
                    {
                        _context.Hashtags.Remove(existingHashtag);
                    }
                    _context.Hashtags.Update(existingHashtag);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
