using InventorySystem.Entities.Entities;

namespace InventorySystem.Business.Services
{
    /// <summary>
    /// Interface para la lógica de artículos.
    /// </summary>
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<Article?> GetArticleByIdAsync(Guid id);
        Task<Guid> CreateArticleAsync(Article article);
        Task UpdateArticleAsync(Article article);
        Task DeleteArticleAsync(Guid id);

    }
}
