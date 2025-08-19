using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Entities.Interfaces;

namespace DataAccessLayer.Entities;

public class BaseEntity : IEntity
{
    /// <summary>
    ///     The unique identifier for the entity
    /// </summary>
    [Key]
    [Required]
    public Guid Id { get; set; }
}