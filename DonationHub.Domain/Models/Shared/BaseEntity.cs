namespace DonationHub.Domain.Models.Shared
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        public DateTime CreatedAt { get; protected set; }

        //protected BaseEntity(Guid id, DateTime createdAt)
        //{
        //    Id = id;
        //    CreatedAt = createdAt;
        //}


        public override bool Equals(object? obj)
        {
            if (obj is null) 
                return false;
            if (!(obj is BaseEntity entity))
                return false;
            return Id == entity.Id;

        }

        public override int GetHashCode() => Id.GetHashCode();
        public static bool operator==(BaseEntity? firstEntity, BaseEntity? secondEntity)
        {
            if(firstEntity is null && secondEntity is null)
                return true;
            if (firstEntity is null) 
                return  false;
            return firstEntity.Equals(secondEntity);

        }
        public static bool operator !=(BaseEntity? firstEntity, BaseEntity? secondEntity)
        {

            return !(firstEntity == secondEntity);

        }


    }
}
