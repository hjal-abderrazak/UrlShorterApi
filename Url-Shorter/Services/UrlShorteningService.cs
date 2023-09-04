using Microsoft.EntityFrameworkCore;

namespace Url_Shorter.Services
{
    public class UrlShorteningService
    {
        public const int NumberOfCharsInShortLink = 8;
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private  readonly Random _random = new Random();
        private readonly ApplicationDbContext _context;

        public UrlShorteningService(ApplicationDbContext context)
        {
            _context = context;
        }
        public  async Task<string> GenerateUniqueCode()
        {
          var codeChars = new char[NumberOfCharsInShortLink];
                
          while(true)
            {
                for (int i = 0; i < NumberOfCharsInShortLink; i++)
                {
                    var randomIndex = _random.Next(Alphabet.Length - 1);
                    codeChars[i] = Alphabet[randomIndex];
                }

                var code = new string(codeChars);

                if (! await _context.ShortenUrls.AnyAsync(s => s.Code == code))
                {
                    return code;
                }
            }
        }



    }
}
