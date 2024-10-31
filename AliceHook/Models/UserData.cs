using System.Collections.Generic;
using Google.Cloud.Firestore;

namespace AliceHook.Models
{
    [FirestoreData]
    public class UserData
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Token { get; set; }
        [FirestoreProperty]
        public List<Webhook> Webhooks { get; set; } = new List<Webhook>();
    }
}