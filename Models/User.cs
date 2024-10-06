using System;
using System.Dynamic;

namespace TaskManager.Models;

public class User : IDomainObject
{
    public string FirstName { get; init; }

    public int Id { get; set; }

    public string LastName { get; init; }

    public User(string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(firstName))
        {
            throw new ArgumentException(nameof(firstName));
        }
        if (string.IsNullOrEmpty(lastName))
        {
            throw new ArgumentException(nameof(lastName));
        }

        FirstName = firstName;
        LastName = lastName;
    }

    public override bool Equals(object? obj)
    {
        if (obj is User user)
        {
            return this.Id == user.Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"{LastName} {FirstName[0]}.";
    }
}
