using System.Linq.Expressions;
using AutoMapper;
using DrMW.Core.Abstractions;
using DrMW.Core.Models.Abstractions;
using DrMW.Repositories.Abstractions.Components.Common.Reads;
using DrMW.Repositories.Extensions;
using DrMW.Repositories.Extensions.Paging;
using Microsoft.EntityFrameworkCore;

namespace DrMW.Repositories.Concretes.Components.Common.Reads;

public class ReadOriginRepository<TEntity, TPrimary> :ReadAnonymousRepository<TEntity>, IReadOriginRepository<TEntity, TPrimary>
    where TEntity : class, IOriginEntity<TPrimary>
{
    protected readonly IMapper Mapper;
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOriginRepository{TEntity,TPrimary}"/> class.
    /// </summary>
    /// <param name="database">The database context to be used by the repository.</param>
    /// <param name="mapper">The AutoMapper instance for entity-DTO mappings.</param>
    public ReadOriginRepository(IReadDatabase database, IMapper mapper):base(database) 
        => Mapper = mapper;

    protected  internal ReadOriginRepository(DbContext dbContext, IMapper mapper):base(dbContext) 
        => Mapper = mapper;
    
    #region GetAll

    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity as a list and maps them to a list of DTOs of type TDto.
    /// </summary>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of DTOs of type TDto mapped from the entities of type TEntity.</returns>
    public virtual async Task<List<TDto>> GetAllListAsync<TDto>(Expression<Func<TEntity, bool>> predicate)
        where TDto : class, IBaseDto<TPrimary>
        => (await GetAllListAsync(predicate)).Select(Mapper.Map<TDto>).ToList();


    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity, maps them to a list of DTOs of type TDto, and applies localization based on the specified language code.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="languageCode">The language code to apply for localization.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A task that represents the asynchronous operation. The task result contains a localized list of DTOs of type TDto.</returns>
    public virtual async Task<List<TDto>> GetAllListAsync<TDto>(Expression<Func<TEntity, bool>> predicate,string languageCode)
        where TDto : class, IBaseDto<TPrimary>
    {
        var data = await GetAllListAsync(predicate);
        var mapDto = data.Select(Mapper.Map<TDto>).ToList();
        return LanguageHelper.GetLocalizedList<TEntity, TDto, TPrimary>(data, mapDto, languageCode).ToList();
    }
    
    #endregion

    #region GetAllIncluding

    /// <summary>
    /// Asynchronously retrieves all entities of type TEntity including specified related entities, and maps them to a list of DTOs of type TDto.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="includeProperties">Expressions indicating the related entities to include in the query.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of DTOs of type TDto with specified related entities included.</returns>
    public async Task<List<TDto>> GetAllListIncludingAsync<TDto>(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includeProperties) where TDto : class, IBaseDto<TPrimary>
        => (await GetAllListIncludingAsync(predicate,includeProperties)).Select(Mapper.Map<TDto>).ToList();

    public virtual async Task<List<TDto>> GetAllListIncludingAsync<TDto>(Expression<Func<TEntity, bool>> predicate,string languageCode,
        params Expression<Func<TEntity, object>>[] includeProperties) where TDto : class, IBaseDto<TPrimary>
    {
        var data = await GetAllListIncludingAsync(predicate,includeProperties);
        return LanguageHelper.GetLocalizedList<TEntity, TDto, TPrimary>(data, data.Select(Mapper.Map<TDto>).ToList(), languageCode)
            .ToList();
    }
    
    #endregion

    #region Find
    /// <summary>
    /// Asynchronously finds an entity of type TEntity by its primary key.
    /// </summary>
    /// <param name="id">The primary key of the entity to find.</param>
    /// <returns>A ValueTask representing the asynchronous operation. The task result contains the entity found, or null if not found.</returns>
    public virtual async ValueTask<TEntity> FindAsync(TPrimary id)
        => await Table.FindAsync(id);


    /// <summary>
    /// Asynchronously finds an entity of type TEntity by its primary key and maps it to a DTO of type TDto.
    /// </summary>
    /// <param name="id">The primary key of the entity to find.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entity will be mapped.</typeparam>
    /// <returns>A ValueTask representing the asynchronous operation. The task result contains the DTO of type TDto mapped from the found entity, or null if not found.</returns>
    public virtual async ValueTask<TDto> FindAsync<TDto>(TPrimary id) where TDto : class, IBaseDto<TPrimary>
        => Mapper.Map<TDto>(await FindAsync(id));


    /// <summary>
    /// Asynchronously finds an entity of type TEntity by its primary key, maps it to a DTO of type TDto, and applies localization based on the specified language code.
    /// </summary>
    /// <param name="languageCode">The language code to apply for localization.</param>
    /// <param name="id">The primary key of the entity to find.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entity will be mapped.</typeparam>
    /// <returns>A ValueTask representing the asynchronous operation. The task result contains the localized DTO of type TDto.</returns>
    public virtual async ValueTask<TDto> FindAsync<TDto> (TPrimary id,string languageCode)
        where TDto : class, IBaseDto<TPrimary>
    {
        var data = await FindAsync(id);
        return LanguageHelper.GetLocalized(data, Mapper.Map<TDto>(data), languageCode);
    }
    #endregion
    
    #region GetFirst


    /// <summary>
    /// Asynchronously retrieves the first entity of type TEntity that satisfies a specified condition and maps it to a DTO of type TDto.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entity will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains the DTO of type TDto mapped from the first entity that satisfies the condition, or null if no such entity is found.</returns>
    public async Task<TDto?> GetFirstAsync<TDto>(Expression<Func<TEntity, bool>> predicate)
        where TDto : class, IBaseDto<TPrimary>
    {
        var data = await GetFirstAsync(predicate);
        return data == null ? null : Mapper.Map<TDto>(data);
    }


    /// <summary>
    /// Asynchronously retrieves the first entity of type TEntity that satisfies a specified condition, maps it to a DTO of type TDto, and applies localization based on the specified language code.
    /// </summary>
    /// <param name="languageCode">The language code to apply for localization.</param>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entity will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains the localized DTO of type TDto.</returns>
    public virtual async Task<TDto?> GetFirstAsync<TDto>(Expression<Func<TEntity, bool>> predicate,string languageCode)
        where TDto : class, IBaseDto<TPrimary>
    {
        var data = await GetFirstAsync(predicate);
        return data == null ? null : Mapper.Map<TDto>(data);
    }

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
    public virtual async Task<SourcePaged<TDto>> GetSourcePagedAsync<TDto>(IQueryable<TEntity> source, PageReq req)
        where TDto : class, IBaseDto<TPrimary>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        req.Page = req.Page == 0 ? 1 : req.Page;

        if (req.PerPage > 0 && req.PerPage <= 200)
            Paginate<TEntity>.PerPage = req.PerPage;

        return new SourcePaged<TDto>
        {
            PagingModel = new PageModel(await source.CountAsync(), req.Page, Paginate<TEntity>.PerPage),
            Source = (await Paginate<TEntity>.Paging(source, req.Page).ToListAsync()).Select(Mapper.Map<TDto>).ToList(),
        };
    }


    /// <summary>
    /// Language Support IQueryable Source | Asynchronously retrieves a paged result from a given IQueryable source, maps the results to DTOs of type TDto, and applies localization based on the specified language code.
    /// </summary>
    /// <param name="source">The IQueryable source to paginate.</param>
    /// <param name="req">The pagination request details.</param>
    /// <param name="languageCode">The language code to apply for localization.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of localized DTOs of type TDto.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the request details are null.</exception>
    public virtual async Task<SourcePaged<TDto>> GetSourcePagedAsync<TDto>(IQueryable<TEntity> source, PageReq req, string languageCode)
        where TDto : class, IBaseDto<TPrimary>
    {
        if (req == null) throw new ArgumentNullException(nameof(req) + " is null");
        req.Page = req.Page == 0 ? 1 : req.Page;

        if (req.PerPage > 0 && req.PerPage <= 200)
            Paginate<TEntity>.PerPage = req.PerPage;

        var data = await Paginate<TEntity>.Paging(source, req.Page).ToListAsync();
        return new SourcePaged<TDto>
        {
            PagingModel = new PageModel(await source.CountAsync(), req.Page, Paginate<TEntity>.PerPage),
            Source = LanguageHelper.GetLocalizedList<TEntity, TDto, TPrimary>(data, data.Select(Mapper.Map<TDto>).ToList(), languageCode)
        };
    }

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
    public virtual async Task<SourcePaged<TDto>> GetSourcePagedAsync<TDto>(Expression<Func<TEntity, bool>> predicate,
        PageReq req, params Expression<Func<TEntity, object>>[] includeProperties)
        where TDto : class, IBaseDto<TPrimary>
        => await GetSourcePagedAsync<TDto>(IncludingQueryable(includeProperties).Where(predicate), req);

    /// <summary>
    ///  Language and Predicate Support | Asynchronously retrieves a paged result for entities of type TEntity that satisfy a given predicate, includes specified related entities, maps the results to DTOs of type TDto, and applies localization based on the specified language code.
    /// </summary>
    /// <param name="predicate">An expression to test each entity for a condition.</param>
    /// <param name="req">The pagination request details.</param>
    /// <param name="languageCode">The language code to apply for localization.</param>
    /// <param name="includeProperties">Expressions indicating the related entities to include in the query.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of localized DTOs of type TDto that satisfy the condition.</returns>
    public virtual async Task<SourcePaged<TDto>> GetSourcePagedAsync<TDto>(Expression<Func<TEntity, bool>> predicate,
        PageReq req, string languageCode, params Expression<Func<TEntity, object>>[] includeProperties)
        where TDto : class, IBaseDto<TPrimary>
        => await GetSourcePagedAsync<TDto>(IncludingQueryable(includeProperties).Where(predicate), req,
            languageCode);


    #endregion

    #region FuncQuery

    /// <summary>
    /// DTO and Query Search | Asynchronously retrieves a paged result using a custom query function applied to the entities of type TEntity and maps the results to DTOs of type TDto.
    /// </summary>
    /// <param name="req">The pagination request details.</param>
    /// <param name="func">A function to transform the IQueryable before pagination is applied.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of DTOs of type TDto after applying the custom query function.</returns>
    public virtual async Task<SourcePaged<TDto>> GetSourcePagedFuncQueryAsync<TDto>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func, PageReq req)
        where TDto : class, IBaseDto<TPrimary>
        => await GetSourcePagedAsync<TDto>( func(Queryable()), req);

    /// <summary>
    /// Language, DTO, and Query Search Support | Asynchronously retrieves a paged result using a custom query function applied to the entities of type TEntity, maps the results to DTOs of type TDto, and applies localization based on the specified language code.
    /// </summary>
    /// <param name="req">The pagination request details.</param>
    /// <param name="languageCode">The language code to apply for localization.</param>
    /// <param name="func">A function to transform the IQueryable before pagination is applied.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of localized DTOs of type TDto after applying the custom query function.</returns>
    public virtual async Task<SourcePaged<TDto>> GetSourcePagedFuncQueryAsync<TDto>(Func<IQueryable<TEntity>, IQueryable<TEntity>> func, PageReq req, string languageCode)
        where TDto : class, IBaseDto<TPrimary>
        => await GetSourcePagedAsync<TDto>( func(Queryable()), req,
            languageCode);

    #endregion
    
    #region FuncList
    
    /// <summary>
    /// Func DTO | Asynchronously retrieves a paged result using a custom function applied to a list of entities of type TEntity and maps the results to DTOs of type TDto.
    /// </summary>
    /// <param name="req">The pagination request details.</param>
    /// <param name="func">A function to transform the list of entities before pagination is applied.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of DTOs of type TDto after applying the custom function.</returns>
    public virtual async Task<SourcePaged<TDto>> GetSourcePagedFuncListAsync<TDto>(PageReq req,
        Func<List<TEntity>, List<TEntity>>? func = null) where TDto : class, IBaseDto<TPrimary>
    {
        var source = Queryable();
        req.Page = req.Page == 0 ? 1 : req.Page;

        if (req.PerPage > 0 && req.PerPage <= 200)
            Paginate<TEntity>.PerPage = req.PerPage;

        return new SourcePaged<TDto>
        {
            PagingModel = new PageModel(await source.CountAsync(), req.Page, Paginate<TEntity>.PerPage),
            Source = func == null
                ? (await Paginate<TEntity>.Paging(source, req.Page).ToListAsync()).Select(Mapper.Map<TDto>).ToList()
                : func((await Paginate<TEntity>.Paging(source, req.Page).ToListAsync())).Select(Mapper.Map<TDto>)
                    .ToList(),
        };
    }

    /// <summary>
    /// Func Lang DTO | Asynchronously retrieves a paged result using a custom function applied to a list of entities of type TEntity, maps the results to DTOs of type TDto, and applies localization based on the specified language code.
    /// </summary>
    /// <param name="req">The pagination request details.</param>
    /// <param name="languageCode">The language code to apply for localization.</param>
    /// <param name="func">A function to transform the list of entities before pagination is applied.</param>
    /// <typeparam name="TDto">The type of data transfer object to which the entities will be mapped.</typeparam>
    /// <returns>A Task representing the asynchronous operation. The task result contains a paged result of localized DTOs of type TDto after applying the custom function.</returns>
    public virtual async Task<SourcePaged<TDto>> GetSourcePagedFuncListAsync<TDto>(PageReq req, string languageCode,
        Func<List<TEntity>, List<TEntity>>? func = null) where TDto : class, IBaseDto<TPrimary>
    {
        var source = Queryable();
        req.Page = req.Page == 0 ? 1 : req.Page;

        if (req.PerPage > 0 && req.PerPage <= 200)
            Paginate<TEntity>.PerPage = req.PerPage;

        var data = func == null
            ? (await Paginate<TEntity>.Paging(source, req.Page).ToListAsync())
            : func((await Paginate<TEntity>.Paging(source, req.Page).ToListAsync()));

        return new SourcePaged<TDto>
        {
            PagingModel = new PageModel(await source.CountAsync(), req.Page, Paginate<TEntity>.PerPage),
            Source = LanguageHelper.GetLocalizedList<TEntity, TDto, TPrimary>(data,
                data.Select(Mapper.Map<TDto>).ToList(), languageCode)
        };
    }
    
    #endregion
    
    /// <summary>
    /// Destructor for ReadOriginRepository.
    /// </summary>
    /// 
    ~ReadOriginRepository()
    {
        Dispose(false);
    }
}