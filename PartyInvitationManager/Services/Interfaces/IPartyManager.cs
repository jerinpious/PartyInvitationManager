using PartyInvitationManager.Models;
using PartyInvitationManager.ViewModels;

namespace PartyInvitationManager.Services.Interfaces
{
    public interface IPartyManager
    {
        Task<List<PartyViewModel>> GetAllPartiesAsync();
        Task<Party?> GetPartyByIdAsync(int id);
        Task<ManagePartyViewModel?> GetManagePartyViewModelAsync(int id);
        Task CreatePartyAsync(CreatePartyViewModel model);
        Task UpdatePartyAsync(EditPartyViewModel model);
        Task AddInvitationToPartyAsync(int partyId, string guestName, string guestEmail);
        Task<bool> SendInvitationsAsync(int partyId);
        Task<Invitation?> GetInvitationByIdAsync(int id);
        Task<InvitationResponseViewModel?> GetInvitationResponseViewModelAsync(int id);
        Task UpdateInvitationResponseAsync(int id, bool willAttend);
    }
}