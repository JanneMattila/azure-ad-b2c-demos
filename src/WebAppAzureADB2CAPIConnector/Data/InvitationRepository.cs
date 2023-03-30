using System.Collections.Concurrent;

namespace WebAppAzureADB2CAPIConnector.Data;

public class InvitationRepository
{
    private ConcurrentDictionary<string, string> _invitations = new();

    public void Add(string email, string invitationCode)
    {
        _invitations.AddOrUpdate(email, email, (key, value) => email);
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

    public List<string> GetAllInvitations()
    {
        return _invitations.Keys.ToList();
    }
}
