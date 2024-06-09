using System.ComponentModel.DataAnnotations;

namespace Models.People;


public abstract class Person
{
    public readonly static string INSERT = "INSERT INTO Person (Document, Name, BirthDate, AddressId, Phone, Email) VALUES (@Document, @Name, @BirthDate, @AddressId, @Phone, @Email);";

    [Key]
    public string Document { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public Address Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public Person() { }

    public Person(string document, string name, DateTime birthDate, Address address, string phone, string email)
    {
        Document = document;
        Name = name;
        BirthDate = birthDate;
        Address = address;
        Phone = phone;
        Email = email;
    }
}

