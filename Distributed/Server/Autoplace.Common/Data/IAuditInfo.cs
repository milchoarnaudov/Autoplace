namespace Autoplace.Common.Data
{
    public interface IAuditInfo
    {
        DateTime CreatedOn { get; set; }

        DateTime? UpdatedOn { get; set; }
    }
}
