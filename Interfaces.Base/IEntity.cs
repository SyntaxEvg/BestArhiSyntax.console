namespace Interfaces.Base
{
    public interface IEntity<T>
    {
        public T ID { get; set; }
    }
    public interface IEntityName
    {
        /// <summary>
        /// имя для сущности 
        /// </summary>
        public string Name { get; set; }
    }
}