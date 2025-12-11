namespace t2.Domain.Exceptions
{
    public class NotFoundException : DomainException
    {
        public string EntityName { get; }
        public object EntityId { get; }

        public NotFoundException(string entityName, object entityId)
          : base($"{entityName} con id {entityId} no encontrada.")
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}

