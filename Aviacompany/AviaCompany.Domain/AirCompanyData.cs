namespace AviaCompany.Domain;

public static class TestDataGenerator
{
    public static List<AircraftFamily> GenerateAircraftFamilies()
    {
        return new List<AircraftFamily>
        {
            new AircraftFamily { Id = 1, Name = "A320", Manufacturer = "Airbus" },
            new AircraftFamily { Id = 2, Name = "737", Manufacturer = "Boeing" },
            new AircraftFamily { Id = 3, Name = "A350", Manufacturer = "Airbus" },
            new AircraftFamily { Id = 4, Name = "777", Manufacturer = "Boeing" },
            new AircraftFamily { Id = 5, Name = "CRJ", Manufacturer = "Bombardier" },
            new AircraftFamily { Id = 6, Name = "E-Jet", Manufacturer = "Embraer" },
            new AircraftFamily { Id = 7, Name = "A380", Manufacturer = "Airbus" },
            new AircraftFamily { Id = 8, Name = "787", Manufacturer = "Boeing" },
            new AircraftFamily { Id = 9, Name = "SSJ", Manufacturer = "Sukhoi" },
            new AircraftFamily { Id = 10, Name = "MC-21", Manufacturer = "Irkut" }
        };
    }

    public static List<AircraftModel> GenerateAircraftModels(List<AircraftFamily> families)
    {
        var family1 = families[0];
        var family2 = families[1];
        var family3 = families[2];
        var family4 = families[3];
        var family5 = families[4];
        var family6 = families[5];
        var family7 = families[6];
        var family8 = families[7];
        var family9 = families[8];
        var family10 = families[9];

        return new List<AircraftModel>
        {
            new AircraftModel { Id = 1, Name = "A320-200", FlightRange = 6100, PassengerCapacity = 180, CargoCapacity = 3.5, AircraftFamilyId = family1.Id, AircraftFamily = family1 },
            new AircraftModel { Id = 2, Name = "A320neo", FlightRange = 6300, PassengerCapacity = 195, CargoCapacity = 3.8, AircraftFamilyId = family1.Id, AircraftFamily = family1 },
            new AircraftModel { Id = 3, Name = "737-800", FlightRange = 5765, PassengerCapacity = 189, CargoCapacity = 3.4, AircraftFamilyId = family2.Id, AircraftFamily = family2 },
            new AircraftModel { Id = 4, Name = "737 MAX 8", FlightRange = 6570, PassengerCapacity = 210, CargoCapacity = 3.7, AircraftFamilyId = family2.Id, AircraftFamily = family2 },
            new AircraftModel { Id = 5, Name = "A350-900", FlightRange = 15000, PassengerCapacity = 440, CargoCapacity = 12.5, AircraftFamilyId = family3.Id, AircraftFamily = family3 },
            new AircraftModel { Id = 6, Name = "777-300ER", FlightRange = 13650, PassengerCapacity = 550, CargoCapacity = 14.5, AircraftFamilyId = family4.Id, AircraftFamily = family4 },
            new AircraftModel { Id = 7, Name = "CRJ-900", FlightRange = 2845, PassengerCapacity = 90, CargoCapacity = 1.8, AircraftFamilyId = family5.Id, AircraftFamily = family5 },
            new AircraftModel { Id = 8, Name = "E195", FlightRange = 4260, PassengerCapacity = 146, CargoCapacity = 2.5, AircraftFamilyId = family6.Id, AircraftFamily = family6 },
            new AircraftModel { Id = 9, Name = "A380-800", FlightRange = 15200, PassengerCapacity = 853, CargoCapacity = 18.0, AircraftFamilyId = family7.Id, AircraftFamily = family7 },
            new AircraftModel { Id = 10, Name = "787-9", FlightRange = 14140, PassengerCapacity = 420, CargoCapacity = 11.0, AircraftFamilyId = family8.Id, AircraftFamily = family8 }
        };
    }

