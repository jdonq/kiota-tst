using KiotaTestukas.Client;
using KiotaTestukas.Client.Models;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var authenticationProvider = new AnonymousAuthenticationProvider();
        var requestAdapter = new HttpClientRequestAdapter(authenticationProvider);
        var carClient = new CarClient(requestAdapter);
        var cars = carClient.Api.Cars;

        // GET /api/cars
        var existingCars = await cars.GetAsync();
        Console.WriteLine("Cars:");
        foreach (var car in existingCars ?? [])
        {
            Console.WriteLine($"{car.Id}: {car.Year} {car.Make} {car.Model} ({car.Color})");
        }

        // POST /api/cars
        var createdCar = await cars.PostAsync(new CreateCarRequest
        {
            Make = "Subaru",
            Model = "Outback",
            Year = 2025,
            Color = "Green"
        });

        if (createdCar?.Id is not int carId)
        {
            Console.WriteLine("The car was not created.");
            return;
        }

        Console.WriteLine($"Created car {carId}.");

        // GET /api/cars/{id}
        var fetchedCar = await cars[carId].GetAsync();
        Console.WriteLine($"Fetched: {fetchedCar?.Make} {fetchedCar?.Model}");

        // PUT /api/cars/{id}
        var updatedCar = await cars[carId].PutAsync(new UpdateCarRequest
        {
            Make = "Subaru",
            Model = "Outback",
            Year = 2025,
            Color = "Blue"
        });
        Console.WriteLine($"Updated color: {updatedCar?.Color}");

        // DELETE /api/cars/{id}
        await cars[carId].DeleteAsync();
        Console.WriteLine($"Deleted car {carId}.");
    }
}