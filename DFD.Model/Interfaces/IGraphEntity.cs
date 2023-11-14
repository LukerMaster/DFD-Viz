namespace DFD.Model.Interfaces;

public interface IGraphEntity
{
    IGraphEntity Parent { get; set; }
    ICollection<IGraphEntity> Children { get; set; }
    public string EntityName { get; }
}