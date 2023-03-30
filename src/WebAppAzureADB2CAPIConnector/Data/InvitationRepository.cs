using System.Collections.Concurrent;
using WebApp.Models;

namespace WebAppAzureADB2CAPIConnector.Data;

public class InvitationRepository
{
    private ConcurrentDictionary<string, string> _invitations = new();

    public void Add(string email, string invitationCode)
    {
        _invitations.AddOrUpdate(email, invitationCode, (key, value) => invitationCode);
    }

    public string Get(string email)
    {
        return _invitations[email];
    }

    public void Remove(string email)
    {
        _invitations.Remove(email, out _);
    }

    public bool Contains(string email)
    {
        return _invitations.ContainsKey(email);
    }

    public List<Invitation> GetAllInvitations()
    {
        return _invitations
            .Select(o => new Invitation()
            {
                Email = o.Key,
                InvitationCode = o.Value
            })
            .ToList();
    }
}
