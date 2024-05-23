namespace Exam.Core.Persistence.Models.Entities;

public interface IEntityBase<T> : IEntityBase
{
    public T Id { get; set; }
}

public interface IEntityBase
{
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string LastUpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public abstract class EntityBase<T> : EntityBase, IEntityBase<T>
{
    public T Id { get; set; }
}

public abstract class EntityBase : IEntityBase
{
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string LastUpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}