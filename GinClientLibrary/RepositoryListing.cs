using System;

namespace GinClientLibrary
{
    /// <summary>
    ///     Represents the output of gin repos --json
    /// </summary>
    public class Owner
    {
        public int id { get; set; }
        public string login { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string avatar_url { get; set; }
        public string username { get; set; }
    }

    public class Permissions
    {
        public bool admin { get; set; }
        public bool push { get; set; }
        public bool pull { get; set; }
    }

    public class RepositoryListing
    {
        public int id { get; set; }
        public Owner owner { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        public string description { get; set; }
        public bool @private { get; set; }
        public bool fork { get; set; }
        public object parent { get; set; }
        public bool empty { get; set; }
        public bool mirror { get; set; }
        public int size { get; set; }
        public string html_url { get; set; }
        public string ssh_url { get; set; }
        public string clone_url { get; set; }
        public string website { get; set; }
        public int stars_count { get; set; }
        public int forks_count { get; set; }
        public int watchers_count { get; set; }
        public int open_issues_count { get; set; }
        public string default_branch { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public Permissions permissions { get; set; }
    }
}