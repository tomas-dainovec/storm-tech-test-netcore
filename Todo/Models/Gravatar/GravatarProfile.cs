using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Models.Gravatar
{
    public class GravatarProfile
    {
        public string Id { get; set; }
        public string Hash { get; set; }
        public string RequestedHash { get; set; }
        public string ProfileUrl { get; set; }
        public string PreferredUsername { get; set; }
        public string ThumbnailUrl { get; set; }
        public IEnumerable<PhotosEntry> Photos { get; set; }
        public NameEntry Name { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<UrlEntry> Urls { get; set; }
    }

    public class PhotosEntry
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }

    public class NameEntry
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Formatted { get; set; }
    }

    public class UrlEntry
    {
        public string Value { get; set; }
        public string Title { get; set; }
    }
}
