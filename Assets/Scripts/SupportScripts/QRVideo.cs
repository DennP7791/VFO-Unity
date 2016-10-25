using System;

public class QrVideo
{
    public int Id;
    public string Name;
    public string Description;
    public string Path;
    public int Count;
    public int UserGroupId;
    public int UserId;
    public DateTime ReleaseDate;
    public int VideoCategoryId;


    public QrVideo(int id, string name, string description, string path, int count, int userGroupId, int userId, DateTime releaseDate, int videoCategoryId)
    {
        Id = id;
        Name = name;
        Description = description;
        Path = path;
        Count = count;
        UserGroupId = userGroupId;
        UserId = userId;
        ReleaseDate = releaseDate;
        VideoCategoryId = videoCategoryId;
    }

    public QrVideo(string name, string description, string path, int count, int userGroupId, int userId, DateTime releaseDate, int videoCategoryId)
    {
        Name = name;
        Description = description;
        Path = path;
        Count = count;
        UserGroupId = userGroupId;
        UserId = userId;
        ReleaseDate = releaseDate;
        VideoCategoryId = videoCategoryId;
    }
}