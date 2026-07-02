using System;

namespace VeteranLogistics.Data.Entities.Base;

/// <summary>
/// Base entity with common properties shared across all entities.
/// Keep this class minimal: no audit, tenant or soft-delete properties here.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Record creation timestamp (UTC recommended).
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Last modification timestamp (nullable when not modified yet).
    /// </summary>
    public DateTime? ModifiedOn { get; set; }
}
