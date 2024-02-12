using System;

namespace SimpleClassCreator.Lib.Models
{
  public class TableQuery
    : IEquatable<TableQuery>
  {
    public string LinkedServer { get; set; }

    public string Database { get; set; }

    public string Schema { get; set; }

    /// <summary>Qualified version of the table name.</summary>
    public string Table { get; set; }

    /// <summary>Unqualified version of the table name.</summary>
    public string TableUnqualified { get; set; }

    public override bool Equals(object obj) => Equals(obj as TableQuery);


    public bool Equals(TableQuery other)
    {
      //Check if the right hand argument is null
      if (other is null)
      {
        return false;
      }

      // Optimization for a common success case. If these are the same object, then they are equal.
      if (ReferenceEquals(this, other))
      {
        return true;
      }

      // If run-time types are not exactly the same, return false.
      if (GetType() != other.GetType())
      {
        return false;
      }

      var areEqual =
        LinkedServer == other.LinkedServer &&
        Database == other.Database &&
        Table == other.Table &&
        TableUnqualified == other.TableUnqualified;

      return areEqual;
    }

    public override int GetHashCode() => 
      LinkedServer.GetHashCode() +
      Database.GetHashCode() +
      Table.GetHashCode() +
      TableUnqualified.GetHashCode();

    public static bool operator ==(TableQuery lhs, TableQuery rhs)
    {
      if (lhs is null)
      {
        if (rhs is null)
        {
          return true;
        }

        // Only the left side is null.
        return false;
      }

      // Equals handles case of null on right side.
      return lhs.Equals(rhs);
    }

    public static bool operator !=(TableQuery lhs, TableQuery rhs) => !(lhs == rhs);
  }
}
