﻿namespace Url_Shorter.Entities
{
    public class ShortenUrl
    {
        public Guid Id { get; set; }
        public string LongUrl { get; set; } = string.Empty;
        public string ShortUrl { get; set; }=string.Empty;
        public string Code { get; set; }=string.Empty;

        public DateTime CreatedOnUtc { get; set; }
    }
}
