using System;

public class QrVideoUserView
{
    public Guid VideoId;
    public int UserId;
    public DateTime ViewDate;

    public QrVideoUserView(Guid videoId, int userId, DateTime viewDate)
    {
        VideoId = videoId;
        UserId = userId;
        ViewDate = viewDate;
    }
}
