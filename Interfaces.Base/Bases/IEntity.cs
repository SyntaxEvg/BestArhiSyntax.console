using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace Interfaces.Base.Base
{
    /// <summary>
    /// SOLID Функция модели, для унификации Репозиторя
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<Tid> where Tid : IComparable<Tid>, IEquatable<Tid>
    {
        Tid ID { get; set; }
    }

    //public interface IEntity<Tid> where Tid : notnull
    //{
    //    [Key]
    //    public Tid ID { get; set; }

    //    // bool Equals(Tid? other);
    //}

    //public interface IEntityGuid
    //{
    //    [Key]
    //    public Guid ID { get; set; }
    //}
    //public interface IEntityInt
    //{
    //    [Key]
    //    public int ID { get; set; }
    //}
    //public interface IEntityName
    //{
    //    /// <summary>
    //    /// имя для сущности 
    //    /// </summary>
    //    public string Name { get; set; }
    //}
}