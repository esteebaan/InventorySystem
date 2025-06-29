using InventorySystem.DataAccess;
using InventorySystem.Entities.Entities;

namespace InventorySystem.Business.Services
{
    /// <summary>
    /// Implementación de lógica de negocio para artículos.
    /// </summary>
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _uow;

        public ArticleService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            return await _uow.Articles.GetAllAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(Guid id)
        {
            return await _uow.Articles.GetByIdAsync(id);
        }

        public async Task<Guid> CreateArticleAsync(Article article)
        {
            var all = await _uow.Articles.GetAllAsync();
            if (all.Any(a => a.Code == article.Code))
                throw new InvalidOperationException("Article code must be unique.");

            await _uow.Articles.AddAsync(article);
            await _uow.CompleteAsync();
            return article.ArticleId;
        }

        public async Task UpdateArticleAsync(Article article)
        {
            _uow.Articles.Update(article);
            await _uow.CompleteAsync();
        }

        public async Task DeleteArticleAsync(Guid id)
        {
            var existing = await _uow.Articles.GetByIdAsync(id);
            if (existing != null)
            {
                _uow.Articles.Remove(existing);
                await _uow.CompleteAsync();
            }
        }
    }
}
