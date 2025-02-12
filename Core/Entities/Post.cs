using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

//[Index(nameof(Title), nameof(Contect), IsUnique = true, Name = "IX_Title_Context_Test")] // Add a unique index
public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Contect { get; set; }
    ////[ForeignKey("Blog")]
    public int BlogKey { get; set; }
    ////[ForeignKey("PostKey")]
    //public Blog Blog { get; set; }

    //public ICollection<Tag> Tags { get; set; }
    public ICollection<PostTag> PostTags { get; set; }

}

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    //public ICollection<Post> Posts { get; set; }
    public ICollection<PostTag> PostTags { get; set; }
}

public class PostTag
{
    public int PostId { get; set; }
    public Post Post { get; set; }
    public int TagId { get; set; }
    public Tag Tag { get; set; }
    public DateTime PostedAt { get; set; }

}
