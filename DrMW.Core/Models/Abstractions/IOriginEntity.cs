namespace DrMW.Core.Models.Abstractions;

public interface IOriginEntity<T>
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    T Id { get; set; }
}