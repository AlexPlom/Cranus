using Cranus.Collaboration.Contracts.Profiles.Commands;
using Elders.Cronus.DomainModeling;

namespace Cranus.Collaboration.Profiles
{
    public class ProfileAppService : AggregateRootApplicationService<Profile>,
        ICommandHandler<CreateProfile>,
        ICommandHandler<ChangeProfileEmail>
    {
        public void Handle(CreateProfile command)
        {
            var profile = new Profile(command.Id, command.Email);

            Repository.Save(profile);
        }

        public void Handle(ChangeProfileEmail command)
        {
            Update(command.Id, profile => profile.ChangeEmail(command.Email));
        }
    }
}
