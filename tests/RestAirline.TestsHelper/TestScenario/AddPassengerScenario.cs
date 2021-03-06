using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using RestAirline.Commands.Passenger;
using RestAirline.Domain.Booking;
using RestAirline.Domain.Booking.Passenger;
using RestAirline.Shared;
using RestAirline.Shared.ModelBuilders;

namespace RestAirline.TestsHelper.TestScenario
{
    public class AddPassengerScenario : ScenarioBase
    {
        private readonly Passenger _passenger;
        
        public AddPassengerScenario(ICommandBus commandBus, BookingId bookingId = null, Passenger passenger = null) :
            base(commandBus)
        {
            CommandBus = commandBus;
            BookingId = bookingId;
            _passenger = passenger;
        }

        public override async Task Execute()
        {
            if (BookingId == null)
            {
                var selectJourneysScenario = new SelectJourneysScenario(CommandBus);
                await selectJourneysScenario.Execute();
                BookingId = selectJourneysScenario.BookingId;
            }
           
            var passenger = _passenger ?? new PassengerBuilder().CreatePassenger();
            var command = new AddPassengerCommand(BookingId)
            {
                Age = passenger.Age,
                Email = passenger.Email,
                Name = passenger.Name,
                PassengerType = passenger.PassengerType
            };

            await CommandBus.PublishAsync(command, CancellationToken.None);
        }
    }
}