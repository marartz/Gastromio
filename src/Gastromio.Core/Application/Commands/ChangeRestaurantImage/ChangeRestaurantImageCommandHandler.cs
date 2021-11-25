using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.RestaurantImages;
using Gastromio.Core.Domain.Model.Users;
using SixLabors.ImageSharp;

namespace Gastromio.Core.Application.Commands.ChangeRestaurantImage
{
    public class ChangeRestaurantImageCommandHandler : ICommandHandler<ChangeRestaurantImageCommand>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IRestaurantImageRepository restaurantImageRepository;

        public ChangeRestaurantImageCommandHandler(
            IRestaurantRepository restaurantRepository,
            IRestaurantImageRepository restaurantImageRepository
        )
        {
            this.restaurantRepository = restaurantRepository;
            this.restaurantImageRepository = restaurantImageRepository;
        }

        public async Task HandleAsync(ChangeRestaurantImageCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.RestaurantAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var restaurant = await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                throw DomainException.CreateFrom(new RestaurantDoesNotExistFailure());

            if (currentUser.Role == Role.RestaurantAdmin && !restaurant.HasAdministrator(currentUser.Id))
                throw DomainException.CreateFrom(new ForbiddenFailure());

            if (string.IsNullOrWhiteSpace(command.Type))
                throw DomainException.CreateFrom(new RestaurantImageTypeRequiredFailure());

            if (command.Image == null)
            {
                await restaurantImageRepository.RemoveByRestaurantIdAndTypeAsync(command.RestaurantId, command.Type,
                    cancellationToken);
                return;
            }

            var types = (await restaurantImageRepository.FindTypesByRestaurantIdAsync(command.RestaurantId,
                cancellationToken))?.ToList();
            if (types != null && types.Contains(command.Type))
            {
                await restaurantImageRepository.RemoveByRestaurantIdAndTypeAsync(command.RestaurantId, command.Type,
                    cancellationToken);
            }

            if (command.Image.Length > 2024 * 2024) // 4 MB
                throw DomainException.CreateFrom(new RestaurantImageDataTooBigFailure());

            try
            {
                using (var srcMemoryStream = new MemoryStream(command.Image))
                using (var dstMemoryStream = new MemoryStream())
                {
                    var imageObj = Image.Load(srcMemoryStream);
                    if (imageObj == null)
                        throw DomainException.CreateFrom(new RestaurantImageNotValidFailure());

                    imageObj.SaveAsJpeg(dstMemoryStream);

                    await dstMemoryStream.FlushAsync(cancellationToken);

                    var restaurantImage = new RestaurantImage(
                        new RestaurantImageId(Guid.NewGuid()),
                        command.RestaurantId,
                        command.Type,
                        dstMemoryStream.GetBuffer(),
                        DateTimeOffset.UtcNow,
                        currentUser.Id,
                        DateTimeOffset.UtcNow,
                        currentUser.Id
                    );

                    await restaurantImageRepository.StoreAsync(restaurantImage, cancellationToken);
                }
            }
            catch
            {
                // TODO: Log error
                throw DomainException.CreateFrom(new RestaurantImageNotValidFailure());
            }
        }
    }
}
