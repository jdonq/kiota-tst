using Microsoft.AspNetCore.Http.HttpResults;
using provider.Models;
using provider.Services;

namespace provider.Endpoints;

/// <summary>Maps CRUD endpoints for cars.</summary>
public static class CarEndpoints
{
    public static WebApplication MapCarEndpoints(this WebApplication app)
    {
        var cars = app.MapGroup("/api/cars").WithTags("Cars");

        cars.MapGet("/", (CarStore store, CancellationToken cancellationToken) =>
            TypedResults.Ok(store.GetAll()))
            .WithName("GetCars")
            .WithSummary("List all cars")
            .WithDescription("Returns all cars currently held in the in-memory sample store.")
            .Produces<Car[]>(StatusCodes.Status200OK);

        cars.MapGet("/{id:int}", Results<Ok<Car>, NotFound> (int id, CarStore store, CancellationToken cancellationToken) =>
            store.GetById(id) is { } car
                ? TypedResults.Ok(car)
                : TypedResults.NotFound())
            .WithName("GetCarById")
            .WithSummary("Get a car by ID")
            .WithDescription("Returns a car, or 404 when the ID does not exist.")
            .Produces<Car>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        cars.MapPost("/", (CreateCarRequest request, CarStore store, CancellationToken cancellationToken) =>
        {
            var car = store.Create(request);
            return TypedResults.Created($"/api/cars/{car.Id}", car);
        })
            .WithName("CreateCar")
            .WithSummary("Create a car")
            .WithDescription("Adds a car to the in-memory sample store.")
            .Produces<Car>(StatusCodes.Status201Created);

        cars.MapPut("/{id:int}", Results<Ok<Car>, NotFound> (int id, UpdateCarRequest request, CarStore store, CancellationToken cancellationToken) =>
            store.Update(id, request) is { } car
                ? TypedResults.Ok(car)
                : TypedResults.NotFound())
            .WithName("UpdateCar")
            .WithSummary("Replace a car")
            .WithDescription("Replaces all car details for the given ID.")
            .Produces<Car>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        cars.MapDelete("/{id:int}", Results<NoContent, NotFound> (int id, CarStore store, CancellationToken cancellationToken) =>
            store.Delete(id)
                ? TypedResults.NoContent()
                : TypedResults.NotFound())
            .WithName("DeleteCar")
            .WithSummary("Delete a car")
            .WithDescription("Removes the car with the given ID from the in-memory sample store.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
