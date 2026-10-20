namespace provider.Models;

/// <summary>Represents a car managed by the sample API.</summary>
public sealed record Car(int Id, string Make, string Model, int Year, string Color);

/// <summary>Payload for creating a car.</summary>
public sealed record CreateCarRequest(string Make, string Model, int Year, string Color);

/// <summary>Payload for replacing a car's details.</summary>
public sealed record UpdateCarRequest(string Make, string Model, int Year, string Color);
