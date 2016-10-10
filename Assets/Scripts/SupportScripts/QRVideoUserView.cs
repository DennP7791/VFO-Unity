using System;

public class QrVideoUserView
{
    public int VideoId;
    public int UserId;
    public DateTime ViewDate;

    public QrVideoUserView(int videoId, int userId, DateTime viewDate)
    {
        VideoId = videoId;
        UserId = userId;
        ViewDate = viewDate;
    }
}
