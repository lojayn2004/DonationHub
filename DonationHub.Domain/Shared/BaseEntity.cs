namespace DonationHub.Domain.Shared
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }

        public DateTime CreatedAt { get; private set; }

        //protected BaseEntity(Guid id, DateTime createdAt)
        //{
        //    Id = id;
        //    CreatedAt = createdAt;
        //}


        public override bool Equals(object? obj)
        {
            if (obj == null) 
                return false;
            if (!(obj is BaseEntity entity))
                return false;
            return Id == entity.Id;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator==(BaseEntity firstEntity, BaseEntity secondEntity)
        {
            if (firstEntity == null) 
                return  false;
            return firstEntity.Equals(secondEntity);

        }
        public static bool operator !=(BaseEntity firstEntity, BaseEntity secondEntity)
        {

            return !(firstEntity == secondEntity);

        }


    }
}