    public static List<Flight> GenerateFlights(List<AircraftModel> models)
    {
        var model1 = models[0];
        var model2 = models[1];
        var model3 = models[2];
        var model4 = models[3];
        var model5 = models[4];
        var model6 = models[5];
        var model7 = models[6];
        var model8 = models[7];
        var model9 = models[8];
        var model10 = models[9];

        return new List<Flight>
        {
            new Flight { Id = 1, Code = "SU1001", DeparturePoint = "Moscow", ArrivalPoint = "St. Petersburg", DepartureDate = new DateTime(2024, 1, 15), ArrivalDate = new DateTime(2024, 1, 15), DepartureTime = new TimeSpan(8, 0, 0), Duration = new TimeSpan(1, 30, 0), AircraftModelId = model1.Id, AircraftModel = model1 },
            new Flight { Id = 2, Code = "SU1002", DeparturePoint = "Moscow", ArrivalPoint = "St. Petersburg", DepartureDate = new DateTime(2024, 1, 15), ArrivalDate = new DateTime(2024, 1, 15), DepartureTime = new TimeSpan(14, 0, 0), Duration = new TimeSpan(1, 25, 0), AircraftModelId = model2.Id, AircraftModel = model2 },
            new Flight { Id = 3, Code = "SU2001", DeparturePoint = "Moscow", ArrivalPoint = "Sochi", DepartureDate = new DateTime(2024, 1, 16), ArrivalDate = new DateTime(2024, 1, 16), DepartureTime = new TimeSpan(9, 30, 0), Duration = new TimeSpan(2, 30, 0), AircraftModelId = model3.Id, AircraftModel = model3 },
            new Flight { Id = 4, Code = "SU3001", DeparturePoint = "Moscow", ArrivalPoint = "New York", DepartureDate = new DateTime(2024, 1, 17), ArrivalDate = new DateTime(2024, 1, 17), DepartureTime = new TimeSpan(12, 0, 0), Duration = new TimeSpan(10, 45, 0), AircraftModelId = model4.Id, AircraftModel = model4 },
            new Flight { Id = 5, Code = "SU4001", DeparturePoint = "Moscow", ArrivalPoint = "Tokyo", DepartureDate = new DateTime(2024, 1, 18), ArrivalDate = new DateTime(2024, 1, 18), DepartureTime = new TimeSpan(15, 0, 0), Duration = new TimeSpan(9, 30, 0), AircraftModelId = model5.Id, AircraftModel = model5 },
            
            new Flight { Id = 6, Code = "SU5001", DeparturePoint = "St. Petersburg", ArrivalPoint = "Helsinki", DepartureDate = new DateTime(2024, 1, 19), ArrivalDate = new DateTime(2024, 1, 19), DepartureTime = new TimeSpan(10, 0, 0), Duration = new TimeSpan(1, 0, 0), AircraftModelId = model6.Id, AircraftModel = model6 },
            new Flight { Id = 7, Code = "SU5002", DeparturePoint = "St. Petersburg", ArrivalPoint = "Helsinki", DepartureDate = new DateTime(2024, 1, 19), ArrivalDate = new DateTime(2024, 1, 19), DepartureTime = new TimeSpan(16, 0, 0), Duration = new TimeSpan(1, 5, 0), AircraftModelId = model7.Id, AircraftModel = model7 },
            
            new Flight { Id = 8, Code = "SU6001", DeparturePoint = "Sochi", ArrivalPoint = "Moscow", DepartureDate = new DateTime(2024, 1, 20), ArrivalDate = new DateTime(2024, 1, 20), DepartureTime = new TimeSpan(18, 0, 0), Duration = new TimeSpan(2, 25, 0), AircraftModelId = model8.Id, AircraftModel = model8 },
            new Flight { Id = 9, Code = "SU7001", DeparturePoint = "New York", ArrivalPoint = "Moscow", DepartureDate = new DateTime(2024, 1, 21), ArrivalDate = new DateTime(2024, 1, 21), DepartureTime = new TimeSpan(20, 0, 0), Duration = new TimeSpan(10, 30, 0), AircraftModelId = model9.Id, AircraftModel = model9 },
            new Flight { Id = 10, Code = "SU8001", DeparturePoint = "Tokyo", ArrivalPoint = "Moscow", DepartureDate = new DateTime(2024, 1, 22), ArrivalDate = new DateTime(2024, 1, 22), DepartureTime = new TimeSpan(22, 0, 0), Duration = new TimeSpan(9, 45, 0), AircraftModelId = model10.Id, AircraftModel = model10 }
        };
    }

