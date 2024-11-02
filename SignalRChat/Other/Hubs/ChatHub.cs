using SignalRChat.Other.Enums;

namespace SignalRChat.Other.Hubs;

public class ChatHub : Hub
{
    private readonly Globals _globals= Globals.Instance;
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _globals.findRow<Members>(("Id", Convert.ToInt32(GetSessionString(nameof(SessionSaveValue.MemberId))))).Online = false;
        await _globals.Db.SaveChangesAsync();        
    }
    public override async Task OnConnectedAsync()
    {
        _globals.findRow<Members>(("Id", Convert.ToInt32(GetSessionString(nameof(SessionSaveValue.MemberId))))).Online= true;
        await _globals.Db.SaveChangesAsync();
    }

    private string GetSessionString(string key) => Context.GetHttpContext()?.Session.GetString(key) ?? "";

    public async Task AnketSonuc(string? gelenSonuc)
    {
        await Clients.All.SendAsync(nameof(HubFuncName.DisabledDocument));
        if (gelenSonuc is null || gelenSonuc is "" || gelenSonuc.Replace(" ", "").Length < 1)
        {

        }
        else
        {
            await Clients.All.SendAsync(nameof(HubFuncName.ReceiveMessage), "Oley Bot",
                $"{GetSessionString(nameof(SessionSaveValue.MemberName))} {gelenSonuc} diyor."
                , "Bot","","");

            Mesajlar oleybot = new()
            {
                Mesaj = $"{GetSessionString(nameof(SessionSaveValue.MemberName))} {gelenSonuc} diyor.",
                MesajAtanId = 0,
                MesajTarihi = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
            };
            await _globals.AddRowAsync(oleybot);
            await _globals.Db.SaveChangesAsync();

        }
        await Clients.All.SendAsync(nameof(HubFuncName.EnabledDocument));
    }
    public async Task SendMessage(string message)
    {
        if (message.Replace(" ", "").Length >= 1)
        {
            int id = int.Parse(GetSessionString(nameof(SessionSaveValue.MemberId)));
            Members members = await _globals.FindRowAsync<Members>(("Id", id));
            bool isBanned = members.isBanned;
            string? yetki = members.Yetki;
            if (!isBanned)
            {
                string image = members.Resim ?? "";
                string user = GetSessionString(nameof(SessionSaveValue.MemberName));
                await Clients.All.SendAsync(nameof(HubFuncName.ReceiveMessage), user, message, yetki, image, DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                Mesajlar mesaj;
                if (yetki == nameof(Auth.Admin) && message.Contains("<anket>"))
                {
                    mesaj = new()
                    {
                        MesajAtanId = 0,
                        Mesaj = GetSessionString(nameof(SessionSaveValue.MemberName)) + " Anket Açtı: " + message.Split("<anket>")[1],
                        MesajTarihi = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                    };
                    await _globals.AddRowAsync(mesaj);
                    await _globals.Db.SaveChangesAsync();
                    return;
                }
                mesaj = new()
                {
                    MesajAtanId = Convert.ToInt32(GetSessionString(nameof(SessionSaveValue.MemberId))),
                    Mesaj = message,
                    MesajTarihi = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                };
                await _globals.AddRowAsync(mesaj);
                await _globals.Db.SaveChangesAsync();
            }
            if (yetki == nameof(Auth.Personel))
            {
                _= TimeOutSendMessage();
            }
        }
    }
    public async Task TimeOutSendMessage()
    {
        var connectionId = Context.ConnectionId;
        await Clients.Client(connectionId).SendAsync(nameof(HubFuncName.DisabledDocument));
        await Clients.Client(connectionId).SendAsync(nameof(HubFuncName.ShowLockIcon));
        Thread.Sleep(1500);
        await Clients.Client(connectionId).SendAsync(nameof(HubFuncName.EnabledDocument));
        await Clients.Client(connectionId).SendAsync(nameof(HubFuncName.ShowUnlockIcon));
    }
    public async Task OnlineStatusBoxOut()
    {       
        Members kullanici = await _globals.FindRowAsync<Members>(("Id", int.Parse(GetSessionString(nameof(SessionSaveValue.MemberId)))));
        await Clients.All.SendAsync(nameof(HubFuncName.OnlineStatusBoxOut), kullanici.Id);
    }
    public async Task YaziyorGosterme()
    {
        await Clients.Others.SendAsync(nameof(HubFuncName.Yaziyor), GetSessionString(nameof(SessionSaveValue.MemberName)));
    }
    public async Task SessionClear()
    {
        _globals.findRow<Members>(("Id", Convert.ToInt32(GetSessionString(nameof(SessionSaveValue.MemberId))))).Online = false;
        await _globals.Db.SaveChangesAsync();
        Context.GetHttpContext()?.Session.Clear();
    }
    public async Task OldMessage()
    {
        var connectionId = Context.ConnectionId;
        await Clients.Client(connectionId).SendAsync(nameof(HubFuncName.DisabledDocument));
        await Clients.Client(connectionId).SendAsync(nameof(HubFuncName.UnShowMessageField));
        //eski mesajların listelenme işlemi
        List<Mesajlar> oldMessage = await _globals.ListRowAsync<Mesajlar>(true);
        foreach (Mesajlar item in oldMessage)
        {
            if (item.MesajAtanId == 0)
            {
                await Clients.Client(connectionId).SendAsync(nameof(HubFuncName.OldMessageList),
                "Oley Bot",
                item.Mesaj,
                "Bot",
                "",
                item.MesajTarihi);
            }
            else
            {
                Members kullanici = await _globals.FindRowAsync<Members>(("Id", item.MesajAtanId));
                await Clients.Client(connectionId).SendAsync(nameof(HubFuncName.OldMessageList),
                    kullanici.Name,
                    item.Mesaj,
                    kullanici.Yetki,
                    kullanici.Resim,
                    item.MesajTarihi
                    );
            }
        }
        await Clients.Client(connectionId).SendAsync(nameof(HubFuncName.EnabledDocument));
        await Clients.Client(connectionId).SendAsync(nameof(HubFuncName.ShowMessageField));
        Members _kullanici = await _globals.FindRowAsync<Members>(("Id", int.Parse(GetSessionString(nameof(SessionSaveValue.MemberId)))));
        await Clients.Others.SendAsync(nameof(HubFuncName.Toast), $"{_kullanici.Name} Sohbete Katıldı");
        _ = Online();
    }
    public async Task Online()
    {
        while (true)
        {
            await using (AppDbContext baglanti = new())
            {
                List<Members> online = [.. baglanti.Members.Where(x => x.Online == true).AsNoTracking()];
                List<Members> offline = [.. baglanti.Members.Where(x => x.Online == false).AsNoTracking()];
                foreach (var member in online)
                {
                    await Clients.All.SendAsync(nameof(HubFuncName.OnlineStatusBox), $"{member.Name}", $"{member.Id}");
                }
                foreach (var member in offline)
                {
                    await Clients.All.SendAsync(nameof(HubFuncName.OnlineStatusBoxOut), member.Id);
                }
            }
            await Task.Delay(50);
        }
    }




    #region Admin
    public async Task UserOut(string haricKisi)
    {
        await Clients.All.SendAsync(nameof(HubFuncName.UserOut), haricKisi);
    }
    public async Task ChatLock(bool locked)
    {
        if (locked)//kilitle
        {
            await Clients.Others.SendAsync(nameof(HubFuncName.DisabledDocument));
        }
        else//aç
        {
            await Clients.Others.SendAsync(nameof(HubFuncName.EnabledDocument));
        }
    }
    #endregion
}