namespace Autoplace.Common.Data
{
    public interface IDeleteable
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}
