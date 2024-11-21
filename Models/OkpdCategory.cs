using System.ComponentModel.DataAnnotations.Schema;

namespace SoftWizard.Models;

/// <summary> Class для хранения данных Классификатор кодов окпд2 (2024) </summary>
[Table("OkpdCategories")]
public class OkpdCategory
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Idx { get; set; }
    public required string Razdel { get; set; }
    public required string Kod { get; set; }
    public string? Nomdescr { get; set; }
    public required long GlobalId { get; set; }

    /// <summary>
    /// Уровень вложенности 7 возможных значения: 1 - корневые категории, 2 - дочки корневых, 3 - дочки уровня 2 и т.д.
    /// </summary>
    public int? Level { get; set; }

    /// <summary>
    /// Id родителя записи
    /// </summary>
    public int? ParentId { get; set; }

    public int GetLevel() => Idx.TrimEnd('.').Split('.').Length;
    public string GetParentId() => string.Join('.', Idx.Split('.')[0..^1]);

    [NotMapped]
    public required List<int> ParentIds { get; set; }
}