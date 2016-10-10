using System;

public class QrVideo
{
    public int Id;
    public string Name;
    public string Description;
    public string Url;
    public int Count;
    public int UserGroupId;
    public int UserId;
    public DateTime ReleaseDate;
    public string Password;


    public QrVideo(int id, string name, string description, string url, int count, int userGroupId, int userId, DateTime releaseDate, string password)
    {
        Id = id;
        Name = name;
        Description = description;
        Url = url;
        Count = count;
        UserGroupId = userGroupId;
        UserId = userId;
        ReleaseDate = releaseDate;
        Password = password;
    }

    public QrVideo(string name, string description, string url, int count, int userGroupId, int userId, DateTime releaseDate, string password)
    {
        Name = name;
        Description = description;
        Url = url;
        Count = count;
        UserGroupId = userGroupId;
        UserId = userId;
        ReleaseDate = releaseDate;
        Password = password;
    }
}