    public static List<Passenger> GeneratePassengers()
    {
        return new List<Passenger>
        {
            new Passenger { Id = 1, PassportNumber = "AB123456", FullName = "Ivanov Ivan Ivanovich", BirthDate = new DateTime(1985, 5, 15) },
            new Passenger { Id = 2, PassportNumber = "CD654321", FullName = "Petrov Petr Petrovich", BirthDate = new DateTime(1990, 8, 20) },
            new Passenger { Id = 3, PassportNumber = "EF789012", FullName = "Sidorova Anna Sergeevna", BirthDate = new DateTime(1988, 3, 10) },
            new Passenger { Id = 4, PassportNumber = "GH345678", FullName = "Kozlov Dmitry Viktorovich", BirthDate = new DateTime(1992, 11, 5) },
            new Passenger { Id = 5, PassportNumber = "IJ901234", FullName = "Nikolaeva Elena Olegovna", BirthDate = new DateTime(1987, 7, 25) },
            new Passenger { Id = 6, PassportNumber = "KL567890", FullName = "Morozov Andrey Alexandrovich", BirthDate = new DateTime(1995, 2, 14) },
            new Passenger { Id = 7, PassportNumber = "MN123789", FullName = "Pavlova Maria Igorevna", BirthDate = new DateTime(1991, 9, 30) },
            new Passenger { Id = 8, PassportNumber = "OP456123", FullName = "Volkov Sergey Pavlovich", BirthDate = new DateTime(1983, 12, 8) },
            new Passenger { Id = 9, PassportNumber = "QR789456", FullName = "Fedorova Olga Dmitrievna", BirthDate = new DateTime(1993, 4, 18) },
            new Passenger { Id = 10, PassportNumber = "ST012345", FullName = "Borisov Alexey Nikolaevich", BirthDate = new DateTime(1989, 6, 22) }
        };
    }

