namespace ProvidersPlatform.Shared.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int ProviderId { get; set; }

    public string? ImageUrl { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Commentary> Commentaries { get; set; } = new List<Commentary>();

    public virtual Provider Provider { get; set; } = null!;
}
