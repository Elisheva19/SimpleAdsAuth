using System;

namespace SimpleAdsLogIns.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PwHash { get; set; }
        public string Name { get; set; }
    }
}
public class Ad
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public string Text { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }

    public bool Delete { get; set; }
}