    public static List<Ticket> GenerateTicket(List<Flight> flights, List<Passenger> passengers)
    {
        var flight1 = flights[0];
        var flight2 = flights[1];
        var flight3 = flights[2];
        var flight4 = flights[3];
        var flight5 = flights[4];
        var flight6 = flights[5];
        var flight7 = flights[6];
        var flight8 = flights[7];
        var flight9 = flights[8];
        var flight10 = flights[9];

        var passenger1 = passengers[0];
        var passenger2 = passengers[1];
        var passenger3 = passengers[2];
        var passenger4 = passengers[3];
        var passenger5 = passengers[4];
        var passenger6 = passengers[5];
        var passenger7 = passengers[6];
        var passenger8 = passengers[7];
        var passenger9 = passengers[8];
        var passenger10 = passengers[9];

        return new List<Ticket>
        {
            new Ticket { Id = 1, SeatNumber = "1A", HasHandLuggage = true, LuggageWeight = 15.5, FlightId = flight1.Id, Flight = flight1, PassengerId = passenger1.Id, Passenger = passenger1 },
            new Ticket { Id = 2, SeatNumber = "1B", HasHandLuggage = true, LuggageWeight = 12.0, FlightId = flight1.Id, Flight = flight1, PassengerId = passenger2.Id, Passenger = passenger2 },
            new Ticket { Id = 3, SeatNumber = "2A", HasHandLuggage = false, LuggageWeight = 0, FlightId = flight1.Id, Flight = flight1, PassengerId = passenger3.Id, Passenger = passenger3 },
            new Ticket { Id = 4, SeatNumber = "2B", HasHandLuggage = true, LuggageWeight = 8.5, FlightId = flight1.Id, Flight = flight1, PassengerId = passenger4.Id, Passenger = passenger4 },
            new Ticket { Id = 5, SeatNumber = "3A", HasHandLuggage = true, LuggageWeight = 10.0, FlightId = flight1.Id, Flight = flight1, PassengerId = passenger5.Id, Passenger = passenger5 },

            new Ticket { Id = 6, SeatNumber = "1A", HasHandLuggage = true, LuggageWeight = 14.0, FlightId = flight2.Id, Flight = flight2, PassengerId = passenger6.Id, Passenger = passenger6 },
            new Ticket { Id = 7, SeatNumber = "1B", HasHandLuggage = false, LuggageWeight = 0, FlightId = flight2.Id, Flight = flight2, PassengerId = passenger7.Id, Passenger = passenger7 },
            new Ticket { Id = 8, SeatNumber = "2A", HasHandLuggage = true, LuggageWeight = 9.5, FlightId = flight2.Id, Flight = flight2, PassengerId = passenger8.Id, Passenger = passenger8 },
            new Ticket { Id = 9, SeatNumber = "2B", HasHandLuggage = true, LuggageWeight = 11.0, FlightId = flight2.Id, Flight = flight2, PassengerId = passenger9.Id, Passenger = passenger9 },

            new Ticket { Id = 10, SeatNumber = "1A", HasHandLuggage = true, LuggageWeight = 16.0, FlightId = flight3.Id, Flight = flight3, PassengerId = passenger10.Id, Passenger = passenger10 },
            new Ticket { Id = 11, SeatNumber = "1B", HasHandLuggage = false, LuggageWeight = 0, FlightId = flight3.Id, Flight = flight3, PassengerId = passenger1.Id, Passenger = passenger1 },
            new Ticket { Id = 12, SeatNumber = "2A", HasHandLuggage = true, LuggageWeight = 7.5, FlightId = flight3.Id, Flight = flight3, PassengerId = passenger2.Id, Passenger = passenger2 },

            new Ticket { Id = 13, SeatNumber = "1A", HasHandLuggage = true, LuggageWeight = 20.0, FlightId = flight4.Id, Flight = flight4, PassengerId = passenger3.Id, Passenger = passenger3 },
            new Ticket { Id = 14, SeatNumber = "1B", HasHandLuggage = false, LuggageWeight = 0, FlightId = flight4.Id, Flight = flight4, PassengerId = passenger4.Id, Passenger = passenger4 },

            new Ticket { Id = 15, SeatNumber = "1A", HasHandLuggage = true, LuggageWeight = 18.5, FlightId = flight5.Id, Flight = flight5, PassengerId = passenger5.Id, Passenger = passenger5 },

            new Ticket { Id = 16, SeatNumber = "1A", HasHandLuggage = false, LuggageWeight = 0, FlightId = flight6.Id, Flight = flight6, PassengerId = passenger6.Id, Passenger = passenger6 },
            new Ticket { Id = 17, SeatNumber = "1B", HasHandLuggage = false, LuggageWeight = 0, FlightId = flight6.Id, Flight = flight6, PassengerId = passenger7.Id, Passenger = passenger7 },
            new Ticket { Id = 18, SeatNumber = "2A", HasHandLuggage = false, LuggageWeight = 0, FlightId = flight6.Id, Flight = flight6, PassengerId = passenger8.Id, Passenger = passenger8 },

            new Ticket { Id = 19, SeatNumber = "1A", HasHandLuggage = true, LuggageWeight = 13.0, FlightId = flight7.Id, Flight = flight7, PassengerId = passenger9.Id, Passenger = passenger9 },
            new Ticket { Id = 20, SeatNumber = "1B", HasHandLuggage = true, LuggageWeight = 11.5, FlightId = flight8.Id, Flight = flight8, PassengerId = passenger10.Id, Passenger = passenger10 },
            new Ticket { Id = 21, SeatNumber = "1A", HasHandLuggage = false, LuggageWeight = 0, FlightId = flight9.Id, Flight = flight9, PassengerId = passenger1.Id, Passenger = passenger1 },
            new Ticket { Id = 22, SeatNumber = "1B", HasHandLuggage = true, LuggageWeight = 15.0, FlightId = flight10.Id, Flight = flight10, PassengerId = passenger2.Id, Passenger = passenger2 }
        };
    }
}