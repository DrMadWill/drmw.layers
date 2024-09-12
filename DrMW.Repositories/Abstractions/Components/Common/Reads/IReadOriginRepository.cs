using System.Linq.Expressions;
using DrMW.Core.Abstractions;
using DrMW.Core.Models.Abstractions;
using DrMW.Repositories.Extensions.Paging;

namespace DrMW.Repositories.Abstractions.Components.Common.Reads;

public interface IReadOriginRepository<TEntity, TPrimary> : IReadAnonymousRepository<TEntity>
    where TEntity : class, IOriginEntity<TPrimary>
{
    
    #region GetAll

    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity as a list and maps them to a list of DTOs of type TDto.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of DTOs of type TDto mapped from the entities of type TEntity.</returns>
    Task<List<TDto>> GetAllListAsync<TDto>(Expression<Func<TEntity, bool>> predicate)
        where TDto : class, IBaseDto<TPrimary>;


    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity, maps them to a list of DTOs of type TDto, and applies localization based on the specified language code.
    /// </summary>
    /// <param name="languageCode">The language code to apply for localization.</param>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A task that represents the asynchronous operation. The task result contains a localized list of DTOs of type TDto.</returns>
    Task<List<TDto>> GetAllListAsync<TDto>(Expression<Func<TEntity, bool>> predicate, string languageCode)
        where TDto : class, IBaseDto<TPrimary>;


    #endregion
   
    #region GetAllIncluding
    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity including specified related entities, and maps them to a list of DTOs of type TDto.
    /// </summary>
    /// <param name="includeProperties">Expressions indicating the related entities to include in the query.</param>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of DTOs of type TDto with specified related entities included.</returns>
    Task<List<TDto>> GetAllListIncludingAsync<TDto>(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includeProperties) where TDto : class, IBaseDto<TPrimary>;

    Task<List<TDto>> GetAllListIncludingAsync<TDto>(Expression<Func<TEntity, bool>> predicate, string languageCode,
        params Expression<Func<TEntity, object>>[] includeProperties) where TDto : class, IBaseDto<TPrimary>;
    #endregion

    #region Find
    /// <summary>
    /// Asynchronously finds an entity of type TEntity by its primary key.
    /// </summary>
    /// <param name="id">The primary key of the entity to find.</param>
    /// <returns>A ValueTask representing the asynchronous operation. The task result contains the entity found, or null if not found.</returns>
    ValueTask<TEntity> FindAsync(TPrimary id);


    /// <summary>
    /// Asynchronously finds an entity of type TEntity by its primary key and maps it to a DTO of type TDto.
    /// </summary>
    /// <param name="id">The primary key of the entity to find.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entity will be mapped.</typeparam>
    /// <returns>A ValueTask representing the asynchronous operation. The task result contains the DTO of type TDto mapped from the found entity, or null if not found.</returns>
    ValueTask<TDto> FindAsync<TDto>(TPrimary id) where TDto : class, IBaseDto<TPrimary>;

    ValueTask<TDto> FindAsync<TDto>(TPrimary id, string languageCode)
        where TDto : class, IBaseDto<TPrimary>;
    
    #endregion

    #region GetFirst

    Task<TDto?> GetFirstAsync<TDto>(Expression<Func<TEntity, bool>> predicate)
        where TDto : class, IBaseDto<TPrimary>;

    Task<TDto?> GetFirstAsync<TDto>(Expression<Func<TEntity, bool>> predicate, string languageCode)
        where TDto : class, IBaseDto<TPrimary>;

    #endregion
    
    #region Standart

    /// <summary>
    /// DTO and IQueryable Source | Asynchronously retrieves a paged result from a given IQueryable source and maps the results to DTOs of type TDto.
    /// </summary>
    /// <param name="source">The IQueryable source to paginate.</param>
    /// <param name="req">The pagination request details.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of DTOs of type TDto.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the source is null.</exception>
    Task<SourcePaged<TDto>> GetSourcePagedAsync<TDto>(IQueryable<TEntity> source, PageReq req)
        where TDto : class, IBaseDto<TPrimary>;


    /// <summary>
    /// Language Support IQueryable Source | Asynchronously retrieves a paged result from a given IQueryable source, maps the results to DTOs of type TDto, and applies localization based on the specified language code.
    /// </summary>
    /// <param name="source">The IQueryable source to paginate.</param>
    /// <param name="req">The pagination request details.</param>
    /// <param name="languageCode">The language code to apply for localization.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of localized DTOs of type TDto.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the request details are null.</exception>
    Task<SourcePaged<TDto>> GetSourcePagedAsync<TDto>(IQueryable<TEntity> source, PageReq req, string languageCode)
        where TDto : class, IBaseDto<TPrimary>;

    #endregion

    #region Expression

    /// <summary>
    /// DTO and Predicate Overload | Asynchronously retrieves a paged result for entities of type TEntity that satisfy a given predicate, includes specified related entities, and maps the results to DTOs of type TDto.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <param name="req">The pagination request details.</param>
    /// <param name="includeProperties">Expressions indicating the related entities to include in the query.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of DTOs of type TDto that satisfy the condition.</returns>
    Task<SourcePaged<TDto>> GetSourcePagedAsync<TDto>(Expression<Func<TEntity, bool>> predicate, PageReq req,
        params Expression<Func<TEntity, object>>[] includeProperties)
        where TDto : class, IBaseDto<TPrimary>;

    /// <summary>
    ///  Language and Predicate Support | Asynchronously retrieves a paged result for entities of type TEntity that satisfy a given predicate, includes specified related entities, maps the results to DTOs of type TDto, and applies localization based on the specified language code.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <param name="req">The pagination request details.</param>
    /// <param name="languageCode">The language code to apply for localization.</param>
    /// <param name="includeProperties">Expressions indicating the related entities to include in the query.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of localized DTOs of type TDto that satisfy the condition.</returns>
    Task<SourcePaged<TDto>> GetSourcePagedAsync<TDto>(Expression<Func<TEntity, bool>> predicate, PageReq req,
        string languageCode, params Expression<Func<TEntity, object>>[] includeProperties)
        where TDto : class, IBaseDto<TPrimary>;

    #endregion

    #region FuncQuery

    /// <summary>
    /// DTO and Query Search | Asynchronously retrieves a paged result using a custom query function applied to the entities of type TEntity and maps the results to DTOs of type TDto.
    /// </summary>
    /// <param name="req">The pagination request details.</param>
    /// <param name="func">A function to transform the IQueryable before pagination is applied.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of DTOs of type TDto after applying the custom query function.</returns>
    Task<SourcePaged<TDto>> GetSourcePagedFuncQueryAsync<TDto>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func, PageReq req)
        where TDto : class, IBaseDto<TPrimary>;

    /// <summary>
    /// Language, DTO, and Query Search Support | Asynchronously retrieves a paged result using a custom query function applied to the entities of type TEntity, maps the results to DTOs of type TDto, and applies localization based on the specified language code.
    /// </summary>
    /// <param name="req">The pagination request details.</param>
    /// <param name="languageCode">The language code to apply for localization.</param>
    /// <param name="func">A function to transform the IQueryable before pagination is applied.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of localized DTOs of type TDto after applying the custom query function.</returns>
    Task<SourcePaged<TDto>> GetSourcePagedFuncQueryAsync<TDto>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func, PageReq req, string languageCode)
        where TDto : class, IBaseDto<TPrimary>;

    #endregion

    #region FuncList

    /// <summary>
    /// Func DTO | Asynchronously retrieves a paged result using a custom function applied to a list of entities of type TEntity and maps the results to DTOs of type TDto.
    /// </summary>
    /// <param name="req">The pagination request details.</param>
    /// <param name="func">A function to transform the list of entities before pagination is applied.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of DTOs of type TDto after applying the custom function.</returns>
    Task<SourcePaged<TDto>> GetSourcePagedFuncListAsync<TDto>(PageReq req, Func<List<TEntity>, List<TEntity>>? func = null)
        where TDto : class, IBaseDto<TPrimary>;

    /// <summary>
    /// Func Lang DTO | Asynchronously retrieves a paged result using a custom function applied to a list of entities of type TEntity, maps the results to DTOs of type TDto, and applies localization based on the specified language code.
    /// </summary>
    /// <param name="req">The pagination request details.</param>
    /// <param name="languageCode">The language code to apply for localization.</param>
    /// <param name="func">A function to transform the list of entities before pagination is applied.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of localized DTOs of type TDto after applying the custom function.</returns>
    Task<SourcePaged<TDto>> GetSourcePagedFuncListAsync<TDto>(PageReq req, string languageCode,
        Func<List<TEntity>, List<TEntity>>? func = null) where TDto : class, IBaseDto<TPrimary>;

    #endregion
    
}