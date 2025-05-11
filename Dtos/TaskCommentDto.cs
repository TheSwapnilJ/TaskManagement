public class TaskCommentDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; }
    public int TaskId { get; set; }  // This needs to be here
}
