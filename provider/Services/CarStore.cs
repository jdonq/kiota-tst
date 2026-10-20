using System.Collections.Generic;
using provider.Models;

namespace provider.Services;

/// <summary>Provides thread-safe, in-memory storage for sample car data.</summary>
public sealed class CarStore
{
    private readonly object _sync = new();
    private readonly Dictionary<int, Car> _cars = new()
    {
        [1] = new Car(1, "Toyota", "Corolla", 2022, "Blue"),
        [2] = new Car(2, "Honda", "Civic", 2023, "Red")
    };
    private int _nextId = 2;

    public Car[] GetAll()
    {
        lock (_sync)
        {
            return _cars.Values.OrderBy(car => car.Id).ToArray();
        }
    }

    public Car? GetById(int id)
    {
        lock (_sync)
        {
            return _cars.GetValueOrDefault(id);
        }
    }

    public Car Create(CreateCarRequest request)
    {
        lock (_sync)
        {
            var car = new Car(
                ++_nextId,
                request.Make,
                request.Model,
                request.Year,
                request.Color);
            _cars.Add(car.Id, car);
            return car;
        }
    }

    public Car? Update(int id, UpdateCarRequest request)
    {
        lock (_sync)
        {
            if (!_cars.ContainsKey(id))
            {
                return null;
            }

            var car = new Car(id, request.Make, request.Model, request.Year, request.Color);
            _cars[id] = car;
            return car;
        }
    }

    public bool Delete(int id)
    {
        lock (_sync)
        {
            return _cars.Remove(id);
        }
    }
}
