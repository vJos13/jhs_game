namespace Core.Interactions
{
    /// <summary>
    /// Contrato para entidades interactivas del entorno.
    /// Desacopla la lógica de recolección de las clases concretas.
    /// </summary>
    public interface ICollectable
    {
        void Collect();
    }
}